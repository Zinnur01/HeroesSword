using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlameThrowAction : UnitAction
{
    [SerializeField]
    private Unit flameUnit;

    [SerializeField]
    private Vector2Int damageRange;

    [SerializeField]
    private int range;

    public override string Name => "Flamethrower";

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
        return GridManager.Instance.GetNeighborsByRadius(GetOwner().GetOccupiedTile(), range, (tile) => true).ToHashSet();
    }

    protected override void SelectAvailableTile(Tile tile)
    {
        List<Tile> path = GridManager.Instance.GetPath(GetOwner().GetOccupiedTile(), tile, (tile) => true)
            ;
        foreach (Tile t in path)
        {
            if (t.IsOccupied())
            {
                Unit target = t.GetOccupiedUnit();
                int damage = Mathf.CeilToInt(Random.Range(damageRange.x, damageRange.y) * (1 - debuff));

                UnitGroupHealth groupHealth = GetOwner().GetComponent<UnitGroupHealth>();

                UnitHealth health = target.GetComponent<UnitHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage * groupHealth.GetCurrectUnitCount());
                }
            }
            else
            {
                UnitManager.Instance.Spawn(flameUnit, t);
            }
        }

        GetOwner().ReleaseAttack();
    }
}
