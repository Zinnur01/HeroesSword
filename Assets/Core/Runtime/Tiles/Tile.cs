using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour, IAStarNode<Tile>, IDepthSearch
{
    [SerializeField]
    private Transform unitContianer;

    [SerializeField]
    private SpriteRenderer hightlight;

    [SerializeField]
    private bool isWaikable;

    // Stored required components.
    private Unit occupiedUnit;

    // Stored required properties.
    private Vector2Int index2D;

    private void Awake()
    {
        index2D = GridManager.Instance.RegisterTile(this);
        gameObject.name = gameObject.name + $" {index2D.x}:{index2D.y}";
    }

    private void OnDestroy()
    {
        GridManager.Instance.UnregisterTile(this);
    }

    public void HightlightOn(Color color)
    {
        color.a = .5f;
        hightlight.gameObject.SetActive(true);
        hightlight.color = color;
    }

    public void HightlightOff()
    {
        hightlight.gameObject.SetActive(false);
    }

    public virtual bool IsWalkable()
    {
        return false;
    }

    #region [Unit]
    public bool SetUnit(Unit unit)
    {
        if (occupiedUnit != null)
        {
            return false;
        }

        occupiedUnit = unit;
        if (unit != null)
        {
            unit.SetOccupiedTile(this);
        }
        return true;
    }

    public void ClearSetOccupied()
    {
        occupiedUnit = null;
    }

    public Unit GetOccupiedUnit()
    {
        return occupiedUnit;
    }

    public bool IsOccupied()
    {
        return occupiedUnit != null;
    }
    #endregion

    #region [Pointer Events]
    public void OnPointEnter()
    {
        GameManager.Instance.HoverTile(this);
        //Hightlight(true);
    }

    public void OnPointExit()
    {
        //Hightlight(false);
    }

    public void OnPointDown()
    {
        GameManager.Instance.SelectTitle(this);
    }

    public void OnPointUp()
    {
        
    }
    #endregion

    #region [IAStarNode Implementation]
    private int gCost;
    private int hCost;
    private Tile parent;
    private int heapIndex;

    public Vector2 Position => transform.position;
    public bool Walkable => IsWalkable();

    public int GCost { get => gCost; set => gCost = value; }
    public int HCost { get => hCost; set => hCost = value; }
    public Tile Parent { get => parent; set => parent = value; }
    public int HeapIndex { get => heapIndex; set => heapIndex = value; }

    public IEnumerable<Tile> GetNeighbours()
    {
        foreach (Tile neighbour in GridManager.Instance.GetNeighbors(this))
        {
            yield return neighbour;
        }
    }

    public int CompareTo(Tile other)
    {
        return (other.GCost + other.HCost).CompareTo(GCost + HCost);
    }
    #endregion=

    #region [IDepthSearch Implementation]
    private int depth;
    public int Depth { get => depth; set => depth = value; }
    #endregion
}