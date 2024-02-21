using System;
using UnityEngine;

public class ZombieFleeingState : ZombieState
{
    private float m_arrivedThreshold = 1f;

    public override void OnStart() 
    { 
        Debug.Log("Idle State State"); 
    }

    public override bool CanEnter(IState currentState)
    {  
        return false; 
    }

    public override void OnEnter()
    { 
        Debug.Log("Entering ZombieRunaway State"); 
    }

    public override bool CanExit() 
    { 
        return true; 
    }

    public override void OnExit()
    {  
        Debug.Log("Exiting ZombieRunaway State");  
    }

    public override void OnUpdate()
    { 
        Debug.Log("Idle ZombieRunaway OnUpdate");
        if (m_stateMachine.HasReachedDestination(m_stateMachine.m_preyPosition, m_arrivedThreshold)) return;
        GetAwayFromPrey();
    }

    private void GetAwayFromPrey()
    {
        m_stateMachine.m_agent.SetDestination(-m_stateMachine.m_preyPosition);
    }

    public override void OnFixedUpdate()
    { 
        Debug.Log("ZombieRunaway State OnFixedUpdate"); 
    }

}
