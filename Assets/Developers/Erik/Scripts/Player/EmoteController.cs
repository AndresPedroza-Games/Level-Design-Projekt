using UnityEngine;
using UnityEngine.InputSystem;

public class EmoteController : MonoBehaviour
{
    private Animator animator;
    private bool performing;


    private void Start() {
        animator = GetComponent<Animator>();
    }


    private void OnEnable() {
        InputManager.Instance.Controls.Emotes.Emote1.performed += OnEmote1Performed;
    }

    private void OnDisable() {
        InputManager.Instance.Controls.Emotes.Emote1.performed -= OnEmote1Performed;
    }


    private void OnEmote1Performed(InputAction.CallbackContext ctx) {
        performing = !performing;
        animator.SetBool("Emote1", performing);
    }
}
