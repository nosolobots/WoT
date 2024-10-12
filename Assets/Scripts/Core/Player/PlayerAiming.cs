using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAiming : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform turretTransform;

    private void LateUpdate()
    {
        if (!IsOwner) return;

        Vector2 aimPos = Camera.main.ScreenToWorldPoint(inputReader.AimPosition);

        Vector2 lookDir = aimPos - (Vector2) turretTransform.position;
        turretTransform.up = lookDir;
    }
}
