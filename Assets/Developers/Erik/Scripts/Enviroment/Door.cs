using UnityEngine;

public class Door : MonoBehaviour, IInteractable {
    [SerializeField] private GameObject requiredKey;


    public void Interact(Player player) {
        if (player.Inventory.CurrentKey == null)
            return;


        if (player.Inventory.CurrentKey.gameObject == requiredKey) {
            OpenDoor();
            player.Inventory.UseKey();
        }
    }


    private void OpenDoor() {
        gameObject.SetActive(false);
    }
}
