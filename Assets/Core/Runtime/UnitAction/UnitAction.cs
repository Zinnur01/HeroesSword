using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAction : MonoBehaviour
{
    // Stored required components.
    private Unit owner;
    protected HashSet<Tile> availableTiles;

    // Stored required properties.
    protected float debuff;

    public virtual void Initialize(Unit unit)
    {
        owner = unit;
    }

    public abstract string Name { get; }

    public virtual void OnSelect() { }

    public virtual void OnDeselect()
    {
        ResetAvailableTiles();
    }

    public virtual void OnTileSelect(Tile tile) { }

    public virtual void OnTileHover(Tile tile) { }

    public virtual void OnStartGameTurn() { }
    public virtual void OnEndGameTurn() { }
    public virtual void OnStartUnitTurn() { }
    public virtual void OnEndUnitTurn() { }

    protected void UpdateAvailableTiles()
    {
        ResetAvailableTiles();

        availableTiles = GetAvailableTiles();

        if (availableTiles == null) return; 

        foreach (Tile tile in availableTiles)
        {
            if (ValidateTile(tile))
            {
                tile.HightlightOn(Color.green);
            }
            else
            {
                tile.HightlightOn(Color.yellow);
            }
        }
    }

    protected void ResetAvailableTiles()
    {
        if (availableTiles != null)
        {
            foreach (Tile tile in availableTiles)
            {
                tile.HightlightOff();
            }
        }
    }

    protected abstract HashSet<Tile> GetAvailableTiles();

    protected abstract void SelectAvailableTile(Tile tile);

    protected virtual bool ValidateTile(Tile tile)
    {
        return true;
    }

    #region [Getter / Setter]
    public Unit GetOwner()
    {
        return owner;
    }

    public void SetDebuff(float debuff)
    {
        this.debuff = Mathf.Clamp01(debuff);
    }
    #endregion
}