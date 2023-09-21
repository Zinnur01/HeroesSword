using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionsUI : MonoBehaviour
{
    [SerializeField]
    private Button buttonPrefab;

    [SerializeField]
    private Text actionText;

    // Stored required components.
    private Unit currentUnit;
    private List<Button> actionButtons = new List<Button>();

    private void OnEnable()
    {
        GameManager.Instance.OnStartUnitTurn += OnStartUnitTurn;
        GameManager.Instance.OnEndUnitTurn += OnEndUnitTurn;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnStartUnitTurn -= OnStartUnitTurn;
        GameManager.Instance.OnEndUnitTurn -= OnEndUnitTurn;
    }

    private void OnStartUnitTurn(Unit unit)
    {
        currentUnit = unit;
        UpdateActions();
    }

    private void OnEndUnitTurn(Unit unit)
    {
        actionText.text = string.Empty;
    }

    private void UpdateActions()
    {
        for (int i = 0; i < actionButtons.Count; i++)
        {
            Destroy(actionButtons[i].gameObject);
        }

        actionButtons.Clear();

        if (currentUnit == null) return;

        foreach (UnitAction action in currentUnit.GetAvailableActions())
        {
            Button button = Instantiate(buttonPrefab);
            actionButtons.Add(button);
            button.transform.SetParent(transform);
            button.GetComponentInChildren<Text>().text = action.Name;

            button.onClick.AddListener(() => OnClick(currentUnit, action));
        }
    }

    private void OnClick(Unit unit, UnitAction action)
    {
        actionText.text = action.Name;
        unit.SelectAction(action);
    }
}
