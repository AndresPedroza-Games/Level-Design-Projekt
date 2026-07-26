using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Patrol Settings")]
    [SerializeField] private float _PatrolSpeed = 2f;
    [SerializeField] private float _DistanceToPoint = 0.1f;
    [SerializeField] private float _WaitingTime = 1f;
    [SerializeField] private List<Transform> _PatrolPoints;

    [Header("Enemy Chasing Settings")]
    [SerializeField] private float _ChasingSpeed = 2f;
    [SerializeField] private float _MinDistancePlayer = 0.5f;
    [SerializeField] private float _ViewDistance = 5f;
    [SerializeField] private float _ViewAngle = 90f;
    [SerializeField] private float _LosePlayerTime = 1f;
    [SerializeField] private LayerMask _DetectionLayer;

    private float _TimeSinceLostPlayer;

    private GameObject _Player;

    private Dictionary<States, UnityAction> _StateMachine;

    private States _CurrentState;
    private int _CurrentPoint;

    private Coroutine _CurrentCoroutine;

    private NavMeshAgent _Agent;

    private void Awake()
    {
        _CurrentState = States.patrol;

        _StateMachine = new()
        {
            {States.patrol, Patrol},
            {States.chasing, Chasing}
        };

        _Agent = GetComponent<NavMeshAgent>();
        _Player = FindFirstObjectByType<Player>()._CameraTarget.gameObject;
    }

    private void Start()
    {
        int randomPoint = Random.Range(0, _PatrolPoints.Count);
        _CurrentPoint = randomPoint;

        EventSystemController.eventSystemController.onEndGame += () => FreezEnemy(true);
        EventSystemController.eventSystemController.onRestart += () => FreezEnemy(false);

        _CurrentState = States.patrol;
    }

    private void Update()
    {
        if (CheckDistanceWithPlayer() <= _MinDistancePlayer)
            EventSystemController.eventSystemController.EndGame();

        _StateMachine[_CurrentState].Invoke();

        // Debug.Log($"Enemy current state: {_CurrentState}");
    }

    private void Patrol()
    {
        if (_CurrentCoroutine != null || _PatrolPoints.Count <= 0)
            return;

        if(CheckDistanceWithPlayer() <= _ViewDistance && CanSeePlayer())
            _CurrentState = States.chasing;

        _Agent.speed = _PatrolSpeed;

        if(!_Agent.pathPending && _Agent.remainingDistance <= _DistanceToPoint)
            _CurrentCoroutine = StartCoroutine(StartPatrol());
    }

    private void Chasing()
    {
        _Agent.speed = _ChasingSpeed;
        _Agent.SetDestination(_Player.transform.position);

        if (!CanSeePlayer())
        {
            _TimeSinceLostPlayer += Time.deltaTime;
            if (_TimeSinceLostPlayer >= _LosePlayerTime)
                _CurrentState = States.patrol;
        }
        else
            _TimeSinceLostPlayer = 0;
    }

    private IEnumerator StartPatrol()
    {
        _Agent.isStopped = true;

        yield return new WaitForSeconds(_WaitingTime);

        _Agent.isStopped = false;
        GoToNextPoint();
    }

    private void GoToNextPoint()
    {
        _Agent.SetDestination(_PatrolPoints[_CurrentPoint].position);

        int randomPoint = Random.Range(0, _PatrolPoints.Count);
        _CurrentPoint = randomPoint;

        _CurrentCoroutine = null;
    }

    private bool CanSeePlayer()
    {
        return IsFacingPlayer() && HasClearPathToPlayer();
    }

    private bool IsFacingPlayer()
    {
        Vector3 directionToPlayer = (_Player.transform.position - transform.position).normalized;
        directionToPlayer.y = 0;

        Vector3 forward = transform.forward;
        forward.y = 0;

        float angle = Vector3.Angle(forward.normalized, directionToPlayer);

        return angle <= _ViewAngle / 2f;
    }

    private bool HasClearPathToPlayer()
    {
        Vector3 directionToPlayer = _Player.transform.position - transform.position;

        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out RaycastHit hit, directionToPlayer.magnitude))
            return hit.transform.root == _Player.transform.root;

        return true;
    }

    private float CheckDistanceWithPlayer()
    {
        Vector3 currentPos = transform.position;
        Vector3 playerPos = _Player.transform.position;

        float distance = Vector3.Distance(currentPos, playerPos);

        return distance;
    }


    private void FreezEnemy(bool status)
    {
        if(status)
            StopAllCoroutines();

        this.enabled = !status;
        _Agent.isStopped = !status; 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 origin = transform.position;

        Gizmos.DrawLine(origin,origin + transform.forward * _ViewDistance);

        Vector3 leftBoundary = Quaternion.Euler(0, -_ViewAngle / 2f, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, _ViewAngle / 2f, 0) * transform.forward;

        Gizmos.DrawLine(origin,origin + leftBoundary * _ViewDistance);

        Gizmos.DrawLine(origin,origin + rightBoundary * _ViewDistance);

        if(_Player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(origin, _Player.transform.position);
        }
    }

    private enum States 
    {
        patrol,
        chasing
    }

}
