using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Door : MonoBehaviour, IInteractable {

	[Header("---Key Config---")]
	[SerializeField] private GameObject requiredKey;
	public bool _DontNeedKey;

	[Header("---Double Door Config---")]
	[SerializeField] private bool isDoubleDoor;
	[SerializeField] private List<GameObject> doors;

	[Header("---On Open Action---")]
	public UnityEvent onOpenAction;


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
		
		onOpenAction?.Invoke();
		SoundManager.Instance.PlaySound("OpenDoor");
		EventSystemController.eventSystemController.OpenDoor(this);
		
		gameObject.SetActive(false);
	}

}