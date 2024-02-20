using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamingState : ZombieState
{
    private float patrolInterval = 5f;
    private float timer;
    
    public override void OnStart()
    {
        Debug.Log("Roaming State OnStart");
        timer = patrolInterval;
    }

    public override bool CanEnter(IState currentState){ return !m_stateMachine.m_isPreyInSight; }

    public override void OnEnter() { Debug.Log("Zombie Entering Roaming State"); }

    public override bool CanExit(){ return m_stateMachine.m_isPreyInSight; }

    public override void OnExit()
    {
        timer = patrolInterval;
        Debug.Log("Zombie Exiting Roaming State");
    }

    public override void OnUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Patrol();
            timer = patrolInterval; // Reset the timer
        }
    }

    public override void OnFixedUpdate() { }

    void Patrol()
    {
        Vector3 randomDirection = Random.insideUnitSphere * m_stateMachine.patrolRange;
        randomDirection += m_stateMachine.transform.position;
        NavMeshHit hit;

        int walkableAreaMask = 1 << NavMesh.GetAreaFromName("Walkable"); 

        // Attempt to find a valid sample position within the walkable area of the NavMesh
        if (NavMesh.SamplePosition(randomDirection, out hit, m_stateMachine.patrolRange, walkableAreaMask))
        {
            // Keep the same Y coordinate as the starting position of the agent
            Vector3 finalPosition = hit.position;
            finalPosition.y = m_stateMachine.transform.position.y;

            // Set the destination only if the sampled position is valid
            m_stateMachine.m_agent.SetDestination(finalPosition);
        }
        else
        {
            Debug.LogWarning("Failed to find a valid patrol destination.");
        }
    }




}

//m_stateMachine.ZombieAnimator.SetBool("IsRunning", false);
//m_stateMachine.ZombieAnimator.SetBool("IsWalking", true);