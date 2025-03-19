using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSkillController : MonoBehaviour
{
    public float _coolDown;
    [SerializeField] private GameObject _iceSkillPrefab;
    private bool _canAttack = true;

    public void Update()
    {
        Collider[] _objectHit = Physics.OverlapSphere(transform.position, 30f, LayerMask.GetMask("Enemy"));
        if (_canAttack && _objectHit.Length > 0)
        {
            StartCoroutine(CoolDown());
            PoolingSkill.Instance.SpawnSkill(Define.SKILL_ICE, transform.position);
        }
    }

    public IEnumerator CoolDown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_coolDown);
        _canAttack = true;
    }
}
