using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Key : MonoBehaviour, IInteractable {

	public KeyDataSO keyData;

    public bool isGrabbed;

    public bool CanPickUp { get; private set; }

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void Interact(Player player) {
        if (!CanPickUp)
            return;

        DisablePhysics();
        player.Inventory.AddKey(this);
    }

    private void DisablePhysics() {
        CanPickUp = false;

        col.enabled = false;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.freezeRotation = true;
    }


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