﻿
using System.Collections.Generic;
using UnityEngine;

public class AbstractStateMachine<T> : MonoBehaviour where T : IState
{
    protected T m_currentState;
    protected List<T> m_possibleStates;

    protected virtual void Awake()
    {
        CreatePossibleStates();
    }

    protected virtual void Start()
    {
        foreach (IState state in m_possibleStates)
        {
            state.OnStart();
        }
        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();
    }

    protected virtual void Update()
    {
        //Debug.Log("Current state:" +  m_currentState.GetType());
        m_currentState.OnUpdate();
        TryStateTransition();
    }

    protected virtual void FixedUpdate()
    {
        //Debug.Log("Current state:" + m_currentState.GetType());
        m_currentState.OnFixedUpdate();
    }

    protected virtual void CreatePossibleStates()
    {

    }

    protected void TryStateTransition()
    {
        if (!m_currentState.CanExit())
        {
            return;
        }

        foreach (var state in m_possibleStates)
        {
            //Debug.Log("State: " + state.GetType());
            if (m_currentState.Equals(state))
            {
                continue;
            }

            if (state.CanEnter(m_currentState))
            {
                //Quitter le state actuel
                m_currentState.OnExit();
                m_currentState = state;
                //Rentrer dans le state state
                m_currentState.OnEnter();
                return;
            }
        }
    }
}
