using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryState : CharacterState
{

    private const string KEY_STATE_TRIGGER_VICTORY = "Victory";

    private const float STATE_EXIT_TIMER = 3.0f;

    private float m_timer = 0;

    public override void OnStart()
    {

    }

    public override void OnEnter()
    {
        Debug.Log("Entering Victory state");
        m_stateMachine.Animator.SetTrigger(KEY_STATE_TRIGGER_VICTORY);
    }

    public override void OnExit()
    {
        Debug.Log("Exiting Victory state");
        m_stateMachine.SetEnemyDefeated(false);
        m_timer = 0.0f;
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
        m_timer += Time.deltaTime;
    }

    public override bool CanEnter(IState currentState)
    {
        return m_stateMachine.IsEnemyDefeated();
    }

    public override bool CanExit()
    {
        return m_timer >= STATE_EXIT_TIMER;
    }
}