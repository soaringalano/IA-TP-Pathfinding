using UnityEngine;

public class ZombieAttackState : ZombieState
{
    public override void OnStart()
    { 
        //Debug.Log("ZombieAttackState OnStart()"); 
    }

    public override bool CanEnter(IState currentState)
    { 
        return m_stateMachine.m_isPreyInReach; 
    }

    public override bool CanExit()
    {
        //Debug.Log("ZombieAttackState CanExit() : " + (m_stateMachine.m_health < ZombieFSM.MIN_HEALTH_TRIGGER_FEAR));
        //return !m_stateMachine.m_isPreyInReach ||
        //    m_stateMachine.m_health < ZombieFSM.MIN_HEALTH_TRIGGER_FEAR;
        return true;
    }

    public override void OnEnter()
    { 
        //Debug.Log("Entering Attack State"); 
    }

    public override void OnExit()
    { 
        //Debug.Log("Exiting Attack State"); 
    }

    public override void OnUpdate() 
    { 
        //Debug.Log("Atack State OnUpdate"); 
    }

    public override void OnFixedUpdate()
    { 
        //Debug.Log("ZombieAttackState OnFixedUpdate"); 
    }

}

//m_stateMachine.ZombieAnimator.SetBool("IsRunning", false);
//m_stateMachine.ZombieAnimator.SetBool("IsWalking", false);