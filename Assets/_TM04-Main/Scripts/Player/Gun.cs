using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class Gun : MonoBehaviour
{
    [SerializeField]
    private bool AddBulletSpread = true;
    [SerializeField]
    private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField]
    private ParticleSystem ShootingSystem;
    [SerializeField]
    private Transform BulletSpawnPoint;
    [SerializeField]
    private ParticleSystem ImpactParticleSystem;
    [SerializeField]
    private TrailRenderer BulletTrail;
    [SerializeField]
    private float ShootDelay = 0.5f;
    [SerializeField]
    private LayerMask MaskEnemy;
    //[SerializeField]
    //private LayerMask MaskEnvironment;

    [SerializeField]
    private float BulletSpeed = 100;

    [SerializeField] private int _damage;

    [SerializeField] private StarterAssetsInputs _inputs;

    private Animator Animator;
    private float LastShootTime;

    public enum ImpactType
    {
        EnemyImpact,
        EnvironmentImpact,
        NoneImpact,
    }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_inputs.lookTopDown != Vector2.zero)
        {   
            ShootingSystem.Play(true);
            Shoot();
        }
        else
        {
            ShootingSystem.Pause();
            ShootingSystem.Clear();
        }
    }

    public void Shoot()
    {
        if (LastShootTime + ShootDelay < Time.time)
        {
            // Use an object pool instead for these! To keep this tutorial focused, we'll skip implementing one.
            // For more details you can see: https://youtu.be/fsDE_mO4RZM or if using Unity 2021+: https://youtu.be/zyzqA_CPz2E

            Animator.SetBool("IsShooting", true);
            Vector3 direction = GetDirection();

            if (Physics.Raycast(BulletSpawnPoint.position, direction, out RaycastHit hit, float.MaxValue, MaskEnemy))
            {
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit.point,  hit,  hit.normal, true));

                LastShootTime = Time.time;
            }
            // this has been updated to fix a commonly reported problem that you cannot fire if you would not hit anything
            else
            {
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, BulletSpawnPoint.position + GetDirection() * 100, hit, Vector3.zero, false));

                LastShootTime = Time.time;
            }
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = BulletSpawnPoint.transform.forward;

        if (AddBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
            );

            direction.Normalize();
        }

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint,  RaycastHit hit, Vector3 HitNormal, bool MadeImpact)
    {
        // This has been updated from the video implementation to fix a commonly raised issue about the bullet trails
        // moving slowly when hitting something close, and not
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= BulletSpeed * Time.deltaTime;

            yield return null;
        }
        Animator.SetBool("IsShooting", false);
        Trail.transform.position = HitPoint;
        if (MadeImpact)
        {
            // if (type == ImpactType.EnemyImpact)
            // {
                Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
            //}
            // else if (type == )
            // {
            //     
            // }
            Zombie1 enemy = hit.collider.GetComponent<Zombie1>();
            if (enemy != null)
            {
                //Debug.Log(_damage);
                enemy.TakeDamage(_damage);
            }
        }

        Destroy(Trail.gameObject, Trail.time);
    }
}
