using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie1_WalkState : IState
{
    private AEnemy _controller;
    private Animator _animator;

    public Zombie1_WalkState(AEnemy controller, Animator animator)
    {
        _controller = controller;
        _animator = animator;
    }
    public void Enter()
    {
        
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
}
