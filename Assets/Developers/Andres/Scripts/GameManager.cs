using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _EndGameHUD;

    private EventSystemController _EventSystemController;

    private void Start()
    {
        _EventSystemController = EventSystemController.eventSystemController;
        _EventSystemController.onEndGame += EndGame;
        _EventSystemController.onRestart += Restart;
    }

    private void EndGame()
    {
        _EndGameHUD.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("End Game");
    }

    private void Restart()
    {
        SceneManager.LoadScene(1);
    }
}
