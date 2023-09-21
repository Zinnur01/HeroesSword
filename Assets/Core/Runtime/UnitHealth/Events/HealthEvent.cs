using UnityEngine;

public class HealthEvent : MonoBehaviour
{
    // Stored required components.
    protected Unit owner;
    protected UnitHealth health;

    public virtual void Initialize(UnitHealth health)
    {
        owner = health.GetComponent<Unit>();
        this.health = health;
    }

    public virtual void OnTakeDamage(int damage) { }
    public virtual void OnGroupDeath(int deathGroupCount) { }
    public virtual void OnDeath() { }
}
