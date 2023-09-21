using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class MeleeAttackAction : UnitAction
{
    [SerializeField]
    private Vector2Int damageRange;

    public override string Name => "Melee Attack";

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
        return GridManager.Instance.GetNeighborsByRadius(
            GetOwner().GetOccupiedTile(),
            1,
            (tile) => true).ToHashSet();
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

    protected override bool ValidateTile(Tile tile)
    {
        return tile.IsOccupied() && tile.GetOccupiedUnit().GetCurrentFraction() != GetOwner().GetCurrentFraction();
    }
}
