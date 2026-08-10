using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerLockController : MonoBehaviour
{
    [SerializeField] private float _RayDistance = 2f;
    [SerializeField] private LayerMask _DetectorLayerMask;
    [SerializeField] private GameObject _Crosshair;

    private EventSystemController _EventSystemController;
    private GameObject _Camera;
    private GameObject _CameraLock;
    private Player _Player;

    private Vector2 direction;

    private void Start()
    {
        _EventSystemController = EventSystemController.eventSystemController;
        _EventSystemController.onInteractWithLock += InteractLock;
        _EventSystemController.onPuzzleCompleted += ExitLockOnComplete;

        _Player = Player.Instance;

        _Camera = _Player._Camera.gameObject;
    }

    private void OnEnable()
    {
        InputManager.Instance.ExitLock.performed += ExitLock;

        InputManager.Instance.RotatePiece.performed += RotateLock;
        InputManager.Instance.RotatePiece.canceled += RotateLock;

        InputManager.Instance.ReleasePiece.performed += RelasePiece;
        InputManager.Instance.SelectPiece.performed += SelectPiece;
    }

    private void OnDisable()
    {
        InputManager.Instance.ExitLock.performed -= ExitLock;

        InputManager.Instance.RotatePiece.performed -= RotateLock;
        InputManager.Instance.RotatePiece.canceled -= RotateLock;

        InputManager.Instance.ReleasePiece.performed -= RelasePiece;
        InputManager.Instance.SelectPiece.performed -= SelectPiece;

        _EventSystemController.onPuzzleCompleted -= ExitLockOnComplete;
    }

    private void InteractLock()
    {
        _Camera.SetActive(false);
        _Player.FreezeCharacter(true);
    }

    private void ExitLock(InputAction.CallbackContext ctx)
    {
        _EventSystemController.ExitLock();
        _Camera.SetActive(true);
        _Player.FreezeCharacter(false);
    }


    private void ExitLockOnComplete() {
	    _EventSystemController.ExitLock();
	    _Camera.SetActive(true);
	    _Player.FreezeCharacter(false);
    }

    private void RotateLock(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<Vector2>();
        _EventSystemController.RotateLock(direction);
    }

    private void RelasePiece(InputAction.CallbackContext ctx)
    {
        _EventSystemController.ReleasePiece();
    }

    private void SelectPiece(InputAction.CallbackContext ctx)
    {
        if (!GameManager.lockIsActive)
            return;

        _CameraLock = LockInteractor.Instance._Camera;

        if (Physics.Raycast(_CameraLock.transform.position, _CameraLock.transform.forward, out RaycastHit hit, _RayDistance, _DetectorLayerMask)) {
	        if (hit.collider.TryGetComponent(out IInteractable interactable))
		        interactable.Interact(_Player);
        }
        
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if(_CameraLock != null)
            Gizmos.DrawLine(_CameraLock.transform.position, _CameraLock.transform.forward * _RayDistance);

    }
}
