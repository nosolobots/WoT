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

    [Header("Settings")]
    [SerializeField] float projectileSpeed;

    private bool shouldFire;

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
        if (!IsOwner || !shouldFire) return;

        // Mensaje al servidor para que dispare e informe a los clientes
        PrimaryFireServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.up);

        // Disparo local
        SpawnProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);
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
        GameObject projectile = Instantiate(
            clientProjectilePrefab,
            position,
            Quaternion.identity
        );

        projectile.transform.up = direction;
    }
}
