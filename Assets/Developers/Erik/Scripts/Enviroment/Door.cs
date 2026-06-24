using UnityEngine;

public class Door : MonoBehaviour, IInteractable {
    [SerializeField] private GameObject requiredKey;
    [SerializeField] private bool _DontNeedKey;

    public void Interact(Player player) {
        if (player.Inventory.CurrentKey == null && !_DontNeedKey)
        {
            UIController.uiController.ChangeTextBox("Closed Door");
            return;
        }

        if (_DontNeedKey)
        {
            OpenDoor();
            return;
        }

        if (player.Inventory.CurrentKey.gameObject == requiredKey) {
            OpenDoor();
            player.Inventory.UseKey();
        }
    }


    public void OpenDoor() {
        gameObject.SetActive(false);
        EventSystemController.eventSystemController.OpenDoor(this);
    }
}
