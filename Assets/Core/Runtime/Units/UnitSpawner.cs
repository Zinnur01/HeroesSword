using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Tile))]
public class UnitSpawner : MonoBehaviour
{
    [SerializeField]
    private Unit unitToStawn;

    private void Awake()
    {
        Tile tile = GetComponent<Tile>();
        UnitManager.Instance.Spawn(unitToStawn, tile);
    }

    #region [Editor]
#if UNITY_EDITOR
    private static GUIStyle textStyle;

    private void OnDrawGizmos()
    {
        if (textStyle == null)
        {
            textStyle = new GUIStyle();
            textStyle.alignment = TextAnchor.MiddleCenter;
            textStyle.fontSize = 16;
            textStyle.normal.textColor = Color.black;
            textStyle.fontStyle = FontStyle.Bold;
        }

        if (!Application.isPlaying)
        {
            Handles.color = Color.black;
            if (unitToStawn != null)
            {
                Handles.Label(transform.position, unitToStawn.name, textStyle);
            }
            else
            {
                Handles.Label(transform.position, "None", textStyle);
            }
        }
    }
#endif
    #endregion

}
