
using UnityEngine;

public class ZombieFleeingState : ZombieState
{
    private float m_arrivedThreshold = 1f;
  

    public override void OnStart() 
    {
        Debug.Log("ZombieFleeingState OnStart()"); 
    }

    public override bool CanEnter(IState currentState)
    {  
        Debug.Log("ZombieFleeingState ZombieRunaway CanEnter: " + (m_stateMachine.m_health < ZombieFSM.MIN_HEALTH_TRIGGER_FEAR));
        return m_stateMachine.m_health < ZombieFSM.MIN_HEALTH_TRIGGER_FEAR; 
    }

    public override bool CanExit()
    {
        Debug.Log("CanExit  ZombieFleeingState State");
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
        if (m_stateMachine.HasReachedDestination(m_stateMachine.m_preyPosition, m_arrivedThreshold)) return;
        GetAwayFromPrey();
    }

    private void GetAwayFromPrey()
    {
        m_stateMachine.m_agent.SetDestination(-m_stateMachine.m_preyPosition);
    }

    public override void OnFixedUpdate()
    { 
        Debug.Log("ZombieFleeingState State OnFixedUpdate"); 
    }

}
