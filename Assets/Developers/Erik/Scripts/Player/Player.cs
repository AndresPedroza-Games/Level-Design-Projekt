using Unity.Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory Inventory { get; private set; }

    private GameObject _Camera;

    void Start()
    {
        Inventory = GetComponent<Inventory>();

        EventSystemController.eventSystemController.onEndGame += () => FreezCharacter(true);
        EventSystemController.eventSystemController.onRestart += () => FreezCharacter(false);

        _Camera = FindFirstObjectByType<CinemachineCamera>().gameObject;
    }

    private void FreezCharacter(bool status)
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
