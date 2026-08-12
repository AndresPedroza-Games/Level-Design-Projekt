using System.Collections;
using TMPro;
using UnityEngine;


public class UIController : MonoBehaviour
{
    public static UIController uiController;

    [Header("---Interface---")]
    [SerializeField] private GameObject crosshair;

    [SerializeField] private TMP_Text _TextBox;

    private void Awake()
    {
        if (uiController == null)
            uiController = this;
    }


    private void OnEnable() {
	    EventSystemController.eventSystemController.onRestart += OnRestart;
	    EventSystemController.eventSystemController.onEndGame += OnEndGame;
    }


    private void OnDisable() {
	    EventSystemController.eventSystemController.onRestart -= OnRestart;
	    EventSystemController.eventSystemController.onEndGame -= OnEndGame;
	    
	    
    }


    private void OnRestart() {
	    crosshair.SetActive(true);
    }
    
    
    private void OnEndGame() {
	    crosshair.SetActive(false);
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
