public class ResurrectionEvent : HealthEvent
{
    // Stored required properties.
    private int useCount = 0;

    public override void OnGroupDeath(int deathGroupCount)
    {
        base.OnGroupDeath(deathGroupCount);

        useCount++;

        float debuff = 0;
        if (useCount == 1)
        {
            debuff = .3f;
        }
        else if (useCount == 2)
        {
            debuff = .6f;
        }
        else if (useCount == 3)
        {
            debuff = .9f;
        }

        Tile tile = GridManager.Instance.GetRangomNeighbour(owner.GetOccupiedTile(), (tile) => !tile.IsOccupied());
        if (tile != null)
        {
            Unit cloneUnit = UnitManager.Instance.SpawnClone(owner, tile, deathGroupCount, debuff);
            UnitHealth cloneHealth = cloneUnit.GetComponent<UnitHealth>();
            ResurrectionEvent resurrection = cloneUnit.GetComponentInChildren<ResurrectionEvent>();
            if (resurrection != null)
            {
                cloneHealth.RemoveEvent(resurrection);
            }
        }
    }
}
