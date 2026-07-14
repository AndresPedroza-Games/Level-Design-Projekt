using Unity.Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public Inventory Inventory { get; private set; }

    public GameObject _Camera;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    [field: SerializeField] public Transform _CameraTarget { get; private set; }

    void Start()
    {
        Inventory = GetComponent<Inventory>();

        EventSystemController.eventSystemController.onEndGame += () => FreezCharacter(true);
        EventSystemController.eventSystemController.onRestart += () => FreezCharacter(false);

        _Camera = FindFirstObjectByType<CinemachineCamera>().gameObject;
    }

    public void FreezCharacter(bool status)
    {
        if (status)
        {
            InputManager.Instance.Controls.Movement.Disable();
            InputManager.Instance.Controls.Interaction.Disable();
        }
        else
        {
            InputManager.Instance.Controls.Movement.Enable();
            InputManager.Instance.Controls.Interaction.Enable();
        }

        _Camera.SetActive(!status);
    }
}
