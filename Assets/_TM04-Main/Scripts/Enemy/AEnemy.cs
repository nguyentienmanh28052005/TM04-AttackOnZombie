using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEnemy : AObserver
{
    protected Animator _animator;
    protected GameObject _player;
    public int health;
    private int _maxHealth;
    [SerializeField] private GameObject _visual;
    
    
    protected virtual void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _maxHealth = health;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        MoveToGameObject(_player);
    }
    

    protected void MoveToGameObject(GameObject _gameObject)
    {
        Quaternion currentRotation = transform.rotation;
                    Quaternion targetRotation = Quaternion.LookRotation(_gameObject.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 1f * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(_gameObject.transform.position.x, 0, _gameObject.transform.position.z), 2f * Time.deltaTime);
    }

    public void TakeDamage()
    {
        health -= 5;
        if(health < 0) Deadth();
    }

    public void Deadth()
    {
        health = _maxHealth;
        gameObject.SetActive(false);
        PoolingEnemy.Instance.BackToPool(this);
    }
    
    public virtual void Born(Vector3 _position)
    {
        transform.position = _position;
        gameObject.SetActive(true);
        // transform.position = _position;
        // //countAttackTime = 1 / attackSpeed;
        // //currentState = moveState;
        //
        // gameObject.SetActive(true);
        // //hpManager.HealFullHp();
        // //currentState.EnterState(this);
    }

    public void OnTriggerEnter(Collider other)
    {
        // if (!IsServer) return;
        // if (other.CompareTag("Bullet"))
        // {
        //     TakeDamageServerRpc();
        // }

        // if (other.CompareTag("RangeCamera"))
        // {
        //     _visual.SetActive(true);
        // }
    }

    public void OnTriggerExit(Collider other)
    {
        // if (other.CompareTag("RangeCamera"))
        // {
        //     _visual.SetActive(false);
        // }
    }
    
    
    
}
