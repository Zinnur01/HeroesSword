using UnityEngine;

public class HealthFilter : MonoBehaviour
{
    public virtual void FilterDamage(ref int damage) { }
}
