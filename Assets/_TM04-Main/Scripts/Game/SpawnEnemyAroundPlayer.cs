// using System.Collections;
// using System.Collections.Generic;
// using Unity.Netcode;
// using UnityEngine;
//
// public class SpawnEnemyAroundPlayer : Singleton<SpawnEnemyAroundPlayer>
// {
//     public GameObject _home;
//     private float _cooldown = 0.2f;
//     private float _time = 0;
//
//     private float _radius = 30f;
//     void Start()
//     {
//         //_player = GameObject.FindGameObjectWithTag("Home");
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         _time += Time.deltaTime;
//         while (_time > _cooldown)
//         {
//             _time = 0;
//             if(LevelManager._countEnemy < 1 && IsOwner) SpawnServerRpc();
//         }
//     }
//     
//     
//     private Vector3 GetRandomPositionAroundCenter()
//     {
//         Vector3 center = _home.transform.position;
//         float randomAngle = Random.Range(0f, Mathf.PI * 2);
//         float x = center.x + Mathf.Cos(randomAngle) * _radius;
//         float y = center.z + Mathf.Sin(randomAngle) * _radius;
//
//         return new Vector3(x, 0, y);
//     }
//
//     [ServerRpc(RequireOwnership = false)]
//     public void SpawnServerRpc()
//     {
//         NotifyClientRpc();
//     }
//
//     [ClientRpc]
//     public void NotifyClientRpc()
//     {
//         PoolingEnemy.Instance.SpawnEnemy(Define.ZOMBIE1, GetRandomPositionAroundCenter());
//         Debug.Log("hi");
//     }
//     
// }
using UnityEngine;

public class SpawnEnemyAroundPlayer : Singleton<SpawnEnemyAroundPlayer>
{
    public GameObject _home;
    public float _cooldown = 0.2f;
    private float _time = 0;
    private float _radius = 30f;

    void Update()
    {
        _time += Time.deltaTime;
        while (_time > _cooldown)
        {
            _time = 0;
            Spawn();
        }
    }

    private Vector3 GetRandomPositionAroundCenter()
    {
        Vector3 center = _home.transform.position;
        float randomAngle = Random.Range(0f, Mathf.PI * 2);
        float x = center.x + Mathf.Cos(randomAngle) * _radius;
        float y = center.z + Mathf.Sin(randomAngle) * _radius;
        return new Vector3(x, 0, y);
    }

    private void Spawn()
    {
        if(LevelManager.atLevel == 0) PoolingEnemy.Instance.SpawnEnemy(Define.ZOMBIE1, GetRandomPositionAroundCenter());
        if (LevelManager.atLevel == 1)
        {
            PoolingEnemy.Instance.SpawnEnemy(Define.ZOMBIE1, GetRandomPositionAroundCenter());
            PoolingEnemy.Instance.SpawnEnemy(Define.ZOMBIE1, GetRandomPositionAroundCenter());
        }

        if (LevelManager.atLevel == 2 || LevelManager.atLevel == 3)
        {
            PoolingEnemy.Instance.SpawnEnemy(Define.ZOMBIE1, GetRandomPositionAroundCenter());
            PoolingEnemy.Instance.SpawnEnemy(Define.ZOMBIE1, GetRandomPositionAroundCenter());
            PoolingEnemy.Instance.SpawnEnemy(Define.ZOMBIE1, GetRandomPositionAroundCenter());
        }

        if (LevelManager.atLevel == 4)
        {
            PoolingEnemy.Instance.SpawnEnemy(Define.ZOMBIE1, GetRandomPositionAroundCenter());
            PoolingEnemy.Instance.SpawnEnemy(Define.ZOMBIE1, GetRandomPositionAroundCenter());
            PoolingEnemy.Instance.SpawnEnemy(Define.ZOMBIE1, GetRandomPositionAroundCenter());
            PoolingEnemy.Instance.SpawnEnemy(Define.ZOMBIE1, GetRandomPositionAroundCenter());
        }
        
    }

}