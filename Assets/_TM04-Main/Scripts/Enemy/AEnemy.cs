using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEnemy : AObserver
{
    protected Animator _animator;
    protected GameObject _player;


    protected void MoveToGameObject(GameObject _gameObject)
    {
        transform.LookAt(new Vector3(_gameObject.transform.position.x, 0, _gameObject.transform.position.z));
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(_gameObject.transform.position.x, 0, _gameObject.transform.position.z), 1f * Time.deltaTime);
    }

    protected void TakeDamage()
    {
        
    }
}
