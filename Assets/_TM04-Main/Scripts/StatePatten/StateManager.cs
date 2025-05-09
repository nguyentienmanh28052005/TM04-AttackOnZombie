using System;
using NUnit.Framework;
using UnityEngine;

public class StateManager : MonoBehaviour
{

    [SerializeField] private IState _currentState;

    public void ChangeState(IState state)
    {
        if(_currentState != null && state.GetType() == _currentState.GetType()) return;
        if (_currentState != null)
            _currentState.Exit();

        _currentState = state;
        if (_currentState != null)
            _currentState.Enter();
    }

    private void Update()
    {
        if (_currentState != null)
            _currentState.Execute(); 
    }
}