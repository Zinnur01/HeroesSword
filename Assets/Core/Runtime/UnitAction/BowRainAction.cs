using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]
public class BowRainAction : UnitAction
{
    [SerializeField]
    private Vector2Int damageRange;

    // Stored required properties.
    private Tile hoveredTile;

    public override string Name => "Bow Rain";

    protected override HashSet<Tile> GetAvailableTiles()
    {
        if (hoveredTile == null) return null;

        HashSet<Tile> availableTiles = GridManager.Instance.GetNeighborsByRadius(hoveredTile, 1, (tile) => true).ToHashSet();

        Tile t1 = GridManager.Instance.GetTileByOffset(hoveredTile, new Vector2Int(0, 2));
        IEnumerable<Tile> r1 = GridManager.Instance.GetNeighborsByRadius(t1, 1, (tile) => true);
        availableTiles.AddRange(r1);
        availableTiles.Add(hoveredTile);
        availableTiles.Add(t1);

        return availableTiles;
    }

    public override void OnTileSelect(Tile tile)
    {
        if (availableTiles != null)
        {
            HashSet<Tile> hitTiles = new HashSet<Tile>();
            foreach (Tile t in availableTiles)
            {
                if (!ValidateTile(t)) continue;

                hitTiles.Add(t);
            }

            foreach (Tile t in hitTiles)
            {
                SelectAvailableTile(t);
            }

            GetOwner().ReleaseAttack();
            GetOwner().SelectAction(null);
        }
    }

    protected override void SelectAvailableTile(Tile tile)
    {
        Unit target = tile.GetOccupiedUnit();
        int damage = Random.Range(damageRange.x, damageRange.y);

        UnitGroupHealth groupHealth = GetOwner().GetComponent<UnitGroupHealth>();

        UnitHealth health = target.GetComponent<UnitHealth>();
        if (health != null)
        {
            health.TakeDamage(damage * groupHealth.GetCurrectUnitCount());
        }
    }

    public override void OnTileHover(Tile tile)
    {
        base.OnTileHover(tile);

        if (!GetOwner().CanAttack())
        {
            GetOwner().SelectAction(null);
            return;
        }

        hoveredTile = tile;
        UpdateAvailableTiles();
    }

    protected override bool ValidateTile(Tile tile)
    {
        return tile.IsOccupied() && tile.GetOccupiedUnit().GetCurrentFraction() != GetOwner().GetCurrentFraction();
    }
}
