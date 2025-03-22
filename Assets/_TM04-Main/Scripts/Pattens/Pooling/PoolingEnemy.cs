// using System.Collections;
// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEngine;
// using Unity.Netcode;
//
// public class PoolingEnemy : Singleton<PoolingEnemy>
// {
//     [SerializeField] private Zombie1 _zombie1Prefab;
//
//     private Queue<Zombie1> _zombie1Queue = new Queue<Zombie1>();
//     
//     protected override void Awake()
//     {
//         base.Awake();
//     }
//     
//     public void SpawnEnemy(string _nameEnemy, Vector3 _position)
//     {
//         if (!NetworkManager.IsServer) return;
//         LevelManager._countEnemy++;
//         if (_nameEnemy == Define.ZOMBIE1)
//         {
//             if (_zombie1Queue.Count == 0)
//             {
//                 Zombie1 _newZombie1 = Instantiate(_zombie1Prefab, _position, Quaternion.identity);
//                 _zombie1Queue.Enqueue(_newZombie1);
//             }
//             _zombie1Queue.Dequeue().Born(_position);
//             NetworkObject _networkObject;
//         }
//     }
//
//     public void BackToPool(AEnemy _enemy)
//     {
//         switch (_enemy)
//         {
//             case Zombie1 _zombie1:
//                 LevelManager._countEnemy--;
//                 _zombie1Queue.Enqueue(_zombie1);
//                 break;
//         }
//     }
// }

// using System.Collections.Generic;
// using Unity.Netcode;
// using UnityEngine;
//
// public class PoolingEnemy : Singleton<PoolingEnemy>
// {
//     [SerializeField] private Zombie1 _zombie1Prefab;
//     private Queue<Zombie1> _zombie1Queue = new Queue<Zombie1>();
//
//     protected override void Awake()
//     {
//         base.Awake();
//     }
//
//     // Spawn enemy từ server và đồng bộ qua mạng
//     public Zombie1 SpawnEnemy(string _nameEnemy, Vector3 _position)
//     {
//         if (!NetworkManager.Singleton.IsServer) return null; // Chỉ server được spawn
//
//         Zombie1 enemy = null;
//         if (_nameEnemy == Define.ZOMBIE1)
//         {
//             if (_zombie1Queue.Count == 0)
//             {
//                 // Tạo mới enemy nếu queue trống
//                 enemy = Instantiate(_zombie1Prefab, _position, Quaternion.identity);
//             }
//             else
//             {
//                 // Lấy từ queue
//                 enemy = _zombie1Queue.Dequeue();
//             }
//
//             // Đặt vị trí và spawn qua mạng
//             enemy.transform.position = _position;
//             NetworkObject networkObject = enemy.GetComponent<NetworkObject>();
//             if (networkObject != null)
//             {
//                 networkObject.Spawn(); // Spawn qua mạng
//                 int _newVal = LevelManager._countEnemy.Value+1;
//                 UpdateCountServerRpc(_newVal);
//                 enemy.Born(_position); // Kích hoạt enemy
//             }
//             else
//             {
//                 Debug.LogError("Zombie1 prefab không có NetworkObject component!");
//             }
//         }
//         return enemy;
//     }
//
//     // Đưa enemy trở lại pool khi không dùng
//     public void BackToPool(AEnemy _enemy)
//     {
//         if (!NetworkManager.Singleton.IsServer) return; // Chỉ server xử lý
//
//         switch (_enemy)
//         {
//             case Zombie1 _zombie1:
//                 NetworkObject networkObject = _zombie1.GetComponent<NetworkObject>();
//                 if (networkObject != null)
//                 {
//                     networkObject.Despawn(false); // Despawn nhưng không destroy
//                     _zombie1Queue.Enqueue(_zombie1);
//                     int _newVal = LevelManager._countEnemy.Value-1;
//                     UpdateCountServerRpc(_newVal);
//                     _zombie1.gameObject.SetActive(false); // Tắt enemy
//                 }
//                 break;
//         }
//     }
//     
//     [ServerRpc]
//     void UpdateCountServerRpc(int newValue)
//     {
//         LevelManager._countEnemy.Value = newValue;
//     }
// }

using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PoolingEnemy : Singleton<PoolingEnemy>
{
    [SerializeField] private Zombie1 _zombie1Prefab;
    private Queue<Zombie1> _zombie1Queue = new Queue<Zombie1>();

    protected override void Awake()
    {
        base.Awake();
    }

    public Zombie1 SpawnEnemy(string _nameEnemy, Vector3 _position)
    {
        if (!NetworkManager.Singleton.IsServer) return null; // Chỉ server spawn

        Zombie1 enemy;
        NetworkObject networkObject;

        if (_nameEnemy == Define.ZOMBIE1)
        {
            if (_zombie1Queue.Count == 0)
            {
                // Tạo mới nếu pool trống
                enemy = Instantiate(_zombie1Prefab, _position, Quaternion.identity);
                networkObject = enemy.GetComponent<NetworkObject>();
                networkObject.Spawn(); // Spawn lần đầu
            }
            else
            {
                // Lấy từ pool
                enemy = _zombie1Queue.Dequeue();
                networkObject = enemy.GetComponent<NetworkObject>();

                // Đặt lại vị trí và spawn lại
                enemy.transform.position = _position;
                enemy.gameObject.SetActive(true); // Kích hoạt lại
                networkObject.Spawn(); // Spawn lại để đồng bộ
            }

            LevelManager._countEnemy.Value++;
            enemy.Born(_position); // Kích hoạt logic enemy
            return enemy;
        }
        return null;
    }

    public void BackToPool(AEnemy _enemy)
    {
        if (!NetworkManager.Singleton.IsServer) return; // Chỉ server xử lý

        if (_enemy is Zombie1 _zombie1)
        {
            NetworkObject networkObject = _zombie1.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                networkObject.Despawn(false); // Despawn nhưng không destroy
                _zombie1.gameObject.SetActive(false); // Tắt enemy
                _zombie1Queue.Enqueue(_zombie1);
                LevelManager._countEnemy.Value--;
            }
        }
    }
}