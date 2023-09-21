using System;
using UnityEngine;

public class UnitGroupHealth : UnitHealth
{
    [SerializeField]
    private int unitCount;

    [Header("References")]
    [SerializeField]
    private TextMesh unitCountText;

    // Stored required properties.
    private int currentUnitCount;

    protected override void Awake()
    {
        base.Awake();
        currentUnitCount = unitCount;
        UpdateView();
    }

    public override void TakeDamage(int damage)
    {
        int lastUnitCount = currentUnitCount;
        base.TakeDamage(damage);

        currentUnitCount = Mathf.CeilToInt((float)GetCurrentHealth() / GetHealth());

        int deathCount = lastUnitCount - currentUnitCount;
        if (deathCount > 0)
        {
            GroupDeathEvent(deathCount);
            OnGroupDeathCallback?.Invoke(deathCount);
        }

        UpdateView();
    }

    private void GroupDeathEvent(int groupCount)
    {
        foreach (HealthEvent healthEvent in healthEvents)
        {
            healthEvent.OnGroupDeath(groupCount);
        }
    }

    private void UpdateView()
    {
        unitCountText.text = currentUnitCount.ToString();
    }

    #region [Events]
    public event Action<int> OnGroupDeathCallback;
    #endregion

    #region [UnitHealth Implementation]
    public override int GetMaxHealth()
    {
        return base.GetMaxHealth() * unitCount;
    }
    #endregion

    #region [Getter / Setter]
    public int GetCurrectUnitCount()
    {
        return currentUnitCount;
    }

    public void SetUnitCount(int count)
    {
        unitCount = count;
        currentUnitCount = count;

        UpdateView();
    }
    #endregion
}
