using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSkill : ASkill
{
    private int res = 0;
    [SerializeField] private GameObject _gameObject;
    public void Update()
    {
        Handler();
    }
    
    public void Handler()
    {
        Collider[] _objectHit = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Enemy"));
        if (_objectHit.Length > 0 && res < 1)
        {
            res++;
            Instantiate(_gameObject, _objectHit[0].transform.position, Quaternion.identity);
        }
            
    }

}
