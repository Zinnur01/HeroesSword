using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class MoveAction : UnitAction
{
    [SerializeField]
    private int moves;

    // Stored required properties.
    private float currentMoves;

    public override string Name => "Move";

    public override void OnSelect()
    {
        base.OnSelect();
        UpdateAvailableTiles();
    }

    public override void OnTileSelect(Tile tile)
    {
        if (availableTiles.Contains(tile))
        {
            if (!ValidateTile(tile)) return;

            SelectAvailableTile(tile);

            GetOwner().SelectAction(null);
        }
    }

    protected override HashSet<Tile> GetAvailableTiles()
    {
        HashSet<Tile> availableTiles = GridManager.Instance.GetNeighborsByRadius(GetOwner().GetOccupiedTile(), Mathf.CeilToInt(currentMoves * (1 - debuff)), (tile) => tile.IsWalkable()).ToHashSet();
        availableTiles.RemoveWhere(x => !x.IsWalkable());
        return availableTiles;
    }

    protected override void SelectAvailableTile(Tile tile)
    {
        if (MoveUnit(GetOwner(), tile))
        {
            GetOwner().SelectAction(null);
        }
    }

    public override void OnStartGameTurn()
    {
        base.OnStartGameTurn();
        currentMoves = moves;
    }

    private bool MoveUnit(Unit unit, Tile tile)
    {
        if (tile.SetUnit(unit))
        {
            currentMoves -= tile.Depth;
            return true;
        }

        return false;
    }
}
