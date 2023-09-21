using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BowAttackAction : UnitAction
{
    [SerializeField]
    private Vector2Int damageRange;

    [SerializeField]
    private int minRange;

    [SerializeField]
    private int maxRange;

    public override string Name => "Bow Attack";

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
        HashSet<Tile> availableTiles = GridManager.Instance.GetNeighborsByRadius(GetOwner().GetOccupiedTile(), maxRange, (tile) => true).ToHashSet();
        availableTiles.RemoveWhere(x => x.Depth < minRange);
        return availableTiles;
    }

    protected override bool ValidateTile(Tile tile)
    {
        return tile.IsOccupied() && tile.GetOccupiedUnit().GetCurrentFraction() != GetOwner().GetCurrentFraction();
    }

    protected override void SelectAvailableTile(Tile tile)
    {
        Unit target = tile.GetOccupiedUnit();
        int damage = Mathf.CeilToInt(Random.Range(damageRange.x, damageRange.y) * (1 - debuff));

        UnitGroupHealth groupHealth = GetOwner().GetComponent<UnitGroupHealth>();

        UnitHealth health = target.GetComponent<UnitHealth>();
        if (health != null)
        {
            health.TakeDamage(damage * groupHealth.GetCurrectUnitCount());
        }

        GetOwner().ReleaseAttack();
    }
}
