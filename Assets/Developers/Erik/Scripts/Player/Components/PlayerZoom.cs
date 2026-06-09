using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerZoom : MonoBehaviour {

	[Header("---Zoom Config---")]
	[SerializeField] private float fovWhenZooming = 30f;
	[SerializeField] private float zoomDuration = 0.2f;

	[SerializeField] private CinemachineCamera cineCam;

	private float defaultFOV;
	private Coroutine zoomRoutine;


	private void Awake() {
		defaultFOV = cineCam.Lens.FieldOfView;
	}


	private void OnEnable() {
		InputManager.Instance.Zoom.performed += OnZoomPerformed;
		InputManager.Instance.Zoom.canceled += OnZoomCanceled;
	}


	private void OnDisable() {
		InputManager.Instance.Zoom.started -= OnZoomPerformed;
		InputManager.Instance.Zoom.canceled -= OnZoomCanceled;
	}


	private void OnZoomPerformed(InputAction.CallbackContext obj) {
		if (zoomRoutine != null) {
			StopCoroutine(zoomRoutine);
			zoomRoutine = null;
		}

		zoomRoutine = StartCoroutine(ZoomCoroutine(fovWhenZooming));
	}


	private void OnZoomCanceled(InputAction.CallbackContext obj) {
		if (zoomRoutine != null) {
			StopCoroutine(zoomRoutine);
			zoomRoutine = null;
		}

		zoomRoutine = StartCoroutine(ZoomCoroutine(defaultFOV));
	}


	private IEnumerator ZoomCoroutine(float targetFOV) {

		float startFOV = cineCam.Lens.FieldOfView;
		float time = 0f;

		while (time < zoomDuration) {
			time += Time.deltaTime;

			cineCam.Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, time / zoomDuration);
			yield return null;
		}

		cineCam.Lens.FieldOfView = targetFOV;
		zoomRoutine = null;
	}

}