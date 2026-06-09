using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Key : MonoBehaviour, IInteractable {

	public KeyDataSO keyData;

	public bool CanPickUp { get; private set; }


	public void Interact(Player player) {
		if (!CanPickUp)
			return;

		CanPickUp = false;
		player.Inventory.AddItem(gameObject);
	}


	private void OnCollisionEnter(Collision collision) {
		if (!CanPickUp)
			CanPickUp = true;
	}

}