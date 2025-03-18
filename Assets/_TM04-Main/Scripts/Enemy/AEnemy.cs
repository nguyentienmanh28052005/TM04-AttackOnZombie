using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEnemy : AObserver
{
    protected Animator _animator;
    protected GameObject _player;

    
    protected static int id = 0;
    protected float dmg;
    protected float originSpeed;
    protected float attackSpeed;
    protected float attackRange;
    protected float detectionRadius = 0.02f;
    protected float currentSpeed;
    protected Vector2 moveDir;
    protected bool isFacingLeft;
    protected float countAttackTime;

    

    protected void MoveToGameObject(GameObject _gameObject)
    {
        transform.LookAt(new Vector3(_gameObject.transform.position.x, 0, _gameObject.transform.position.z));
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(_gameObject.transform.position.x, 0, _gameObject.transform.position.z), 1f * Time.deltaTime);
    }

    protected void TakeDamage()
    {
        
    }

    protected void Deadth()
    {
        gameObject.SetActive(false);
        PoolingEnemy.Instance.BackToPool(this);
    }
    
    public virtual void Born(Vector3 _position)
    {
        transform.position = _position;
        //countAttackTime = 1 / attackSpeed;
        //currentState = moveState;

        gameObject.SetActive(true);
        //hpManager.HealFullHp();
        //currentState.EnterState(this);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Deadth();
        }
    }
}
