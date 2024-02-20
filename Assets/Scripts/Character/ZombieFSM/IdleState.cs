using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : ZombieState
{
    private float m_idleCountdown = 5f;

    public override bool CanEnter(IState currentState)
    {
        return !m_stateMachine.m_isPreyInSight;
    }

    public override bool CanExit()
    {
        return m_idleCountdown <= 0f;
    }

    public override void OnEnter()
    {
        m_stateMachine.ZombieAnimator.SetBool("IsRunning", false);
        m_stateMachine.ZombieAnimator.SetBool("IsWalking", false);
        Debug.Log("Entering Idle State");
    }

    public override void OnExit()
    {
        Debug.Log("Exiting Idle State");
        m_idleCountdown = 5f;
    }

    public override void OnFixedUpdate()
    {
        //Debug.Log("Idle State OnFixedUpdate");
    }

    public override void OnStart()
    {
        Debug.Log("Idle State State");
    }

    public override void OnUpdate()
    {
        //Debug.Log("Idle State OnUpdate");

    }
}
