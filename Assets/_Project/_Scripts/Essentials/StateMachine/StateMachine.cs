using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. References all states
// 2. Oversees the active state
// 3. Handles transitions between states
// 4. Calls methods of states such EnterState, ExitState, UpdateState, etc.
public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
    #region Fields
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> CurrentState;

    protected bool IsTransitioningState = false;
    #endregion

    #region Monobehavior Callbacks
    void Start()
    {
        CurrentState.EnterState();
    }

    void Update()
    {
        EState nextStateKey = CurrentState.GetNextState();
        if (!IsTransitioningState && nextStateKey.Equals(CurrentState.StateKey))
            CurrentState.UpdateState();
        else if (!IsTransitioningState)
            TransitionToState(nextStateKey);
    }

    public void TransitionToState(EState nextStateKey)
    {
        IsTransitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[nextStateKey];
        CurrentState.EnterState();
        IsTransitioningState = false;
    }

    void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }

    void OnTriggerStay(Collider other)
    {
        CurrentState.OnTriggerStay(other);
    }

    void OnTriggerExit(Collider other)
    {
        CurrentState.OnTriggerExit(other);
    }
    #endregion

    #region Custom Functions
    #endregion
}
