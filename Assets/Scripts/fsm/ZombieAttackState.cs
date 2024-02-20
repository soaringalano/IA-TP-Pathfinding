using UnityEngine;

public class ZombieAttackState : ZombieState
{
    public override void OnStart(){ Debug.Log("Idle State State"); }

    public override bool CanEnter(IState currentState){ return m_stateMachine.m_isPreyInReach; }

    public override bool CanExit(){ return !m_stateMachine.m_isPreyInReach;}

    public override void OnEnter(){ Debug.Log("Entering Attack State"); }

    public override void OnExit(){ Debug.Log("Exiting Attack State"); }

    public override void OnUpdate() { Debug.Log("Atack State OnUpdate"); }

    public override void OnFixedUpdate(){ Debug.Log("Idle State OnFixedUpdate"); }

}

//m_stateMachine.ZombieAnimator.SetBool("IsRunning", false);
//m_stateMachine.ZombieAnimator.SetBool("IsWalking", false);