using UnityEngine;

public class Singleton<TMono> : MonoBehaviour where TMono : MonoBehaviour
{
    private static TMono instance;
    public static TMono Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<TMono>();
                if (instance == null)
                {
                    Debug.LogError($"There is no {typeof(TMono)} on the stage. Create it manually.");
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        instance = this as TMono;
    }
}
