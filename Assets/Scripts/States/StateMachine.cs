using System;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private IState _currentState;
    public event Action<IState> OnStateChanged;

    public void Initialize(IState state)
    {
        if (state == null) return;
        _currentState = state;
        state.Enter();
        OnStateChanged?.Invoke(state);
    }

    public void TransitionTo(IState nextState)
    {
        if (nextState == null) return;
        _currentState.Exit();
        _currentState = nextState;
        nextState.Enter();
        OnStateChanged?.Invoke(nextState);
    }

    private void FixedUpdate()
    {
        _currentState?.Update();
    }
}
