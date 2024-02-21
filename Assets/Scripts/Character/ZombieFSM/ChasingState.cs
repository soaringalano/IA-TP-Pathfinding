using UnityEngine;

public class ChasingState : ZombieState
{
    private float m_arrivedThreshold = 0.5f;

    public override void OnStart()
    { 
        Debug.Log("Chasing State OnStart");
    }

    public override bool CanEnter(IState currentState)  
    { 
        return m_stateMachine.m_isPreyInSight; 
    }

    public override void OnEnter() 
    { 
        Debug.Log("Zombie Entering Chasing State");
        GetToPrey();
    }

    public override bool CanExit()
    { 
        return !m_stateMachine.m_isPreyInSight; 
    }

    public override void OnExit()
    {
        //m_stateMachine.ZombieAnimator.SetBool("IsRunning", false);
        Debug.Log("Exiting Chasing State");
    }

    public override void OnUpdate()
    {
        //Debug.Log("Chasing State OnUpdate");

        if (!m_stateMachine.HasReachedDestination(m_stateMachine.m_preyPosition, m_arrivedThreshold) || m_stateMachine.m_isPreyInReach) return;
        Debug.Log("Chasing State OnUpdate, has not reach prey: " + !m_stateMachine.HasReachedDestination(m_stateMachine.m_preyPosition, m_arrivedThreshold) + " prey in reach: " + m_stateMachine.m_isPreyInReach);
        GetToPrey();
    }

    private void GetToPrey()
    {
        m_stateMachine.m_agent.isStopped = true;
        m_stateMachine.m_agent.ResetPath();
        m_stateMachine.m_agent.SetDestination(m_stateMachine.m_preyPosition);
        m_stateMachine.m_agent.isStopped = false;
    }

    public override void OnFixedUpdate()
    {
        //m_stateMachine.ZombieAnimator.SetBool("IsWalking", false);
        //m_stateMachine.ZombieAnimator.SetBool("IsRunning", true);
        //m_stateMachine.GoToDirection(m_stateMachine.m_preyPosition);
    }
}
