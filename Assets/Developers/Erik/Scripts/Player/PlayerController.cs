using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {


	private CharacterController cController;
	private Animator animator;
	private readonly int speed = Animator.StringToHash("Speed");

	public event Action<bool> OnCrouchChanged;

	[Header("---CineMachine---")]
	[SerializeField] private Transform cameraTransform;


	[Header("---Movement---")]
	[SerializeField] private float walkingSpeed = 2.5f;
	[SerializeField] private float crouchSpeed = 1.5f;
	[SerializeField] private float runSpeed = 4f;
	[SerializeField] private float runTransitionSpeed = 0.3f;
	[SerializeField] private float turnSpeed = 10f;

	[Header("---Push Object---")]
	[SerializeField] private float force = 1f;

	[Header("--Animation---")]
	[SerializeField] private float animationDampTime = 0.1f;

	private Vector2 moveInput;
	private Vector3 faceDirection;
	private float yVelocity;
	private const float Gravity = -9.8f;
	private bool isRunning;
	private float movementSpeed;

	private Coroutine runRoutine;


	[Header("---Crouching---")]
	[SerializeField] private float crouchHeight = 1.5f;
	[SerializeField] private float crouchTransitionSpeed = 0.3f;

	private int standUpCollisionMask;
	private float standingHeight;
	private bool isCrouching;
	private Coroutine crouchRoutine;


	[Header("---Gravity---")]
	[SerializeField] private float gravityMultiplier = 1f;
	[SerializeField] [Range(-50f, -1f)] private float maxFallSpeed = -50f;


	private void Awake() {
		cController = GetComponent<CharacterController>();
		animator = GetComponentInChildren<Animator>();

		standUpCollisionMask = ~LayerMask.GetMask("Player");

		standingHeight = cController.height;
		movementSpeed = walkingSpeed;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}


	private void OnEnable() {
		InputManager.Instance.Move.performed += OnMoveInputPerformed;
		InputManager.Instance.Move.canceled += OnMoveInputCanceled;

		InputManager.Instance.Crouch.performed += Crouch;

		InputManager.Instance.Run.performed += Run;
	}


	private void OnDisable() {
		InputManager.Instance.Controls.Disable();

		InputManager.Instance.Move.performed -= OnMoveInputPerformed;
		InputManager.Instance.Move.canceled -= OnMoveInputCanceled;

		InputManager.Instance.Crouch.performed -= Crouch;

		InputManager.Instance.Run.performed -= Run;
	}


	private void OnMoveInputPerformed(InputAction.CallbackContext ctx) {
		moveInput = ctx.ReadValue<Vector2>();
	}


	private void OnMoveInputCanceled(InputAction.CallbackContext ctx) {
		moveInput = Vector2.zero;
	}


	private void Update() {

		HandleGravity();
		HandleMovement();
		HandleRotation();

		HandleMovementAnimation();
	}


	private void HandleMovementAnimation() {
		float moveSpeed = isRunning ? 1f : 0.5f;
		animator.SetFloat(speed, moveSpeed * moveInput.magnitude, animationDampTime, Time.deltaTime);
	}


	private void HandleMovement() {
		Vector3 forward = cameraTransform.forward;
		Vector3 right = cameraTransform.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize();
		right.Normalize();

		faceDirection = (forward * moveInput.y + right * moveInput.x).normalized;

		Vector3 horizontal = faceDirection * movementSpeed;
		Vector3 vertical = Vector3.up * yVelocity;

		cController.Move((horizontal + vertical) * Time.deltaTime);
	}


	private void HandleRotation() {
		if (faceDirection.sqrMagnitude >= 0.001f) {
			Quaternion targetRotation = Quaternion.LookRotation(faceDirection);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
		}
	}


	private void HandleGravity() {
		if (cController.isGrounded && yVelocity <= 0) {
			yVelocity = -2.0f;
		}
		else {
			yVelocity += Gravity * gravityMultiplier * Time.deltaTime;
			yVelocity = Mathf.Clamp(yVelocity, maxFallSpeed, 0f);
		}
	}


	private void Crouch(InputAction.CallbackContext ctx) {
		crouchRoutine ??= StartCoroutine(CrouchCoroutine(crouchTransitionSpeed));
		isRunning = false;
	}


	private IEnumerator CrouchCoroutine(float transitionSpeed) {
		if (!CanStandUp()) {
			crouchRoutine = null;
			yield break;
		}

		isCrouching = !isCrouching;
		OnCrouchChanged?.Invoke(isCrouching);

		float timer = 0f;
		float startHeight = cController.height;
		float startCenterY = cController.center.y;
		float startSpeed = movementSpeed;

		float targetHeight = isCrouching ? crouchHeight : standingHeight;
		Vector3 targetCenterY = isCrouching ? new(0f, crouchHeight / 2, 0f) : new(0f, standingHeight / 2, 0f);
		float targetSpeed = isCrouching ? crouchSpeed : walkingSpeed;

		while (timer < transitionSpeed) {
			timer += Time.deltaTime;
			float t = timer / transitionSpeed;

			cController.height = Mathf.Lerp(startHeight, targetHeight, t);
			cController.center = new(0f, Mathf.Lerp(startCenterY, targetCenterY.y, t), 0f);
			movementSpeed = Mathf.Lerp(startSpeed, targetSpeed, t);
			yield return null;
		}

		cController.height = targetHeight;
		cController.center = targetCenterY;
		movementSpeed = isCrouching ? crouchSpeed : walkingSpeed;
		crouchRoutine = null;
	}


	private bool CanStandUp() {
		float radius = cController.radius;

		Vector3 pos = transform.position + Vector3.up * (standingHeight - cController.radius);

		return !Physics.CheckSphere(pos, radius, standUpCollisionMask);
	}


	private void Run(InputAction.CallbackContext ctx) {
		if (isCrouching) {
			if (!CanStandUp())
				return;

			crouchRoutine ??= StartCoroutine(CrouchCoroutine(crouchTransitionSpeed));
		}

		runRoutine ??= StartCoroutine(RunCoroutine(runTransitionSpeed));
	}


	private IEnumerator RunCoroutine(float transitionSpeed) {
		isRunning = !isRunning;

		float timer = 0f;
		float startSpeed = movementSpeed;

		float targetSpeed = isRunning ? runSpeed : walkingSpeed;

		while (timer < transitionSpeed) {
			timer += Time.deltaTime;
			float t = timer / transitionSpeed;

			movementSpeed = Mathf.Lerp(startSpeed, targetSpeed, t);
			yield return null;
		}

		movementSpeed = isRunning ? runSpeed : walkingSpeed;
		runRoutine = null;
	}


	private void OnControllerColliderHit(ControllerColliderHit hit) {
		Rigidbody rb = hit.collider.attachedRigidbody;

		if (!rb) return;
		
		Vector3 dir = faceDirection;
		dir.y = 0f;
		dir.Normalize();
		rb.AddForceAtPosition(dir * force, transform.position, ForceMode.Impulse);
	}


	private void OnDrawGizmosSelected() {
		if (isCrouching) {
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position + Vector3.up * (standingHeight - cController.radius), cController.radius);
		}
	}

}