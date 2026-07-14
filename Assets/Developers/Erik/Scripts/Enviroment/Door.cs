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
    }

	private void OnLockedDoor() {
		SoundManager.Instance.PlaySound("LockedDoor");
	}


    public void OpenDoor() {
        gameObject.SetActive(false);
        SoundManager.Instance.PlaySound("OpenDoor");
        EventSystemController.eventSystemController.OpenDoor(this);
    }
}
