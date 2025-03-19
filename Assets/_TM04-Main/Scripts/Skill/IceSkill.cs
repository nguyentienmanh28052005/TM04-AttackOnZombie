using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IceSkill : ASkill
{
    public void OnEnable()
    {
        Handler();
        StartCoroutine(CoroutineSkill());
    }

    protected override void Handler()
    {
        Collider[] _objectHit = Physics.OverlapSphere(transform.position, 30f, LayerMask.GetMask("Enemy"));
            
            transform.position = _objectHit[Random.Range(0, _objectHit.Length)].transform.position;
    }
}
