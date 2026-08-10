using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _StartBtn;
    [SerializeField] private Button _ExitBtn;
    [SerializeField] private string sceneToLoadOnStart;

    private void Awake()
    {
        _StartBtn.onClick.AddListener(OnStart);
        _ExitBtn.onClick.AddListener(OnExit);
    }

    private void OnStart()
    {
        SceneManager.LoadScene(sceneToLoadOnStart);
        Time.timeScale = 1f;
    }

    private void OnExit()
    {
        Application.Quit();
    }
}
