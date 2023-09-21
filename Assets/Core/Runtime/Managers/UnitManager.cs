using System.Collections.Generic;
using System.Linq;

public class UnitManager : Singleton<UnitManager>
{
    // Stored required properties.
    private List<Unit> units = new List<Unit>();

    public Unit Spawn(Unit unit, Tile placeTile)
    {
        Unit spawnedUnit = Instantiate(unit);
        spawnedUnit.name = spawnedUnit.name.Replace("(Clone)", "");
        units.Add(spawnedUnit);
        placeTile.SetUnit(spawnedUnit);
        return spawnedUnit;
    }

    public Unit SpawnClone(Unit unit, Tile placeTile, int unitCount, float debuff)
    {
        Unit clonedUnit = Spawn(unit, placeTile);
        clonedUnit.SetDebuff(debuff);

        UnitGroupHealth groupHealth = clonedUnit.GetComponent<UnitGroupHealth>();
        groupHealth.SetUnitCount(unitCount);
        groupHealth.SetDebuff(debuff);

        return clonedUnit;
    }

    public void OnUnitDeath(Unit unit)
    {
        units.Remove(unit);

        if (units.All(u => u.GetCurrentFraction() == Fraction.Red))
        {
            GameManager.Instance.WinTeam(Fraction.Red);
        }

        if (units.All(u => u.GetCurrentFraction() == Fraction.Blue))
        {
            GameManager.Instance.WinTeam(Fraction.Blue);
        }
    }

    #region [Getter / Setter]
    public List<Unit> GetUnits()
    {
        return units;
    }

    public Queue<Unit> GetUnitsByInitiative()
    {
        return new Queue<Unit>(units.OrderByDescending(u => u.GetInitiative()));
    }
    #endregion
}
