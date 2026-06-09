using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public PlayerControls Controls { get; private set; }

    public InputAction Move => Controls.Movement.Move;
    public InputAction Run => Controls.Movement.Run;
    public InputAction Crouch => Controls.Movement.Crouch;
    public InputAction Interact => Controls.Interaction.Interact;

    public InputAction Zoom => Controls.Interaction.Zoom;
    public InputAction Throw => Controls.Interaction.Throw;


    public InputAction Emote1 => Controls.Emotes.Emote1;
    public InputAction Emote2 => Controls.Emotes.Emote2;
    public InputAction Emote3 => Controls.Emotes.Emote3;


    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        else {
            Instance = this;
        }

        Controls ??= new PlayerControls();
    }

    private void OnEnable() {
        Controls.Enable();
    }
}
