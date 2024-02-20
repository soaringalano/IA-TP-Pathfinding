using System.Collections.Generic;
using System.Net;
using UnityEngine;

public  class AbstractFSM<T> : MonoBehaviour where T : IState
{
    protected T m_currentState;
    protected List<T> m_possibleStates;

    static public Transform Scene { get; private set; }

    protected virtual void CreatePossibleStates() { }

    protected virtual void Awake(){ CreatePossibleStates();}

    protected virtual void Start()
    {
        foreach (IState state in m_possibleStates){ state.OnStart(); }
        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();
    }

    protected virtual void Update()
    {
        //Debug.Log("Current state:" + m_currentState.GetType());
        m_currentState.OnUpdate();
        TryStateTransition();
    }

    protected virtual void FixedUpdate(){m_currentState.OnFixedUpdate();} 

    protected void TryStateTransition()
    {
        if (!m_currentState.CanExit())
            return;
       
        //Je PEUX quitter le state actuel
        foreach (var state in m_possibleStates)
        {
            if (m_currentState.Equals(state))//if same state do nothing
                continue;

            if (state.CanEnter(m_currentState))
            {    
                m_currentState.OnExit();//Quit actual state
                m_currentState = state; //set currState 2 newState
                m_currentState.OnEnter();//Rentrer dans le state state
                return;
            }
        }
    }
}
