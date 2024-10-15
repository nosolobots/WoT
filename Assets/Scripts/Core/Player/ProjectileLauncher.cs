using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] InputReader inputReader;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] GameObject serverProjectilePrefab;
    [SerializeField] GameObject clientProjectilePrefab;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] Collider2D playerCollider;

    [Header("Settings")]
    [SerializeField] float projectileSpeed;
    [SerializeField] float fireRate;
    [SerializeField] float muzzleFlashDuration;
    private bool shouldFire;
    
    private float previousFireTime;
    private float muzzleFlashTimer;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        inputReader.PrimaryFireEvent += HandlePrimaryFire;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        inputReader.PrimaryFireEvent -= HandlePrimaryFire;
    }

    void Update()
    {
        if (muzzleFlashTimer > 0f)
        {
            muzzleFlashTimer -= Time.deltaTime;
            if (muzzleFlashTimer <= 0f)
            {
                muzzleFlash.SetActive(false);
            }
        }

        if (!IsOwner || !shouldFire) return;

        if (Time.time < (1 / fireRate) + previousFireTime)
        {
            return;
        }

        // Mensaje al servidor para que dispare e informe a los clientes
        PrimaryFireServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.up);

        // Disparo local
        SpawnProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);

        previousFireTime = Time.time;
    }

    private void HandlePrimaryFire(bool shouldFire)
    {
        this.shouldFire = shouldFire;
    }

    [ServerRpc]
    private void PrimaryFireServerRpc(Vector3 position, Vector3 direction)
    {
        GameObject projectile = Instantiate(
            serverProjectilePrefab,
            position,
            Quaternion.identity
        );

        projectile.transform.up = direction;

        // Ignorar colisión con el jugador
        Physics2D.IgnoreCollision(playerCollider, projectile.GetComponent<Collider2D>());

        // Aplicar velocidad al proyectil
        if (projectile.TryGetComponent(out Rigidbody2D rb))
        {
            rb.velocity = direction * projectileSpeed;
        }

        // Mensaje a los clientes para que disparen
        PrimaryFireClientRpc(position, direction);
    }

    [ClientRpc]
    private void PrimaryFireClientRpc(Vector3 position, Vector3 direction)
    {
        if (IsOwner) return;

        SpawnProjectile(position, direction);
    }

    private void SpawnProjectile(Vector3 position, Vector3 direction)
    {
        muzzleFlash.SetActive(true);
        muzzleFlashTimer = muzzleFlashDuration;

        GameObject projectile = Instantiate(
            clientProjectilePrefab,
            position,
            Quaternion.identity
        );

        projectile.transform.up = direction;

        // Ignorar colisión con el jugador
        Physics2D.IgnoreCollision(playerCollider, projectile.GetComponent<Collider2D>());

        // Aplicar velocidad al proyectil
        if (projectile.TryGetComponent(out Rigidbody2D rb))
        {
            rb.velocity = direction * projectileSpeed;
        }
    }
}
