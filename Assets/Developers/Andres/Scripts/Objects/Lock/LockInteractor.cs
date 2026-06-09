using Unity.Cinemachine;
using UnityEngine;

public class LockInteractor : MonoBehaviour, IInteractable
{
    public static GameObject _Camera;

    private EventSystemController _EventSystemController;

    private void Start()
    {
        _EventSystemController = EventSystemController.eventSystemController;
        _EventSystemController.onEndInteractionWithLock += ExitInteraction;

        _Camera = FindFirstObjectByType<CinemachineCamera>(FindObjectsInactive.Include).gameObject;
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
