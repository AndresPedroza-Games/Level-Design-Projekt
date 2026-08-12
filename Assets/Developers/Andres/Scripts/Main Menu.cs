using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _StartBtn;
    [SerializeField] private Button _ExitBtn;

    private void Awake()
    {
        _StartBtn.onClick.AddListener(OnStart);
        _ExitBtn.onClick.AddListener(OnExit);
    }

    private void OnStart()
    {
        SceneManager.LoadScene("Grayboxing-v.1");
    }

    private void OnExit()
    {
        Application.Quit();
    }
}
