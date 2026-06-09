using UnityEngine;
using UnityEngine.InputSystem;


public class AnimationController : MonoBehaviour {

	private readonly int emote1 = Animator.StringToHash("Emote1");
	private readonly int emote2 = Animator.StringToHash("Emote2");
	private readonly int emote3 = Animator.StringToHash("Emote3");

	private readonly int animSpeedParam = Animator.StringToHash("Speed");
	private readonly int inputX = Animator.StringToHash("InputX");
	private readonly int inputY = Animator.StringToHash("InputY");
	private readonly int isCrouching = Animator.StringToHash("isCrouching");


	[SerializeField] private float animationDampTime = 0.1f;

	private PlayerController playerController;
	private Animator animator;

	private bool emoting;


	private void Awake() {
		playerController = GetComponentInParent<PlayerController>();
		animator = GetComponent<Animator>();
	}


	private void OnEnable() {
		playerController.OnCrouchChanged += HandleCrouchAnimation;

		InputManager.Instance.Emote1.performed += OnEmote1Performed;
		InputManager.Instance.Emote2.performed += OnEmote2Performed;
		InputManager.Instance.Emote3.performed += OnEmote3Performed;

	}


	private void OnDisable() {
		playerController.OnCrouchChanged -= HandleCrouchAnimation;

		InputManager.Instance.Emote1.performed -= OnEmote1Performed;
		InputManager.Instance.Emote2.performed -= OnEmote2Performed;
		InputManager.Instance.Emote3.performed -= OnEmote3Performed;
	}


	private void LateUpdate() {
		HandleMovementAnimation();
	}


	private void HandleMovementAnimation() {
		float speedMultiplier = playerController.IsRunning ? 1f : 0.5f;
		animator.SetFloat(animSpeedParam, speedMultiplier * playerController.MoveInput.magnitude, animationDampTime, Time.deltaTime);
		animator.SetFloat(inputX, playerController.MoveInput.x, animationDampTime, Time.deltaTime);
		animator.SetFloat(inputY, playerController.MoveInput.y, animationDampTime, Time.deltaTime);
	}


	private void HandleCrouchAnimation(bool crouching) {
		if (crouching)
			InputManager.Instance.Controls.Emotes.Disable();
		else
			InputManager.Instance.Controls.Emotes.Enable();

		animator.SetBool(isCrouching, crouching);
	}


	private void ResetAllEmotes() {
		animator.SetBool(emote1, false);
		animator.SetBool(emote2, false);
		animator.SetBool(emote3, false);
	}


	private void PlayEmote(int emote) {
		ResetAllEmotes();
		emoting = !emoting;
		animator.SetBool(emote, emoting);
	}


	private void OnEmote1Performed(InputAction.CallbackContext ctx) {
		PlayEmote(emote1);
	}


	private void OnEmote2Performed(InputAction.CallbackContext ctx) {
		PlayEmote(emote2);
	}


	private void OnEmote3Performed(InputAction.CallbackContext ctx) {
		PlayEmote(emote3);
	}

}