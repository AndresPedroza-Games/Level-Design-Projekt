using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public PlayerControls Controls { get; private set; }


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
