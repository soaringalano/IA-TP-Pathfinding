using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieFSM : AbstractStateMachine<ZombieState>
{
    public Rigidbody RB { get; private set; }
    public Animator ZombieAnimator { get; private set; }

    public Vector3 m_preyPosition = Vector3.zero;
    public Vector3 m_newDirection = Vector3.zero;

    public float m_health = 100f;
    public const float MIN_HEALTH_TRIGGER_FEAR = 80f;

    public bool m_isPreyInSight = false;
    public bool m_isPreyInReach = false;

    public NavMeshAgent m_agent;

    [SerializeField]
    public float patrolRange = 30f;

    protected override void Start()
    {
        RB = GetComponent<Rigidbody>();
        m_agent = GetComponent<NavMeshAgent>();

        if (RB == null) Debug.LogError("RB is null");

        foreach (ZombieState state in m_possibleStates)
        {
            state.OnStart(this);
        }
 
        base.Start();
        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();
    }


    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<ZombieState>();
        m_possibleStates.Add(new ZombieRoamingState());
        m_possibleStates.Add(new ZombieChasingState());
        m_possibleStates.Add(new ZombieAttackState());
        m_possibleStates.Add(new ZombieFleeingState());
    }

    public bool HasReachedDestination(Vector3 destination, float threshold)
    {
        // Determine if the NPC has reached its destination
        // This could be based on distance to the destination point
        return Vector3.Distance(transform.position, destination) < threshold;
    }
}
