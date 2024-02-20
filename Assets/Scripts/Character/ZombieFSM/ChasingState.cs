using UnityEngine;

public class ChasingState : ZombieState
{
    public override void OnStart(){ Debug.Log("Chasing State OnStart"); }

    public override bool CanEnter(IState currentState)  { return m_stateMachine.m_isPreyInSight; }

    public override void OnEnter() { Debug.Log("Zombie Entering Chasing State"); }

    public override bool CanExit(){  return !m_stateMachine.m_isPreyInSight; }

    public override void OnExit()
    {
        //m_stateMachine.ZombieAnimator.SetBool("IsRunning", false);
        Debug.Log("Exiting Chasing State");
    }

    public override void OnUpdate() { Debug.Log("Chasing State OnUpdate");  }

    public override void OnFixedUpdate()
    {
        //m_stateMachine.ZombieAnimator.SetBool("IsWalking", false);
        //m_stateMachine.ZombieAnimator.SetBool("IsRunning", true);
        //m_stateMachine.GoToDirection(m_stateMachine.m_preyPosition);
    }
}
