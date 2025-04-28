using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateManager))]
public abstract class AEnemy : AObserver
{
    //private Rigidbody _rb;
    protected Collider _collider;
    protected StateManager _stateManager;
    protected CharacterController _controller;
    [SerializeField] protected GameObject _player;
    [SerializeField] protected GameObject _visual;
    [SerializeField] protected int _maxHealth;
    [SerializeField] protected bool _canAttack = true;
    [SerializeField] protected bool _atOriginState = true;
    public Animator _animator;
    public int health;
    public float speed = 2;
    public float originSpeed;
    public float angrySpeed;
    public float currentSpeed;
    
    
    
    protected virtual void Start()
    {
        //_rb = GetComponent<Rigidbody>();
        _stateManager = GetComponent<StateManager>();
        _controller = GetComponent<CharacterController>();
        _collider = GetComponentInChildren<Collider>();
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponentInChildren<Animator>();
        _maxHealth = health;
        originSpeed = speed;
        currentSpeed = speed;
    }

    // Update is called once per frame
    
    protected void MoveToGameObject2(GameObject _gameObject)
    {
        // Quaternion currentRotation = transform.rotation;
        //             Quaternion targetRotation = Quaternion.LookRotation(_gameObject.transform.position - transform.position);
        //             transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 1f * Time.deltaTime);
        transform.LookAt(_player.transform);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(_gameObject.transform.position.x, transform.position.y, _gameObject.transform.position.z), speed  * Time.deltaTime);
    }
    
    public void MoveToGameObject(GameObject _gameObject)
    {
        transform.LookAt(_player.transform);
        Vector3 targetDirection = _gameObject.transform.position - transform.position;
        _controller.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, -1, 0.0f) * Time.deltaTime);
    }
    
    public void MoveToPlayer()
    {
        MoveToGameObject(_player);
    }
    
    public virtual void TakeDamage()
    {
        health -= 5;
        if(health < 0) Deadth();
    }

    public virtual void SetAngryState()
    {
        if(_canAttack)
        {
            speed = angrySpeed;
            currentSpeed = speed;
        }
        if (_atOriginState)
        {
            _animator.SetBool("Angry",  true);
            _atOriginState = false;
        }
    }

    public void Deadth()
    {
        SetOrigin();
        PoolingEnemy.Instance.BackToPool(this);
    }

    public virtual void SetOrigin()
    {
        if (!_canAttack)
        {
            _animator.SetBool("Attack", false);
            speed = currentSpeed;
            _canAttack = true;
        }
        health = _maxHealth;
        speed = originSpeed;
        currentSpeed = speed;
        gameObject.SetActive(false);
        _animator.SetBool("Angry",  false);
        _atOriginState = true;
    }

    public void SetIdleState()
    {
        if (!_canAttack)
        {
            _animator.SetBool("Attack", false);
            speed = currentSpeed;
            _canAttack = true;
        }
        speed = originSpeed;
        currentSpeed = speed;
        _animator.SetBool("Angry",  false);
        _atOriginState = true;
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

    protected virtual IEnumerator Attack()
    {
        //_rb.velocity = Vector3.zero;
        _canAttack = false;
        speed = 0;
        _animator.SetBool("Attack", true);
        yield return new WaitForSeconds(1f);
        _animator.SetBool("Attack", false);
        speed = currentSpeed;
        _canAttack = true;
    }
    
}
