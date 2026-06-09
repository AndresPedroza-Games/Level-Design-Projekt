using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Throwable : MonoBehaviour, IInteractable ,IThrowable {

	private Rigidbody rb;


	private void Awake() {
		rb = GetComponent<Rigidbody>();
		gameObject.layer = LayerMask.NameToLayer("Interactable");
	}


	public void Interact(Player player) {
		player.Inventory.AddItem(gameObject);
	}


	public void Throw(Vector3 dir, float force) {
	    rb.AddForce(dir * force, ForceMode.Impulse);
    }

}
