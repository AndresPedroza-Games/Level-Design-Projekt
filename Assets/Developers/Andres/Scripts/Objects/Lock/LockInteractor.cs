using UnityEngine;

public class LockInteractor : MonoBehaviour, IInteractable
{
    public GameObject _Camera;

    private EventSystemController _EventSystemController;

    public static LockInteractor Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _EventSystemController = EventSystemController.eventSystemController;
        _EventSystemController.onEndInteractionWithLock += ExitInteraction;

    }

    public void Interact(Player player)
    {
        _EventSystemController.InteractWithLock();
        _Camera.SetActive(true);
        gameObject.SetActive(false);
        GameManager.lockIsActive = true;
    }

    public void ExitInteraction()
    {
        _Camera.SetActive(false);
        gameObject.SetActive(true);
        GameManager.lockIsActive = false;
    }
}
