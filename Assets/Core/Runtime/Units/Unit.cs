using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[DisallowMultipleComponent]
public class Unit : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform highlight;

    [SerializeField]
    private TextMesh nameText;

    [SerializeField]
    private TextMesh unitCountText;

    [SerializeField]
    private SpriteRenderer centerSprite;

    [Header("Stats")]
    [SerializeField]
    private Fraction fraction;

    [SerializeField]
    private int initiative;

    [SerializeField]
    [Range(0f, 1f)]
    private float doubleTurnChange;

    // Stored required components.
    private Tile occupiedTile;
    private UnitAction[] availableActions;
    private UnitAction currentAction;

    // Stored required properties.
    private Fraction currectFraction;
    private bool canAttack;
    private float debuff = 0;
    private bool fractionSwitched;

    private void Awake()
    {
        LoadUnitActions();
    }

    private void Start()
    {
        currectFraction = fraction;
        UpdateView();
        nameText.text = gameObject.name;
    }

    public void SwitchFraction()
    {
        fractionSwitched = true;
        currectFraction = currectFraction == Fraction.Blue ? Fraction.Red : Fraction.Blue;
        UpdateView();
    }

    private void LoadUnitActions()
    {
        availableActions = GetComponentsInChildren<UnitAction>();
        for (int i = 0; i < availableActions.Length; i++)
        {
            availableActions[i].Initialize(this);
        }
    }

    private void UpdateView()
    {
        centerSprite.color = currectFraction == Fraction.Red ? Color.red : Color.blue;
    }

    public void OnStartGameTurn()
    {

        for (int i = 0; i < availableActions.Length; i++)
        {
            availableActions[i].OnStartGameTurn();
        }
    }

    public void OnEndGameTurn()
    {
        for (int i = 0; i < availableActions.Length; i++)
        {
            availableActions[i].OnEndGameTurn();
        }
    }

    public void OnStartUnitTurn()
    {
        canAttack = true;

        if (highlight != null)
        {
            highlight.gameObject.SetActive(true);
        }

        for (int i = 0; i < availableActions.Length; i++)
        {
            availableActions[i].OnStartUnitTurn();
        }
    }

    public void OnEndUnitTurn()
    {
        if (highlight != null)
        {
            highlight.gameObject.SetActive(false);
        }

        if (currentAction != null)
        {
            SelectAction(null);
        }

        for (int i = 0; i < availableActions.Length; i++)
        {
            availableActions[i].OnEndUnitTurn();
        }

        if (fractionSwitched)
        {
            currectFraction = fraction;
            UpdateView();
        }
    }

    public void SetOccupiedTile(Tile tile)
    {
        if (occupiedTile != null)
        {
            occupiedTile.ClearSetOccupied();
        }

        occupiedTile = tile;
        transform.position = tile.transform.position;
    }

    public Tile GetOccupiedTile()
    {
        return occupiedTile;
    }

    public void SelectAction(UnitAction action)
    {
        if (currentAction != null)
        {
            currentAction.OnDeselect();
        }

        currentAction = action;
        if (currentAction != null)
        {
            currentAction.OnSelect();
        }
    }

    public void ReleaseAttack()
    {
        canAttack = false;
    }

    #region [Getter / Setter]
    public Fraction GetCurrentFraction()
    {
        return currectFraction;
    }

    public int GetInitiative()
    {
        return (int)(initiative * (1 - debuff));
    }

    public bool CanAttack()
    {
        return canAttack;
    }

    public UnitAction[] GetAvailableActions()
    {
        return availableActions;
    }

    public UnitAction GetCurrectAction()
    {
        return currentAction;
    }

    public void SetDebuff(float debuff)
    {
        this.debuff = Mathf.Clamp01(debuff);

        for (int i = 0; i < availableActions.Length; i++)
        {
            availableActions[i].SetDebuff(debuff);
        }
    }

    public float GetDoubleTurnChange()
    {
        return doubleTurnChange;
    }
    #endregion
}
