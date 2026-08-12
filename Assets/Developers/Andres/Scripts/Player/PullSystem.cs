using UnityEngine;

public class PullSystem : MonoBehaviour
{
    [Header("Interaction Settings")]

    [SerializeField] private Transform _InteractorCenterPos;
    [SerializeField] private Vector3 _HalfExtends;
    [SerializeField] private LayerMask _InteractableMask;

    [SerializeField][Range(0.1f, 0.5f)] private float _Offset = 0.1f;

    [Header("---Follow Speed---")]
    [SerializeField] private float _SmoothSpeed = 15f;

    [Header(("---Joint Break Config---"))]
    [SerializeField] private float _BreakDistance = 2f;
    [SerializeField] private float _JointLimit = 0.5f;

    [Header("---Joint Driver Config---")]
    [SerializeField] private float _PositionSpring = 800f;
    [SerializeField] private float _PositionDamper = 40f;
    [SerializeField] private float _MaxForce = 1000f;

    private ConfigurableJoint _Joint;

    private Rigidbody _CurrentHoldGameObject;
    private RigidbodyConstraints _OriginalConstraints;
    private bool _IsKinematic;

    private Quaternion _RotationOffset;

    private GameObject _HoldBody;
    private Rigidbody _HoldRb;
    private HoldProfiler _HoldProfiler;

    private void Awake()
    {
        _Joint = GetComponent<ConfigurableJoint>();
    }

    private void OnEnable()
    {
        InputManager.Instance.Pull.performed += Pull;
        InputManager.Instance.Pull.canceled += Release;
    }

    private void OnDisable()
    {
        InputManager.Instance.Pull.performed -= Pull;
        InputManager.Instance.Pull.canceled -= Release;
    }

    private void Start()
    {
        CreateHoldBody();

        _HoldProfiler = new HoldProfiler()
        {
            useGravity = true,
            constraints = RigidbodyConstraints.FreezeRotation,
            xMotion = ConfigurableJointMotion.Free,
            yMotion = ConfigurableJointMotion.Free,
            zMotion = ConfigurableJointMotion.Free,
            angularXMotion = ConfigurableJointMotion.Locked,
            angularYMotion = ConfigurableJointMotion.Locked,
            angularZMotion = ConfigurableJointMotion.Locked
        };

    }

    private void FixedUpdate()
    {
        CheckJointState();
        MoveHoldPosition();
        UpdateRotation();
    }

    private void Pull(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (_CurrentHoldGameObject != null || ChechColiision() == null)
            return;

        _CurrentHoldGameObject = ChechColiision();

        _OriginalConstraints = _CurrentHoldGameObject.constraints;
        _IsKinematic = _CurrentHoldGameObject.isKinematic;

        _CurrentHoldGameObject.isKinematic = false;
        _CurrentHoldGameObject.useGravity = _HoldProfiler.useGravity;
        _CurrentHoldGameObject.constraints = _HoldProfiler.constraints | _OriginalConstraints;

        _Joint.connectedBody = _CurrentHoldGameObject;

        ApplyMotionSettings();

        float holdY = _InteractorCenterPos.eulerAngles.y;
        float objectY = _CurrentHoldGameObject.rotation.eulerAngles.y;

        _RotationOffset = Quaternion.Euler(0f, objectY - holdY, 0f);


        Debug.Log("Pull");
    }

    private void Release(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (_CurrentHoldGameObject == null)
            return;

        _CurrentHoldGameObject.constraints = _OriginalConstraints;
        _CurrentHoldGameObject.isKinematic = _IsKinematic;

        _Joint.connectedBody = null;
        _CurrentHoldGameObject = null;

        Debug.Log("Release");

    }

    private void CheckJointState()
    {
        if (!_CurrentHoldGameObject)
            return;

        float distance = Vector3.Distance(_InteractorCenterPos.position, _CurrentHoldGameObject.position);

        if (distance > _BreakDistance)
        {
            //_holdable.Release();
        }
    }


    private void UpdateRotation()
    {
        if (!_CurrentHoldGameObject)
            return;

        if (!_HoldProfiler.followRotation)
            return;

        Quaternion targetRot = Quaternion.Euler(0f, _InteractorCenterPos.eulerAngles.y, 0f) * _RotationOffset;

        _CurrentHoldGameObject.MoveRotation(Quaternion.Slerp(_CurrentHoldGameObject.rotation, targetRot, _SmoothSpeed * Time.fixedDeltaTime));
    }


    private void MoveHoldPosition()
    {
        if (!_CurrentHoldGameObject)
        {
            _HoldRb.MovePosition(_InteractorCenterPos.position);
            return;
        }

        Vector3 targetPos = GetTargetPosition();

        Vector3 smoothPos = Vector3.Lerp(_HoldRb.position, targetPos, _SmoothSpeed * Time.fixedDeltaTime);

        _HoldRb.MovePosition(smoothPos);
    }


    private void ApplyMotionSettings()
    {
        _Joint.xMotion = _HoldProfiler.xMotion;
        _Joint.yMotion = _HoldProfiler.yMotion;
        _Joint.zMotion = _HoldProfiler.zMotion;

        _Joint.angularXMotion = _HoldProfiler.angularXMotion;
        _Joint.angularYMotion = _HoldProfiler.angularYMotion;
        _Joint.angularZMotion = _HoldProfiler.angularZMotion;
    }


    private void CreateHoldBody()
    {
        _HoldBody = new GameObject("Physics Hold Body");

        _HoldRb = _HoldBody.AddComponent<Rigidbody>();
        _HoldRb.isKinematic = true;
        _HoldRb.useGravity = false;

        _Joint = _HoldBody.AddComponent<ConfigurableJoint>();

        _Joint.autoConfigureConnectedAnchor = false;

        _Joint.anchor = Vector3.zero;
        _Joint.connectedAnchor = Vector3.zero;

        _Joint.breakForce = Mathf.Infinity;
        _Joint.breakTorque = Mathf.Infinity;

        JointDrive drive = new JointDrive { positionSpring = _PositionSpring, positionDamper = _PositionDamper, maximumForce = _MaxForce };

        _Joint.xDrive = drive;
        _Joint.yDrive = drive;
        _Joint.zDrive = drive;

        SoftJointLimit limit = new SoftJointLimit { limit = _JointLimit };
        _Joint.linearLimit = limit;

        _Joint.configuredInWorldSpace = true;
        _Joint.projectionMode = JointProjectionMode.PositionAndRotation;
    }

    private Rigidbody ChechColiision()
    {
        Collider[] hits = Physics.OverlapBox(_InteractorCenterPos.position, _HalfExtends, Quaternion.identity, _InteractableMask);

        foreach (Collider col in hits)
        {
            if (col.gameObject.GetComponent<IPullable>() != null)
            {
                return col.gameObject.GetComponent<Rigidbody>();
            }
        }

        return null;
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 target = _InteractorCenterPos.position + _HalfExtends;

        target.y = _CurrentHoldGameObject.position.y;

        return target;
    }
}
