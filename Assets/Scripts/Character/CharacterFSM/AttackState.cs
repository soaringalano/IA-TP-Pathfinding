using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using System.Threading;

public class AttackState : CharacterState
{

    public const string KEY_STATUS_TRIGGER_ATTACK = "CommAttack";

    private const float STATE_EXIT_TIMER = 1.0f;

    private float m_currentStateTimer = 0.0f;

    public override bool CanEnter(IState currentState)
    {
        if (currentState is FreeState)
        {
            bool onfloor = m_stateMachine.IsInContactWithFloor();


            return onfloor && Input.GetKeyDown(KeyCode.J);
        }
        return false;
    }

    public override bool CanExit()
    {
        return m_currentStateTimer <= 0;
    }

    public override void OnStart()
    {

    }

    public override void OnEnter()
    {
        Time.timeScale = 0.6f;
        ActivateAttackTrigger();
        m_currentStateTimer = STATE_EXIT_TIMER;
    }

    public override void OnExit()
    {
        Time.timeScale = 1.0f;
    }

    public override void OnFixedUpdate()
    {
        m_stateMachine.FixedUpdateQuickDeceleration();
    }

    public override void OnUpdate()
    {
        m_currentStateTimer -= Time.deltaTime;
    }

    public void ActivateAttackTrigger()
    {
        m_stateMachine.Animator.SetTrigger(KEY_STATUS_TRIGGER_ATTACK);
    }
}
