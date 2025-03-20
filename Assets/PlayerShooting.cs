using System;
using StarterAssets;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public float rayDistance = 20f; // Độ dài tia raycast
    public float damage = 10f; // Sát thương gây ra
    public float shootCooldown = 0.5f; // Thời gian chờ giữa các lần bắn
    private float nextShootTime; // Thời điểm có thể bắn tiếp theo
    private StarterAssetsInputs _inputs;

    public void Start()
    {
        _inputs = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        // Kiểm tra nếu nhấn phím bắn (ví dụ: chuột trái) và đã qua thời gian cooldown
        if (_inputs.jump && Time.time >= nextShootTime)
        {
            ShootRaycast();
            nextShootTime = Time.time + shootCooldown; // Cập nhật thời gian cooldown
        }
    }

    void ShootRaycast()
    {
        // Điểm bắt đầu của tia là vị trí người chơi
        Vector3 rayOrigin = new Vector3(transform.position.x, 0.85f, transform.position.z);
        // Hướng của tia là transform.forward
        Vector3 rayDirection = transform.forward;

        RaycastHit hit;
        // Bắn tia raycast
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance))
        {
            // Kiểm tra nếu tia trúng enemy
            Zombie1 enemy = hit.collider.GetComponent<Zombie1>();
            if (enemy != null)
            {
                enemy.Deadth(); // Gây sát thương cho enemy
            }
        }

        // Vẽ tia trong Scene view để debug
        //Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red, 0.5f);
    }

    // Hiển thị phạm vi tia trong Editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Vector3(transform.position.x, 0.85f, transform.position.z), transform.forward * rayDistance);
    }
}