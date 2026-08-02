using UnityEngine;
using UnityEngine.InputSystem;


public class ThirdPersonAim : MonoBehaviour {

	[Header("---CameraTarget---")]
	[SerializeField] private Transform cameraTarget;

	[Header("---Sensitivity---")]
	[SerializeField] private float mouseSensitivity = 150f;

	[Header("---Pitch---")]
	[SerializeField] [Range(-89.9f, 0f)] private float minPitch = -50f;
	[SerializeField] [Range(0f, 89.9f)] private float maxPitch = 75f;

	private Vector2 lookInput;

	private float yaw;
	private float pitch;


	private void Awake() {
		Vector3 angles = cameraTarget.rotation.eulerAngles;

		yaw = angles.y;
		pitch = angles.x;
	}


	private void OnEnable() {
		InputManager.Instance.Look.performed += OnLook;
		InputManager.Instance.Look.canceled += OnLook;
	}


	private void OnDisable() {
		InputManager.Instance.Look.performed -= OnLook;
		InputManager.Instance.Look.canceled -= OnLook;
	}


	private void OnLook(InputAction.CallbackContext ctx) {
		lookInput = ctx.ReadValue<Vector2>();
	}


	private void LateUpdate() {
		RotateCamera();
	}


	private void RotateCamera() {

		yaw += lookInput.x * mouseSensitivity * Time.deltaTime;
		pitch -= lookInput.y * mouseSensitivity * Time.deltaTime;

		pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

		cameraTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);
	}

}