using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolingEnemy : Singleton<PoolingEnemy>
{
    [SerializeField] private Zombie1 _zombie1Prefab;

    private Queue<Zombie1> _zombie1Queue = new Queue<Zombie1>();
    
    protected override void Awake()
    {
        base.Awake();
    }
    
    public void SpawnEnemy(string _nameEnemy, Vector3 _position)
    {
        if (_nameEnemy == Define.ZOMBIE1)
        {
            if (_zombie1Queue.Count == 0)
            {
                Zombie1 _newZombie1 = Instantiate(_zombie1Prefab, _position, Quaternion.identity);
                _zombie1Queue.Enqueue(_newZombie1);
            }
            _zombie1Queue.Dequeue().Born(_position);
        }
    }

    public void BackToPool(AEnemy _enemy)
    {
        switch (_enemy)
        {
            case Zombie1 _zombie1:
                _zombie1Queue.Enqueue(_zombie1);
                break;
        }
    }
}
