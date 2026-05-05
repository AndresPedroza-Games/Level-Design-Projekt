using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationController : MonoBehaviour
{
    public float dampTime = 0.1f;

    private PlayerController playerController;
    private Animator animator;

    private bool emoting;

    private const string crouch = "isCrouching";



    private void Awake() {
        playerController = GetComponentInParent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        playerController.OnCrouchChanged += HandleCrouchAnimation;
        InputManager.Instance.Emote1.performed += OnEmote1Performed;
    }


    private void OnDisable() {
        playerController.OnCrouchChanged -= HandleCrouchAnimation;
        InputManager.Instance.Emote1.performed -= OnEmote1Performed;
    }



    private void HandleCrouchAnimation(bool isCrouching) {
        animator.SetBool(crouch, isCrouching);
    }



    private void OnEmote1Performed(InputAction.CallbackContext ctx) {
        if (playerController.IsCrouching) return;

        emoting = !emoting;
        animator.SetBool("Emote1", emoting);
    }
}
