using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class GridManager : Singleton<GridManager>
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private Grid grid;

    // Stored required properties.
    private LayerMask uiLayer;
    private Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
    private Tile lastHoverTile;
    private Tile holdedTile;

    protected override void Awake()
    {
        base.Awake();
        uiLayer = LayerMask.NameToLayer("UI");
    }

    private void Update()
    {
        if (IsPointerOverUIElement())
        {
            return;
        }

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Tile selectedTile = Index2DToTile(PositionToIndex2D(mousePosition));

        if (selectedTile != lastHoverTile)
        {
            if (lastHoverTile != null)
            {
                lastHoverTile.OnPointExit();
            }

            if (selectedTile != null)
            {
                selectedTile.OnPointEnter();
            }

            lastHoverTile = selectedTile;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (selectedTile != null)
            {
                holdedTile = selectedTile;
                selectedTile.OnPointDown();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedTile != null && selectedTile == holdedTile)
            {
                selectedTile.OnPointUp();
            }
            holdedTile = null;
        }
    }

    #region [UI Handler]
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == uiLayer)
                return true;
        }
        return false;
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
    #endregion

    #region [Tile Manipulations]
    public List<Tile> GetPath(Tile start, Tile end, Func<Tile, bool> validation)
    {
        return new AStar<Tile>(100).FindPath(start, end, validation);
    }

    public IEnumerable<Tile> GetNeighborsByRadius(Tile tile, float radius, Func<Tile, bool> validation)
    {
        List<Tile> open = new List<Tile>();
        List<Tile> closed = new List<Tile>();

        tile.Depth = 0;
        open.Add(tile);
        while (open.Count > 0)
        {
            Tile current = open[0];
            closed.Add(current);
            open.Remove(current);

            if (current.Depth == radius) continue;

            foreach (Tile neighbour in GetNeighbors(current as Tile))
            {
                if (closed.Contains(neighbour) || !validation(neighbour))
                {
                    continue;
                }

                if (!open.Contains(neighbour))
                {
                    neighbour.Depth = current.Depth + 1;
                    open.Add(neighbour);
                }
            }
        }

        closed.Remove(tile);

        return closed;
    }

    public IEnumerable<Tile> GetNeighbors(Tile tile)
    {
        Vector2Int index2D = TileToIndex2D(tile);
        foreach (Vector2Int offset in GetNeighborsOffsets(tile))
        {
            Vector2Int offsetIndex2D = index2D + offset;
            Tile neightbout = Index2DToTile(offsetIndex2D);
            if (neightbout != null)
            {
                yield return neightbout;
            }
        }
    }

    public Tile GetTileByOffset(Tile tile, Vector2Int offset)
    {
        Vector2Int index2D = TileToIndex2D(tile);
        Vector2Int offsetIndex2D = index2D + offset;
        return Index2DToTile(offsetIndex2D);
    }

    public Tile GetRangomNeighbour(Tile tile, Func<Tile, bool> validation)
    {
        foreach (Tile neighbour in GetNeighbors(tile))
        {
            if (validation(neighbour))
            {
                return neighbour;
            }
        }
        return null;
    }

    public IEnumerable<Vector2Int> GetNeighborsOffsets(Tile tile)
    {
        Vector2Int index2D = TileToIndex2D(tile);
        bool even = index2D.y % 2 == 0;

        // Top
        yield return new Vector2Int(even ? -1 : 0, 1);
        yield return new Vector2Int(even ? 0 : 1, 1);

        // Bottom
        yield return new Vector2Int(even ? -1 : 0, -1);
        yield return new Vector2Int(even ? 0 : 1, -1);

        //Right
        yield return new Vector2Int(1, 0);

        //Left
        yield return new Vector2Int(-1, 0);
    }
    #endregion

    #region [Tile / Position / Index Operations]
    public Vector2Int Index3DTo2D(Vector3Int index3D)
    {
        return new Vector2Int(index3D.x, index3D.y);
    }

    public Vector2Int PositionToIndex2D(Vector3 position)
    {
        Vector3Int index3D = grid.WorldToCell(position);
        return Index3DTo2D(index3D);
    }

    public Vector2Int TileToIndex2D(Tile tile)
    {
        return PositionToIndex2D(tile.transform.position);
    }

    public Tile Index2DToTile(Vector2Int index2D)
    {
        if (tiles.TryGetValue(index2D, out Tile tile))
        {
            return tile;
        }
        return null;
    }
    #endregion

    #region [Tile registration]
    public Vector2Int RegisterTile(Tile tile)
    {
        Vector2Int index2D = TileToIndex2D(tile);
        tiles.Add(index2D, tile);
        return index2D;
    }

    public void UnregisterTile(Tile tile)
    {
        tiles.Remove(TileToIndex2D(tile));
    }
    #endregion
}