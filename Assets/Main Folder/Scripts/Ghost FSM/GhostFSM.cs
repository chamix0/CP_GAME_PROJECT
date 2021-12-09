using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = System.Random;

public class GhostFSM : MonoBehaviour
{
    #region variables

    private StateMachineEngine GhostFSM_FSM;

    private PushPerception keepWanderingPerception;
    private PushPerception keepChasingPerception;
    private PushPerception humanInRangePerception;
    private PushPerception humanDoesNotDefendItselfPerception;
    private PushPerception humanDefendsItselfPerception;
    private PushPerception humanDiedPerception;
    private PushPerception ghostHasEscapedPerception;
    private State wanderAroundState;
    private State chaseHumanState;
    private State attackState;
    private State runAwayState;

    //Place your variables here

    private static Random _random = new Random();

    private List<Transform> destinations = new List<Transform>();

    private NavMeshAgent _navMeshAgent;
    private Vector3 destination;

    private Perception _transition;

    private bool _attacked;

    private Transform _humanChased;

    [SerializeField] private Text _text;

    [Range(1.0f, 5.0f)] [SerializeField] private float _distanceToAttack;
    
    [Range(1.0f, 5.0f)] [SerializeField] private int _attackDuration;
    
    [Range(1.0f, 10f)] [SerializeField] private float _movementSpeed;

    #endregion variables

    private void Awake()
    {
        var destinationsParentTransform = GameObject.Find("Ghost Destinations").transform;
        foreach (Transform item in destinationsParentTransform)
        {
            destinations.Add(item);
        }

        destinations = new List<Transform>(destinations.OrderBy(n => _random.Next()));
    }

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _movementSpeed;
        GhostFSM_FSM = new StateMachineEngine(false);


        CreateStateMachine();
    }


    private void CreateStateMachine()
    {
        // Perceptions
        // Modify or add new Perceptions, see the guide for more
        keepWanderingPerception = GhostFSM_FSM.CreatePerception<PushPerception>();
        keepChasingPerception = GhostFSM_FSM.CreatePerception<PushPerception>();
        humanInRangePerception = GhostFSM_FSM.CreatePerception<PushPerception>();
        humanDoesNotDefendItselfPerception = GhostFSM_FSM.CreatePerception<PushPerception>();
        humanDefendsItselfPerception = GhostFSM_FSM.CreatePerception<PushPerception>();
        humanDiedPerception = GhostFSM_FSM.CreatePerception<PushPerception>();
        ghostHasEscapedPerception = GhostFSM_FSM.CreatePerception<PushPerception>();

        // States
        wanderAroundState = GhostFSM_FSM.CreateEntryState("Wander around", WanderAroundAction);
        chaseHumanState = GhostFSM_FSM.CreateState("Chase human", ChaseHumanAction);
        attackState = GhostFSM_FSM.CreateState("Attack", AttackAction);
        runAwayState = GhostFSM_FSM.CreateState("Run away", RunAwayAction);

        // Transitions
        GhostFSM_FSM.CreateTransition("Keep wandering", wanderAroundState, keepWanderingPerception, wanderAroundState);
        GhostFSM_FSM.CreateTransition("Human in range", wanderAroundState, humanInRangePerception, chaseHumanState);
        GhostFSM_FSM.CreateTransition("Keep chasing", chaseHumanState, keepChasingPerception, chaseHumanState);
        GhostFSM_FSM.CreateTransition("Human doesn't defend itself", chaseHumanState,
            humanDoesNotDefendItselfPerception, attackState);
        GhostFSM_FSM.CreateTransition("Human defends itself", chaseHumanState, humanDefendsItselfPerception,
            runAwayState);
        GhostFSM_FSM.CreateTransition("Human died", attackState, humanDiedPerception, wanderAroundState);
        GhostFSM_FSM.CreateTransition("Ghost has escaped", runAwayState, ghostHasEscapedPerception, wanderAroundState);

        // ExitPerceptions

        // ExitTransitions
    }

    private void Update()
    {
        GhostFSM_FSM.Update();


        if (IsWanderingAround() && HasReachedDestination())
            _transition = keepWanderingPerception;
        else if (IsChasingHuman())
        {
            if (!HasReachedHuman())
                _transition = keepChasingPerception;
            else
                TryAttack();
        } else if (IsRunningAway() && HasReachedDestination())
        {
            _transition = ghostHasEscapedPerception;
            _attacked = false;
        }

        if (_transition != null)
            GhostFSM_FSM.Fire(_transition);
    }

    private bool IsRunningAway()
    {
        return GhostFSM_FSM.GetCurrentState().Equals(runAwayState);
    }

    private void TryAttack()
    {
        _transition = _humanChased.GetComponent<CharacterManager>().lightManager.CanDefend(this.transform.position)
            ? humanDefendsItselfPerception
            : humanDoesNotDefendItselfPerception;
    }

    private bool IsWanderingAround()
    {
        return GhostFSM_FSM.GetCurrentState().Equals(wanderAroundState);
    }

    private bool IsChasingHuman()
    {
        return GhostFSM_FSM.GetCurrentState().Equals(chaseHumanState);
    }

    private bool HasReachedDestination()
    {
        return Vector3.Distance(transform.position, destination) < 1.0f;
    }

    private bool HasReachedHuman()
    {
        return Vector3.Distance(transform.position, _humanChased.position) < _distanceToAttack;
    }


    // Create your desired actions

    private void WanderAroundAction()
    {
        _text.text = "Wandering Around";
        destination = destinations[_random.Next(destinations.Count)].position;
        _navMeshAgent.SetDestination(destination);
        _transition = null;
    }

    private void ChaseHumanAction()
    {
        _text.text = "Chasing Human";
        _navMeshAgent.SetDestination(_humanChased.position);
        _transition = null;
    }

    private void AttackAction()
    {
        _text.text = "Attacking Human";
        //_transition = humanDiedPerception;
        StartCoroutine(Attack(_attackDuration));
        _transition = null;
    }

    private void RunAwayAction()
    {
        _text.text = "Running Away";
        _attacked = true;

        var farthestDestination = transform.position;
        foreach (var destination in destinations)
        {
            if (Vector3.Distance(transform.position, destination.position) >
                Vector3.Distance(transform.position, farthestDestination))
                farthestDestination = destination.position;
        }

        destination = farthestDestination;
        _navMeshAgent.SetDestination(destination);
        _transition = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_attacked || !other.gameObject.tag.Equals("Player")) return;
        if (other.gameObject.GetComponent<PlayerInfo>().isDead) return;
        _transition = humanInRangePerception;
        _humanChased = other.gameObject.transform;
    }

    private IEnumerator Attack(int seconds)
    {
        _navMeshAgent.speed = 0.0f;
        _humanChased.GetComponent<PlayerInfo>().Die();
        yield return new WaitForSeconds(seconds);
        _navMeshAgent.speed = _movementSpeed;
        _transition = humanDiedPerception;
    }
}