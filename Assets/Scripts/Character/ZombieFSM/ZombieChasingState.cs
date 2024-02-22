using UnityEngine;

public class ZombieChasingState : ZombieState
{
    private float m_arrivedThreshold = 0.5f;

    public override void OnStart()
    { 
    }

    public override bool CanEnter(IState currentState)  
    { 
        return m_stateMachine.m_isPreyInSight;
    }

    public override bool CanExit()
    {
        return !m_stateMachine.m_isPreyInSight ||
           m_stateMachine.m_health < ZombieFSM.MIN_HEALTH_TRIGGER_FEAR;
    }

    public override void OnEnter() 
    { 
        GetToPrey();
    }


    public override void OnExit()
    {
        Debug.Log("Exiting Chasing State");
    }

    public override void OnUpdate()
    {
        if (m_stateMachine.HasReachedDestination(m_stateMachine.m_preyPosition, m_arrivedThreshold))
        {
            if (!m_stateMachine.m_isPreyInReach)
            {
                GetToPrey();
            }

            return;
        }

        GetToPrey();
    }

    private void GetToPrey()
    {
        m_stateMachine.m_agent.isStopped = true;
        m_stateMachine.m_agent.ResetPath();
        m_stateMachine.m_lastKnownPreyPosition = m_stateMachine.m_preyPosition;
        m_stateMachine.m_agent.SetDestination(m_stateMachine.m_preyPosition);
        m_stateMachine.m_agent.isStopped = false;
    }

    public override void OnFixedUpdate()
    {

    }
}
