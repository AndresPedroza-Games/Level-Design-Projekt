using UnityEngine;


public class Door : MonoBehaviour, IInteractable {
    [SerializeField] private GameObject requiredKey;
    [SerializeField] private bool _DontNeedKey;

    public void Interact(Player player) {

        if (_DontNeedKey || player.Inventory.CurrentHoldItem == requiredKey)
        {
	        player.Inventory.UseKey();
            OpenDoor();
            return;
        }
        
	    UIController.uiController.ChangeTextBox("Closed Door");
	    SoundManager.Instance.PlaySound("LockedDoor");
    }


    public void OpenDoor() {
        gameObject.SetActive(false);
        SoundManager.Instance.PlaySound("OpenDoor");
        EventSystemController.eventSystemController.OpenDoor(this);
    }
}
