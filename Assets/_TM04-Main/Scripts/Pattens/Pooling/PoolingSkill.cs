using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingSkill : Singleton<PoolingSkill>
{
    [SerializeField] private IceSkill _iceSkillPrefab;


    private Queue<IceSkill> _iceSkillQueue = new Queue<IceSkill>();

    protected override void Awake()
    {
        base.Awake();
    }
    
    public void SpawnSkill(string nameSkill, Vector3 position)
    {
        if (nameSkill == Define.SKILL_ICE)
        {
            if (_iceSkillQueue.Count == 0)
            {
                IceSkill _iceSkill = Instantiate(_iceSkillPrefab, position, Quaternion.identity);
                _iceSkillQueue.Enqueue(_iceSkill);
            }
            _iceSkillQueue.Dequeue().Born(position);
        }
    }

    public void BackToPool(ASkill _skill)
    {
        switch (_skill)
        {
            case IceSkill _iceSkill:
                _iceSkillQueue.Enqueue(_iceSkill);
                break;
        }
    }
}
