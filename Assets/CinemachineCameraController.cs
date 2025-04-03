using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraController : MonoBehaviour
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
           GameObject networkObject = player.GetComponent<GameObject>();
            
            if (networkObject != null)
            {
                _cinemachineVirtual.Follow = networkObject.transform;
                return;
            }
        }

        Debug.LogWarning("No local player found!");
    }
}
