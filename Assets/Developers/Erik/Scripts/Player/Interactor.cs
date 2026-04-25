using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour {
    
    private Player player;
    [SerializeField] private Transform interactorCenterPos;
    [SerializeField] private Vector3 halfExtends;
    [SerializeField] private LayerMask interactableMask;


    private void OnEnable() {
        InputManager.Instance.Controls.Interaction.Interact.performed += Interaction;
    }


    private void Start() {
        player = GetComponent<Player>();
    }

    private void OnDisable() {
        InputManager.Instance.Controls.Interaction.Interact.performed -= Interaction;
    }



    private void Interaction(InputAction.CallbackContext ctx) {
        Collider[] hits = Physics.OverlapBox(interactorCenterPos.position, halfExtends, Quaternion.identity, interactableMask);

        foreach (Collider col in hits) {
            if (col.gameObject.TryGetComponent(out IInteractable interactable)) {
                interactable.Interact(player);
            }
        }
    }




    private void OnDrawGizmosSelected() {
        if(interactorCenterPos == null ||  halfExtends == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(interactorCenterPos.position, halfExtends * 2);
    }
}
