using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

    private PlayerControls controls;
    private CharacterController charController;

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
    private Coroutine capsuleRoutine = null;

    //Just for Visuals
    public GameObject body;

    [Header("---Gravity---")]
    [SerializeField] private float gravityMultiplier = 1f;
    [SerializeField][Range(-50f, -1f)] private float maxFallSpeed = -50f;




    private void Awake() {
        charController = GetComponent<CharacterController>();

        standUpCollisionMask = ~LayerMask.GetMask("Player");
        standingHeight = charController.height;
        movementSpeed = walkingSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void OnEnable() {
        controls ??= new PlayerControls();
        controls.Enable();
        controls.Movement.Crouch.performed += Crouch;
        controls.Movement.Run.performed += Run;
    }



    private void OnDisable() {
        controls.Disable();
        controls.Movement.Crouch.performed -= Crouch;
        controls.Movement.Run.performed -= Run;
    }


    private void Update() {
        moveInput = controls.Movement.Move.ReadValue<Vector2>();

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

        charController.Move((horizontal + vertical) * Time.deltaTime);
    }



    private void HandleRotation() {
        if (FaceDirection.sqrMagnitude >= 0.001f) {
            Quaternion targetRotation = Quaternion.LookRotation(FaceDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }



    private void HandleGravity() {
        if (charController.isGrounded && yVelocity <= 0) {
            yVelocity = -2.0f;
        }
        else {
            yVelocity += gravity * gravityMultiplier * Time.deltaTime;
            yVelocity = Mathf.Clamp(yVelocity, maxFallSpeed, 0f);
        }
    }



    private void Crouch(InputAction.CallbackContext ctx) {
        crouchRoutine ??= StartCoroutine(CrouchCoroutine(crouchTransitionSpeed));
        capsuleRoutine ??= StartCoroutine(CapsuleCoroutine(crouchTransitionSpeed));
        isRunning = false;
    }


    private IEnumerator CrouchCoroutine(float transitionSpeed) {
        if (!CanStandUp()) {
            crouchRoutine = null;
            yield break;
        }

        isCrouching = !isCrouching;

        float timer = 0f;
        float startHeight = charController.height;
        float startCenterHeight = charController.center.y;
        float startSpeed = movementSpeed;

        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        Vector3 targetCenterHeight = isCrouching ? new(0f, ((standingHeight - crouchHeight) / 2) * -1, 0f) : Vector3.zero;
        float targetSpeed = isCrouching ? crouchSpeed : walkingSpeed;

        while (timer < transitionSpeed) {
            timer += Time.deltaTime;
            float t = timer / transitionSpeed;

            charController.height = Mathf.Lerp(startHeight, targetHeight, t);
            charController.center = new(0f, Mathf.Lerp(startCenterHeight, targetCenterHeight.y, t), 0f);
            movementSpeed = Mathf.Lerp(startSpeed, targetSpeed, t);
            yield return null;
        }

        charController.height = targetHeight;
        charController.center = targetCenterHeight;
        movementSpeed = isCrouching ? crouchSpeed : walkingSpeed;
        crouchRoutine = null;
    }



    private IEnumerator CapsuleCoroutine(float transitionSpeed) {
        if (!CanStandUp()) {
            crouchRoutine = null;
            yield break;
        }

        float timer = 0f;
        float startScale = body.transform.localScale.y;
        float startPos = body.transform.localPosition.y;
        float relation = crouchHeight / standingHeight;

        Vector3 pos = body.transform.localPosition;
        Vector3 scale = body.transform.localScale;

        float targetScale = isCrouching ? scale.y * relation : 1f;
        float targetPos = isCrouching ? -(relation / crouchHeight) / 2 : 0f;

        while (timer < transitionSpeed) {
            timer += Time.deltaTime;
            float t = timer / transitionSpeed;

            body.transform.localScale = new(1f, Mathf.Lerp(startScale, targetScale, t), 1f);
            body.transform.localPosition = new(pos.x, Mathf.Lerp(startPos, targetPos, t), pos.z);
            yield return null;
        }

        body.transform.localScale = new(1f, targetScale, 1f);
        body.transform.localPosition = new(pos.x, targetPos, pos.z);

        capsuleRoutine = null;
    }


    private bool CanStandUp() {
        float radius = charController.radius;

        Vector3 end = transform.position + Vector3.up * (standingHeight - crouchHeight);

        return !Physics.CheckCapsule(transform.position, end, radius, standUpCollisionMask);
    }


    private void Run(InputAction.CallbackContext ctx) {
        if (isCrouching) {
            if (!CanStandUp())
                return;

            crouchRoutine ??= StartCoroutine(CrouchCoroutine(crouchTransitionSpeed));
            capsuleRoutine ??= StartCoroutine(CapsuleCoroutine(crouchTransitionSpeed));
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
        if (isCrouching)
            Gizmos.DrawWireSphere(transform.position + Vector3.up * (standingHeight - crouchHeight), charController.radius);
    }
}
