using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerThrow : MonoBehaviour {

	[Header("---Throw Config---")]
	[SerializeField] private float throwForce = 10f;

	private Player player;
	private Camera cam;


	private void Awake() {
		player = GetComponent<Player>();
		cam = Camera.main;
	}


	private void OnEnable() {
		InputManager.Instance.Throw.performed += Throw;
	}


	private void OnDisable() {
		InputManager.Instance.Throw.performed -= Throw;
	}


	private void Throw(InputAction.CallbackContext ctx) {
		if (!player.Inventory.CurrentHoldItem)
			return;

		if (player.Inventory.CurrentHoldItem.TryGetComponent(out IThrowable throwable)) {
			player.Inventory.DropItem();
			throwable.Throw(cam.transform.forward, throwForce);
		}
	}


	private void OnDrawGizmosSelected() {
		if (!cam) {
			cam = Camera.main;
		}

		if (!cam)
			return;

		Debug.DrawRay(cam.transform.position, cam.transform.forward * 3f, Color.blue);
	}

}