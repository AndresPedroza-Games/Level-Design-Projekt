using System.Collections;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController uiController;

    [SerializeField] private TMP_Text _TextBox;

    private void Awake()
    {
        if (uiController == null)
            uiController = this;
    }

    public void ChangeTextBox(string message)
    {
        _TextBox.text = message;

        StartCoroutine(EmptyTextBox());
    }

    private IEnumerator EmptyTextBox()
    {
        yield return new WaitForSeconds(2f);
        _TextBox.text = "";
    }
}
