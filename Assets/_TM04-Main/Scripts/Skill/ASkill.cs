using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASkill : MonoBehaviour
{
    public float _timeExist = 2f;
    public bool _canAttack = true;

    protected virtual void Handler()
    {
        
    }

    public void Born(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void Death()
    {
        gameObject.SetActive(false);
        PoolingSkill.Instance.BackToPool(this);
    }
    
    protected IEnumerator CoroutineSkill()
    {
        yield return new WaitForSeconds(_timeExist);
        Death();
    }
}
