using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie1 : AEnemy
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _hitAttack;
    
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //if (!_canAttack) _rb.velocity = Vector3.zero;
        if ((Vector3.Distance(transform.position, _player.transform.position) < 1.5f) && _canAttack)
        {
            StartCoroutine(Attack());
            StartCoroutine(HitAttack());
        }
        //MoveToGameObject(_player);
        if (Vector3.Distance(transform.position, _player.transform.position) < 5f)
        {
            // SetAngryState();
            _stateManager.ChangeState(new Zombie1_AngryState(this, _animator));
        }
        
        if (Vector3.Distance(transform.position, _player.transform.position) > 12f)
        {
            // SetAngryState();
            _stateManager.ChangeState(new Zombie1_IdleState(this, _animator));
        }
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        _stateManager.ChangeState(new Zombie1_AngryState(this, _animator));
    }

    private IEnumerator HitAttack()
    {
        yield return new WaitForSeconds(0.6f);
        _hitAttack.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        _hitAttack.SetActive(false);

    }
}
