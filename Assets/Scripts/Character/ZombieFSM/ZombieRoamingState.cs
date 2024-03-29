using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieRoamingState : ZombieState
{
    Vector3 m_destination = Vector3.zero;
    private float m_arrivedThreshold = 1f;
    private float m_patrolInterval = 5f;
    private float m_timer;

    public override void OnStart()
    {
        //Debug.Log("Roaming State OnStart");
        m_timer = m_patrolInterval;
    }

    public override bool CanEnter(IState currentState)
    { 
        return !m_stateMachine.m_isPreyInSight;
    }

    public override void OnEnter() 
    { 
        Debug.Log("Zombie Entering Roaming State");

        if (m_stateMachine.m_lastKnownPreyPosition != Vector3.zero)
        {
            m_destination = m_stateMachine.m_lastKnownPreyPosition;
            m_stateMachine.m_agent.SetDestination(m_destination);
        }
        else
        {
            GenerateRandomDirection();
        }
    }

    public override bool CanExit()
    {
        return m_stateMachine.m_isPreyInSight ||
            m_stateMachine.m_health < ZombieFSM.MIN_HEALTH_TRIGGER_FEAR; 
    }

    public override void OnExit()
    {
        m_timer = m_patrolInterval;
    }

    public override void OnUpdate()
    {
        m_timer -= Time.deltaTime;
        if (m_timer <= 0f)
        {
            Patrol();
            m_timer = m_patrolInterval; // Reset the timer
        }
    }

    public override void OnFixedUpdate() { }

    void Patrol()
    {
        if (!m_stateMachine.HasReachedDestination(m_destination, m_arrivedThreshold)) return;
        GenerateRandomDirection();
    }

    private void GenerateRandomDirection()
    {
        Vector3 randomDirection = Random.insideUnitSphere * m_stateMachine.patrolRange;
        randomDirection += m_stateMachine.transform.position;
        NavMeshHit hit;

        int walkableAreaMask = 1 << NavMesh.GetAreaFromName("Walkable");

        if (NavMesh.SamplePosition(randomDirection, out hit, m_stateMachine.patrolRange, walkableAreaMask))
        {
            m_destination = hit.position;
            m_stateMachine.m_agent.isStopped = true;
            m_stateMachine.m_agent.ResetPath();
            m_destination.y = m_stateMachine.transform.position.y;
            m_stateMachine.m_agent.SetDestination(m_destination);
            m_stateMachine.m_agent.isStopped = false;
        }
        else
        {
            Debug.LogWarning("Failed to find a valid patrol destination.");
        }
    }
}
