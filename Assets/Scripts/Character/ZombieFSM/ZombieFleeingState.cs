using UnityEngine;
using UnityEngine.AI;

public class ZombieFleeingState : ZombieState
{
    Vector3 m_destination = Vector3.zero;
    private float m_arrivedThreshold = 0.1f;

    public override void OnStart()
    {
        //Debug.Log("ZombieFleeingState OnStart()");
    }

    public override bool CanEnter(IState currentState)  
    {
        Debug.Log("ZombieFleeingState ZombieRunaway CanEnter health: " + m_stateMachine.m_health);
        Debug.Log("ZombieFleeingState ZombieRunaway CanEnter: " + (m_stateMachine.m_health < ZombieFSM.MIN_HEALTH_TRIGGER_FEAR));
        return m_stateMachine.m_health < ZombieFSM.MIN_HEALTH_TRIGGER_FEAR;
    }

    public override bool CanExit()
    {
        return false;
    }

    public override void OnEnter() 
    {
        Debug.Log("Entering ZombieFleeingState State");
        GetAwayFromPrey();
    }


    public override void OnExit()
    {
        Debug.Log("Exiting ZombieFleeingState State");
    }

    public override void OnUpdate()
    {
        Debug.Log("ZombieFleeingState OnUpdate");
        if (!m_stateMachine.HasReachedDestination(m_destination, m_arrivedThreshold)) return;
        GetAwayFromPrey();
    }

    private void GetAwayFromPrey()
    {
        Debug.Log("ZombieFleeingState GetAwayFromPrey");

        Vector3 randomDirection = Random.insideUnitSphere * m_stateMachine.patrolRange;
        Vector3 potentialDestination = m_stateMachine.transform.position + randomDirection - m_stateMachine.m_preyPosition;

        NavMeshHit hit;

        int walkableAreaMask = 1 << NavMesh.GetAreaFromName("Walkable");

        if (NavMesh.SamplePosition(potentialDestination, out hit, m_stateMachine.patrolRange, walkableAreaMask))
        {
            m_stateMachine.m_agent.isStopped = true;
            m_stateMachine.m_agent.ResetPath();
            m_destination = hit.position;
            m_destination.y = m_stateMachine.transform.position.y;
            m_stateMachine.m_agent.SetDestination(m_destination);
            m_stateMachine.m_agent.isStopped = false;
        }
        else
        {
            Debug.LogWarning("Failed to find a valid patrol destination.");
            GetAwayFromPrey();
        }
    }

    public override void OnFixedUpdate()
    {
    }
}
