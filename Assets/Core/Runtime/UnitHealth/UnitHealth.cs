using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    private int health;

    // Stored required components.
    private Unit owner;
    protected HealthFilter[] healthFilters;
    protected HashSet<HealthEvent> healthEvents;

    // Stored required properties.
    private int currentHealth;
    private float debuff;

    protected virtual void Awake()
    {
        owner = GetComponent<Unit>();
        LoadHealthFilters();
        LoadHealthEvents();
    }

    private void Start()
    {
        currentHealth = GetMaxHealth();
    }

    private void LoadHealthFilters()
    {
        healthFilters = GetComponentsInChildren<HealthFilter>();
    }

    private void LoadHealthEvents()
    {
        healthEvents = GetComponentsInChildren<HealthEvent>().ToHashSet();
        foreach (HealthEvent healthEvent in healthEvents)
        {
            healthEvent.Initialize(this);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        damage = Mathf.Max(0, damage);

        ApplyDamageFilter(ref damage);

        currentHealth = Mathf.Max(currentHealth - damage, 0);
        Logger.Instance.Log($"{gameObject.name} take <color=red>{damage}</color> damage!");
        TakeDamageEvent(damage);

        Debug.Log($"{name}: {damage} damage");
        OnDamageCallback?.Invoke();

        if (currentHealth <= 0)
        {
            OnDeathCallback?.Invoke();
            OnDeath();
        }
    }

    private void ApplyDamageFilter(ref int damage)
    {
        for (int i = 0; i < healthFilters.Length; i++)
        {
            healthFilters[i].FilterDamage(ref damage);
        }
    }

    private void TakeDamageEvent(int damage)
    {
        foreach (HealthEvent healthEvent in healthEvents)
        {
            healthEvent.OnTakeDamage(damage);
        }
    }

    private void DeathEvent()
    {
        foreach (HealthEvent healthEvent in healthEvents)
        {
            healthEvent.OnDeath();
        }
    }

    public void Kill()
    {
        TakeDamage(currentHealth);
    }

    protected virtual void OnDeath()
    {
        UnitManager.Instance.OnUnitDeath(owner);
        Destroy(gameObject);
    }

    #region [Health Event]
    public bool RemoveEvent(HealthEvent healthEvent)
    {
        if (healthEvents.Contains(healthEvent))
        {
            healthEvents.Remove(healthEvent);
            Destroy(healthEvent);
            return true;
        }
        return false;
    }
    #endregion

    #region [Events]
    public event Action OnDamageCallback;
    public event Action OnDeathCallback;
    #endregion

    #region [Getter / Setter]
    public int GetHealth()
    {
        return Mathf.CeilToInt(health * (1f - debuff));
    }

    public virtual int GetMaxHealth()
    {
        return GetHealth();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetDebuff(float debuff)
    {
        this.debuff = Mathf.Clamp01(debuff);

        int maxHealth = GetMaxHealth();
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    #endregion
}
