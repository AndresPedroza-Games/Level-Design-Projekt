using UnityEngine;
using UnityEngine.InputSystem;


public class Inventory : MonoBehaviour {

	public GameObject CurrentHoldItem { get; private set; }

	public Transform keySocket;
	public Transform itemDropPos;


	private void OnEnable() {
		InputManager.Instance.Controls.Interaction.Drop.performed += DropKeyAction;

	}


	private void OnDisable() {
		InputManager.Instance.Controls.Interaction.Drop.performed -= DropKeyAction;
	}


	public void AddItem(GameObject obj) {
		if (CurrentHoldItem != null) {
			SwitchItems(obj);
		}
		else {
			CurrentHoldItem = obj;
			PickUpItem(obj.gameObject);
		}
	}


	public void UseKey() {
		if (CurrentHoldItem != null) {
			CurrentHoldItem.gameObject.SetActive(false);
			CurrentHoldItem = null;
		}
	}


	private void SwitchItems(GameObject obj) {
		DropItem();

		CurrentHoldItem = obj;
		PickUpItem(obj);
	}


	private void PickUpItem(GameObject item) {
		item.transform.SetParent(keySocket);
		item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
		
		if (item.TryGetComponent(out Rigidbody rb))
			rb.isKinematic = true;

		if (item.TryGetComponent(out Key key)) {
			item.GetComponent<Key>().isGrabbed = true;
		}
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
		rb.isKinematic = false;

		CurrentHoldItem.transform.parent = null;
		CurrentHoldItem.transform.position = itemDropPos.position;

		CurrentHoldItem = null;
	}


	private void DropKeyAction(InputAction.CallbackContext ctx) {
		if (CurrentHoldItem != null)
			DropItem();
	}


	private void OnDrawGizmosSelected() {
		if (itemDropPos != null) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(itemDropPos.position, 0.2f);
		}
	}

}