using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamingState : ZombieState
{
    Vector3 m_destination = Vector3.zero;
    private float m_arrivedThreshold = 1f;
    private float m_patrolInterval = 5f;
    private float m_timer;

    public override void OnStart()
    {
        Debug.Log("Roaming State OnStart");
        m_timer = m_patrolInterval;
        GenerateRandomDirection();
    }

    public override bool CanEnter(IState currentState){ return !m_stateMachine.m_isPreyInSight; }

    public override void OnEnter() { Debug.Log("Zombie Entering Roaming State"); }

    public override bool CanExit(){ return m_stateMachine.m_isPreyInSight; }

    public override void OnExit()
    {
        m_timer = m_patrolInterval;
        Debug.Log("Zombie Exiting Roaming State");
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
        if (!HasReachedDestination()) return;
        GenerateRandomDirection();
    }

    private void GenerateRandomDirection()
    {
        Vector3 randomDirection = Random.insideUnitSphere * m_stateMachine.patrolRange;
        randomDirection += m_stateMachine.transform.position;
        NavMeshHit hit;

        int walkableAreaMask = 1 << NavMesh.GetAreaFromName("Walkable");

        // Attempt to find a valid sample position within the walkable area of the NavMesh
        if (NavMesh.SamplePosition(randomDirection, out hit, m_stateMachine.patrolRange, walkableAreaMask))
        {
            // Keep the same Y coordinate as the starting position of the agent
            m_destination = hit.position;
            //Debug.Log("Final Position: " + m_destination);
            m_destination.y = m_stateMachine.transform.position.y;

            // Set the destination only if the sampled position is valid
            m_stateMachine.m_agent.SetDestination(m_destination);
        }
        else
        {
            Debug.LogWarning("Failed to find a valid patrol destination.");
        }
    }

    bool HasReachedDestination()
    {
        // Determine if the NPC has reached its destination
        // This could be based on distance to the destination point
        return Vector3.Distance(m_stateMachine.transform.position, m_destination) < m_arrivedThreshold;
    }
}

//m_stateMachine.ZombieAnimator.SetBool("IsRunning", false);
//m_stateMachine.ZombieAnimator.SetBool("IsWalking", true);