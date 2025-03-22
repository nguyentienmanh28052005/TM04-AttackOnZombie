using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class CinemachineCameraController : NetworkBehaviour
{

    [SerializeField] private CinemachineVirtualCameraBase _cinemachineVirtual;

    private GameObject[] _gameObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
           
        }
    }

    public void Add()
    {
        FindLocalPlayer();
    }
    
    void FindLocalPlayer()
    {
        // Lấy tất cả GameObject có tag "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            // Lấy NetworkObject từ GameObject
            NetworkObject networkObject = player.GetComponent<NetworkObject>();
            
            if (networkObject != null && networkObject.IsOwner)
            {
                _cinemachineVirtual.Follow = networkObject.transform;
                return;
            }
        }

        Debug.LogWarning("No local player found!");
    }
}
