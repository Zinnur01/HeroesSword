using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    [SerializeField]
    private Text winText;

    private void OnEnable()
    {
        GameManager.Instance.OnWin += OnWin;
    }

    private void OnWin(Fraction fraction)
    {
        GetComponent<Image>().enabled = true;
        winText.text = fraction == Fraction.Red ? "RED WIN" : "BLUE WIN";
    }
}
