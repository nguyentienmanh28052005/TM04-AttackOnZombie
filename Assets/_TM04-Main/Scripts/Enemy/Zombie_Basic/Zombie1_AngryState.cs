using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie1_AngryState : IState
{
    private AEnemy _controller;
    private Animator _animator;

    public Zombie1_AngryState(AEnemy controller, Animator animator)
    {
        _controller = controller;
        _animator = animator;
    }
    public void Enter()
    {
        _controller.SetAngryState();
    }

    public void Execute()
    {
        _controller.MoveToPlayer();
    }

    public void Exit()
    {
        
    }
}
