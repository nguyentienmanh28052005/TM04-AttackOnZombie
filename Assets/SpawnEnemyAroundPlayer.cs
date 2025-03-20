using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyAroundPlayer : MonoBehaviour
{
    private GameObject _player;
    private float _cooldown = 0.2f;
    private float _time = 0;

    private float _radius = 30f;
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        while (_time > _cooldown)
        {
            _time = 0;
            if(LevelManager._countEnemy < 100f) PoolingEnemy.Instance.SpawnEnemy(Define.ZOMBIE1, GetRandomPositionAroundCenter());
        }
    }
    
    
    private Vector3 GetRandomPositionAroundCenter()
    {
        Vector3 center = _player.transform.position;
        float randomAngle = Random.Range(0f, Mathf.PI * 2);
        float x = center.x + Mathf.Cos(randomAngle) * _radius;
        float y = center.z + Mathf.Sin(randomAngle) * _radius;

        return new Vector3(x, 0, y);
    }
}
