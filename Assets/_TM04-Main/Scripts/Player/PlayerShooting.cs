using System;
using StarterAssets;
using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public float rayDistance = 20f;
    public float damage = 10f; 
    public float shootCooldown = 0.005f; 
    private float nextShootTime; 
    private StarterAssetsInputs _inputs;

    [SerializeField] private GameObject _startPosLaze;

    public void Start()
    {
        _inputs = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        if (Time.time >= nextShootTime)
        {
            ShootRaycast();
            nextShootTime = Time.time + shootCooldown; 
        }
    }
    
    void ShootRaycast()
    {
        Vector3 rayOrigin = new Vector3(_startPosLaze.transform.position.x, _startPosLaze.transform.position.y, _startPosLaze.transform.position.z);
        Vector3 rayDirection = _startPosLaze.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance))
        {
            Zombie1 enemy = hit.collider.GetComponent<Zombie1>();
            if (enemy != null)
            {
                enemy.Deadth();
            }
        }
        
        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red, 0.5f);
    }

    // Hiển thị phạm vi tia trong Editor
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawRay(new Vector3(transform.position.x, 0.85f, transform.position.z), transform.forward * rayDistance);
    // }
    
    
    
}