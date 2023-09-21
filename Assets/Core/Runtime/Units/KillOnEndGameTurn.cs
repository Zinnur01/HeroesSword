using UnityEngine;

public class KillOnEndGameTurn : MonoBehaviour
{
    // Stored required components.
    private UnitHealth health;

    private void Awake()
    {
        health = GetComponent<UnitHealth>();
        GameManager.Instance.OnEndGameTurn += OnEndGameTurn;
    }

    private void OnEndGameTurn()
    {
        if (health != null)
        {
            health.Kill();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
