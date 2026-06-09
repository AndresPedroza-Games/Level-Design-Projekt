using UnityEngine;
using UnityEngine.InputSystem;


public class Inventory : MonoBehaviour {

	public GameObject CurrentHoldItem { get; private set; }

	public Transform itemSocket;
	public Transform itemDropPos;


	private void OnEnable() {
		InputManager.Instance.Controls.Interaction.Drop.performed += DropItem;

	}


	private void OnDisable() {
		InputManager.Instance.Controls.Interaction.Drop.performed -= DropItem;
	}


	public void AddItem(GameObject obj) {
		if (CurrentHoldItem) {
			SwitchItems(obj);
		}
		else {
			CurrentHoldItem = obj;
			PickUpItem(obj);
		}
	}


	public void UseItem() {
		if (CurrentHoldItem != null) {
			if (CurrentHoldItem.TryGetComponent(out Key key)) {
				CurrentHoldItem.SetActive(false);
				CurrentHoldItem = null;
			}
		}
	}


	private void SwitchItems(GameObject obj) {
		DropItem();

		CurrentHoldItem = obj;
		PickUpItem(obj);
	}


	private void PickUpItem(GameObject obj) {
		if (!obj.TryGetComponent(out Rigidbody rb))
			return;

		if (!obj.TryGetComponent(out Collider col))
			return;

		col.enabled = false;
		rb.useGravity = false;
		rb.linearVelocity = Vector3.zero;
		rb.freezeRotation = true;

		obj.transform.SetParent(itemSocket);
		obj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
	}


	public void DropItem() {
		if (!CurrentHoldItem)
			return;

		if (!CurrentHoldItem.TryGetComponent(out Rigidbody rb))
			return;

		if (!CurrentHoldItem.TryGetComponent(out Collider col))
			return;


		col.enabled = true;
		rb.useGravity = true;
		rb.freezeRotation = false;

		CurrentHoldItem.transform.parent = null;
		CurrentHoldItem.transform.position = itemDropPos.position;
		
		CurrentHoldItem = null;
	}


	private void DropItem(InputAction.CallbackContext ctx) {
		if (CurrentHoldItem)
			DropItem();
	}


	private void OnDrawGizmosSelected() {
		if (itemDropPos != null) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(itemDropPos.position, 0.2f);
		}
	}

}