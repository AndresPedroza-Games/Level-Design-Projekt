using UnityEngine;


public class Door : MonoBehaviour, IInteractable {

	[SerializeField] private GameObject requiredKey;


	public void Interact(Player player) {
		if (!player.Inventory.CurrentHoldItem || player.Inventory.CurrentHoldItem != requiredKey) {
			OnLockedDoor();
			return;
		}

		OpenDoor();
		player.Inventory.UseItem();
	}


	private void OnLockedDoor() {
		SoundManager.Instance.PlaySound("LockedDoor");
	}


	private void OpenDoor() {
		SoundManager.Instance.PlaySound("OpenDoor");
		gameObject.SetActive(false);
	}

}