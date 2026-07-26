using UnityEngine;
using UnityEngine.InputSystem;


public class Flashlight : MonoBehaviour {

	private Light _light;
	private bool isOn;


	private void Awake() {
		_light = GetComponent<Light>();
	}


	private void OnEnable() {
		InputManager.Instance.Flashlight.performed += ToggleFlashLight;
	}


	private void OnDisable() {
		InputManager.Instance.Flashlight.performed -= ToggleFlashLight;
	}


	private void ToggleFlashLight(InputAction.CallbackContext ctx) {
		isOn = !isOn;

		_light.enabled = isOn;
	}


}