using System;
using UnityEngine;


public class Door : MonoBehaviour, IInteractable {

	[SerializeField] private GameObject requiredKey;


	public void Interact(Player player) {
		if (player.Inventory.CurrentKey != null) {
			if (player.Inventory.CurrentKey.gameObject == requiredKey) {
				OpenDoor();
				player.Inventory.UseKey();
				return;
			}
		}

		OnLockedDoor();

	}


	private void OnLockedDoor() {
		SoundManager.Instance.PlaySound("LockedDoor");
	}


	private void OpenDoor() {
		SoundManager.Instance.PlaySound("OpenDoor");
		gameObject.SetActive(false);
	}

}