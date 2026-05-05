using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationController : MonoBehaviour {
    public float dampTime = 0.1f;

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



    private void HandleCrouchAnimation(bool isCrouching) {
        if (isCrouching)
            InputManager.Instance.Controls.Emotes.Disable();
        else
            InputManager.Instance.Controls.Emotes.Enable();

        animator.SetBool("isCrouching", isCrouching);
    }


    private void ResetAllEmotes() {
        animator.SetBool("Emote1", false);
        animator.SetBool("Emote2", false);
        animator.SetBool("Emote3", false);
    }

    private void PlayEmote(string emote) {
        ResetAllEmotes();
        emoting = !emoting;
        animator.SetBool(emote, emoting);
    }

    private void OnEmote1Performed(InputAction.CallbackContext ctx) {
        PlayEmote("Emote1");
    }

    private void OnEmote2Performed(InputAction.CallbackContext ctx) {
        PlayEmote("Emote2");
    }

    private void OnEmote3Performed(InputAction.CallbackContext ctx) {
        PlayEmote("Emote3");
    }

}
