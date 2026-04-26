using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
    private CharacterController cController;

    [Header("---Cinemachine---")]
    [SerializeField] private Transform cameraTransform;


    private Vector2 moveInput;
    private Vector3 FaceDirection;
    private float yVelocity;
    private const float gravity = -9.8f;

    [Header("---Movement---")]
    [SerializeField] private float walkingSpeed = 4f;
    [SerializeField] private float crouchSpeed = 2f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float runTransitionSpeed = 0.3f;
    [SerializeField] private float turnSpeed = 10f;
    private bool isRunning = false;
    private float movementSpeed;
    private Coroutine runRoutine = null;

    [Header("---Crouching---")]
    [SerializeField] private float crouchHeight = 1.5f;
    [SerializeField] private float crouchTransitionSpeed = 0.3f;

    private int standUpCollisionMask;
    private float standingHeight;
    private bool isCrouching = false;
    private Coroutine crouchRoutine = null;


    [Header("---Gravity---")]
    [SerializeField] private float gravityMultiplier = 1f;
    [SerializeField][Range(-50f, -1f)] private float maxFallSpeed = -50f;


    private void Awake() {
        cController = GetComponent<CharacterController>();

        standUpCollisionMask = ~LayerMask.GetMask("Player");

        standingHeight = cController.height;
        movementSpeed = walkingSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void OnEnable() {
        InputManager.Instance.Controls.Movement.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        InputManager.Instance.Controls.Movement.Move.canceled += ctx => moveInput = Vector2.zero;

        InputManager.Instance.Controls.Movement.Crouch.performed += Crouch;
        InputManager.Instance.Controls.Movement.Run.performed += Run;
    }


    private void OnDisable() {
        InputManager.Instance.Controls.Disable();

        InputManager.Instance.Controls.Movement.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        InputManager.Instance.Controls.Movement.Move.canceled -= ctx => moveInput = Vector2.zero;

        InputManager.Instance.Controls.Movement.Crouch.performed -= Crouch;
        InputManager.Instance.Controls.Movement.Run.performed -= Run;
    }


    private void Update() {

        HandleGravity();
        HandleMovement();
        HandleRotation();
    }


    private void HandleMovement() {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        FaceDirection = (forward * moveInput.y + right * moveInput.x);

        Vector3 horizontal = FaceDirection * movementSpeed;
        Vector3 vertical = Vector3.up * yVelocity;

        cController.Move((horizontal + vertical) * Time.deltaTime);
    }



    private void HandleRotation() {
        if (FaceDirection.sqrMagnitude >= 0.001f) {
            Quaternion targetRotation = Quaternion.LookRotation(FaceDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }



    private void HandleGravity() {
        if (cController.isGrounded && yVelocity <= 0) {
            yVelocity = -2.0f;
        }
        else {
            yVelocity += gravity * gravityMultiplier * Time.deltaTime;
            yVelocity = Mathf.Clamp(yVelocity, maxFallSpeed, 0f);
        }
    }



    private void Crouch(InputAction.CallbackContext ctx) {
        crouchRoutine ??= StartCoroutine(CrouchCoroutine(crouchTransitionSpeed));
        isRunning = false;
    }


    private IEnumerator CrouchCoroutine(float transitionSpeed) {
        if (!CanStandUp()) {
            crouchRoutine = null;
            yield break;
        }

        isCrouching = !isCrouching;

        float timer = 0f;
        float startHeight = cController.height;
        float startCenterY = cController.center.y;
        float startSpeed = movementSpeed;

        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        Vector3 targetCenterY = isCrouching ? new(0f, crouchHeight / 2, 0f) : new(0f, standingHeight / 2, 0f);
        float targetSpeed = isCrouching ? crouchSpeed : walkingSpeed;

        while (timer < transitionSpeed) {
            timer += Time.deltaTime;
            float t = timer / transitionSpeed;

            cController.height = Mathf.Lerp(startHeight, targetHeight, t);
            cController.center = new(0f, Mathf.Lerp(startCenterY, targetCenterY.y, t), 0f);
            movementSpeed = Mathf.Lerp(startSpeed, targetSpeed, t);
            yield return null;
        }

        cController.height = targetHeight;
        cController.center = targetCenterY;
        movementSpeed = isCrouching ? crouchSpeed : walkingSpeed;
        crouchRoutine = null;
    }



    private bool CanStandUp() {
        float radius = cController.radius;

        Vector3 pos = transform.position + Vector3.up * (standingHeight - cController.radius);

        return !Physics.CheckSphere(pos, radius, standUpCollisionMask);
    }


    private void Run(InputAction.CallbackContext ctx) {
        if (isCrouching) {
            if (!CanStandUp())
                return;

            crouchRoutine ??= StartCoroutine(CrouchCoroutine(crouchTransitionSpeed));
        }

        runRoutine ??= StartCoroutine(RunCoroutine(runTransitionSpeed));
    }

    private IEnumerator RunCoroutine(float transitionSpeed) {
        isRunning = !isRunning;

        float timer = 0f;
        float startSpeed = movementSpeed;

        float targetSpeed = isRunning ? runSpeed : walkingSpeed;

        while (timer < transitionSpeed) {
            timer += Time.deltaTime;
            float t = timer / transitionSpeed;

            movementSpeed = Mathf.Lerp(startSpeed, targetSpeed, t);
            yield return null;
        }

        movementSpeed = isRunning ? runSpeed : walkingSpeed;
        runRoutine = null;
    }


    private void OnDrawGizmosSelected() {
        if (isCrouching) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * (standingHeight - cController.radius), cController.radius);
        }
    }
}
