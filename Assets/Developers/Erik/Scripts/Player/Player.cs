using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _EndGameHUD;

    public Inventory Inventory { get; private set; }

    private EventSystemController _EventSystemController;

    void Start()
    {
        Inventory = GetComponent<Inventory>();
        _EventSystemController = EventSystemController.eventSystemController;
        _EventSystemController.onEndGame += EndGame;
    }

    private void EndGame()
    {
        _EndGameHUD.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("End Game");
    }
}
