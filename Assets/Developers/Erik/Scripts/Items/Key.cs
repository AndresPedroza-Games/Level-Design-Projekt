using UnityEngine;

public class Key : MonoBehaviour, IInteractable {
    public KeyDataSO keyData;

    private Rigidbody rb;
    private Collider col;


    private void Awake() {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void Interact(Player player) {
        player.Inventory.AddKey(this);
        DisablePhysics();
    }

    private void DisablePhysics() {
        col.enabled = false;
        rb.useGravity = false;
    }


    public void EnablePhysics() {
        col.enabled = true;
        rb.useGravity = true;
    }
}
