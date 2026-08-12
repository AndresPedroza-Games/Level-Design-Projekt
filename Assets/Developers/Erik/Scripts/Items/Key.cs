using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Key : MonoBehaviour, IInteractable {

	private Rigidbody rb;
	private Collider col;
    private KeyPickupSound pickupSound;

    public KeyDataSO keyData;

    public bool isGrabbed;

    public bool CanPickUp { get; private set; }

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        pickupSound = GetComponent<KeyPickupSound>();
    }

 
	public void Interact(Player player) {
		if (!CanPickUp)
			return;
		
		DisablePhysics();
		CanPickUp = false;

        pickupSound?.PlayPickupSound();

        player.Inventory.AddItem(gameObject);
	}

    private void DisablePhysics() {
        CanPickUp = false;

        col.enabled = false;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.freezeRotation = true;
    }
    
    public void EnablePhysics() {
	    CanPickUp = true;

	    col.enabled = true;
	    rb.useGravity = true;
	    rb.linearVelocity = Vector3.zero;
	    rb.freezeRotation = false;
    }




	private void OnCollisionEnter(Collision collision) {
		if (!CanPickUp)
			CanPickUp = true;
	}

}