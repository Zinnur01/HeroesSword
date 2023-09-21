using UnityEngine;
using UnityEngine.UI;

public class LogMessage : MonoBehaviour
{
    [SerializeField]
    private Text textField;

    public void Display(string text)
    {
        textField.text = text;
    }
}
