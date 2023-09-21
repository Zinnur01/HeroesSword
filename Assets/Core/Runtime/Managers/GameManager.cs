using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // Stored required properties.
    private Queue<Unit> queue;
    private Unit currentUnit;

    private void Start()
    {
        StartGameTurn();
    }

    private void StartGameTurn()
    {
        Debug.Log("Start Game Turn");

        foreach (Unit unit in UnitManager.Instance.GetUnits())
        {
            unit.OnStartGameTurn();
        }

        queue = UnitManager.Instance.GetUnitsByInitiative();
        OnStartGameTurn?.Invoke();
        NextTurn();
    }

    private void EndGameTurn()
    {
        foreach (Unit unit in UnitManager.Instance.GetUnits())
        {
            unit.OnEndGameTurn();
        }

        OnEndGameTurn?.Invoke();

        StartGameTurn();
    }

    private void NextTurn()
    {
        if (TryDoubleTurn())
        {
            Logger.Instance.Log($"<color={(currentUnit.GetCurrentFraction() == Fraction.Red ? Color.red : Color.blue)}>{currentUnit.gameObject.name}</color> makes a repeat move!");
            StartTurn();
        }
        else
        {
            if (queue.Count > 0)
            {
                currentUnit = queue.Dequeue();
                if (currentUnit == null)
                {
                    NextTurn();
                    return;

                }
                StartTurn();
            }
            else
            {
                EndGameTurn();
            }
        }
    }

    private bool TryDoubleTurn()
    {
        if (currentUnit == null)
        {
            return false;
        }

        if (currentUnit.GetDoubleTurnChange() != 0)
        {
            float change = currentUnit.GetDoubleTurnChange();

            if (UnityEngine.Random.value < change)
            {
                return true;
            }
        }

        return false;
    }

    public void StartTurn()
    {
        Debug.Log("Start Unit Turn");
        currentUnit.OnStartUnitTurn();
        OnStartUnitTurn?.Invoke(currentUnit);
    }

    public void EndTurn()
    {
        if (currentUnit != null)
        {
            Debug.Log("End Unit Turn");
            currentUnit.OnEndUnitTurn();
            OnEndUnitTurn?.Invoke(currentUnit);
        }

        NextTurn();
    }

    public void WinTeam(Fraction fraction)
    {
        OnWin?.Invoke(fraction);
    }

    internal void SelectTitle(Tile tile)
    {
        if (currentUnit != null)
        {
            UnitAction action = currentUnit.GetCurrectAction();
            if (action != null)
            {
                action.OnTileSelect(tile);
            }
        }
    }

    internal void HoverTile(Tile tile)
    {
        if (currentUnit != null)
        {
            UnitAction action = currentUnit.GetCurrectAction();
            if (action != null)
            {
                action.OnTileHover(tile);
            }
        }
    }

    #region [Events]
    public event Action<Unit> OnStartUnitTurn;
    public event Action<Unit> OnEndUnitTurn;
    public event Action OnStartGameTurn;
    public event Action OnEndGameTurn;
    public event Action<Fraction> OnWin;
    #endregion
}
