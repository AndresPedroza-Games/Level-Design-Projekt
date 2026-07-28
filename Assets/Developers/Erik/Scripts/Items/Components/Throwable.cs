using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Throwable : MonoBehaviour, IInteractable, IThrowable {

	[Header("---Config---")]
	[SerializeField] private LayerMask distractionLayerMask;
	[SerializeField] private float distractionRange;

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


	private void OnCollisionEnter(Collision collision) {
		TryDistractEnemy();
	}


	private void TryDistractEnemy() {
		Collider[] hits = Physics.OverlapSphere(transform.position, distractionRange);

		foreach (Collider hit in hits) {
			if (hit.TryGetComponent(out Enemy enemy)) {
				enemy.Distract(transform);
			}
		}
	}


	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, distractionRange);
	}

}