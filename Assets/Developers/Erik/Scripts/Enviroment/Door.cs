using System.Collections.Generic;
using UnityEngine;


public class Door : MonoBehaviour, IInteractable {

	[Header("---Key Config---")]
	[SerializeField] private GameObject requiredKey;
	public bool _DontNeedKey;

	[Header("---Double Door Config---")]
	[SerializeField] private bool isDoubleDoor;
	[SerializeField] private List<GameObject> doors;


	public void Interact(Player player) {

		if (_DontNeedKey || player.Inventory.CurrentHoldItem == requiredKey && requiredKey) {
			if (requiredKey)
				player.Inventory.UseKey();
			
			OpenDoor();
			return;
		}

		UIController.uiController.ChangeTextBox("Closed Door");
		SoundManager.Instance.PlaySound("LockedDoor");
	}


	public void OpenDoor() {
		if (isDoubleDoor && doors.Count > 0) {
			foreach (GameObject door in doors) {
				door.SetActive(false);
			}
		}

		gameObject.SetActive(false);
		SoundManager.Instance.PlaySound("OpenDoor");
		EventSystemController.eventSystemController.OpenDoor(this);
	}

}