using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie1_IdleState : IState
{
    private AEnemy _controller;
    private Animator _animator;

    public Zombie1_IdleState(AEnemy controller, Animator animator)
    {
        _controller = controller;
        _animator = animator;
    }
    public void Enter()
    {
        _controller.SetIdleState();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
}
