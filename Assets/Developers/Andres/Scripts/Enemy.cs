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

    [Header("Detector Settings")]
    [SerializeField] private float _Radius = 5f;
    [SerializeField] private LayerMask _Detectable;

    private Collider[] _RangeZone;
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
        _Player = FindFirstObjectByType<PlayerController>().gameObject;
    }

    private void Start()
    {
        int randomPoint = Random.Range(0, _PatrolPoints.Count);
        _CurrentPoint = randomPoint;

        EventSystemController.eventSystemController.onEndGame += () => FreezEnemy(true);
        EventSystemController.eventSystemController.onRestart += () => FreezEnemy(false);
    }

    private void Update()
    {
        if (CheckDistanceWithPlayer())
            EventSystemController.eventSystemController.EndGame();

        if (CheckIfPlayerInsideRange())
            _CurrentState = States.chasing;
        else
            _CurrentState = States.patrol;

        _StateMachine[_CurrentState].Invoke();

        Debug.Log($"Enemy current state: {_CurrentState}");
    }

    private void Patrol()
    {
        if (_CurrentCoroutine != null)
            return;

        if(_PatrolPoints.Count <= 0)
        {
            Debug.Log("There are no patrol points");
            return;
        }

        _Agent.speed = _PatrolSpeed;

        if(!_Agent.pathPending && _Agent.remainingDistance <= _DistanceToPoint)
            _CurrentCoroutine = StartCoroutine(StartPatrol());

    }

    private void Chasing()
    {

        if (_CurrentCoroutine != null)
            return;

        _Agent.speed = _ChasingSpeed;

        _CurrentCoroutine = StartCoroutine(StartChasing());
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

    private IEnumerator StartChasing()
    {
        while (true)
        {
            if (CheckIfPlayerInsideRange())
            {
                _Agent.SetDestination(_Player.transform.position);
            }
            else
                _CurrentCoroutine = null;

            yield return new WaitForSeconds(0.2f);
        }
    }

    private bool CheckIfPlayerInsideRange()
    {
        _RangeZone = Physics.OverlapSphere(transform.position, _Radius, _Detectable);

        foreach (Collider collision in _RangeZone)
        {
            if (collision.GetComponent<PlayerController>() != null)
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckDistanceWithPlayer()
    {
        Vector3 currentPos = transform.position;
        Vector3 playerPos = _Player.transform.position;

        float distance = Vector3.Distance(currentPos, playerPos);

        if (distance <= _MinDistancePlayer)
            return true;

        return false;
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

        Gizmos.DrawWireSphere(transform.position, _Radius);
    }

    private enum States 
    {
        patrol,
        chasing
    }

}
