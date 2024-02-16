using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingState : ZombieState
{
    private float m_roamingCountdown = 16f;
    private float m_newDirectionCountdown = 8f;
    

    public override bool CanEnter(IState currentState)
    {
        return !m_stateMachine.m_isPreyInSight;
    }

    public override bool CanExit()
    {
        return m_roamingCountdown <= 0f;
    }

    public override void OnEnter()
    {
        //m_stateMachine.ZombieAnimator.SetBool("IsRunning", false);
        //m_stateMachine.ZombieAnimator.SetBool("IsWalking", true);
        Debug.Log("Entering Roaming State");
    }

    public override void OnExit()
    {
        Debug.Log("Exiting Roaming State");
        m_roamingCountdown = 16f;
    }

    public override void OnFixedUpdate()
    {
        //Debug.Log("Roaming State OnFixedUpdate");
        if (m_newDirectionCountdown <= 0f)
        {
            m_newDirectionCountdown = 8f;
            m_stateMachine.m_newDirection = new Vector3(Random.Range(-30f, 30f), Random.Range(-30f, 30f), Random.Range(-30f, 30f));
            m_stateMachine.GoToDirection(m_stateMachine.m_newDirection);
        }
    }

    public override void OnStart()
    {
        Debug.Log("Roaming State OnStart");
    }

    public override void OnUpdate()
    {
        //Debug.Log("Roaming State OnUpdate");
        m_roamingCountdown -= Time.deltaTime;
        m_newDirectionCountdown -= Time.deltaTime;


    }
}
