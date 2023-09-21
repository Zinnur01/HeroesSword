using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnAction : UnitAction
{
    [SerializeField]
    private Unit unitToSpawn;

    [SerializeField]
    private float spawnRange = 2;

    public override string Name => $"Spawn {unitToSpawn.name}";

    public override void OnSelect()
    {
        base.OnSelect();

        if (!GetOwner().CanAttack())
        {
            GetOwner().SelectAction(null);
            return;
        }

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
        return GridManager.Instance.GetNeighborsByRadius(GetOwner().GetOccupiedTile(), spawnRange, (tile) => true).ToHashSet();
    }

    protected override void SelectAvailableTile(Tile tile)
    {
        UnitManager.Instance.Spawn(unitToSpawn, tile);
        GetOwner().ReleaseAttack();
    }


    protected override bool ValidateTile(Tile tile)
    {
        return !tile.IsOccupied() && tile.IsWalkable();
    }
}
