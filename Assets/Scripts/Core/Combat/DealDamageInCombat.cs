using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class DealDamageInCombat : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private ulong ownerClientId;
    
    public void SetOwner(ulong ownerClientId)
    {
        this.ownerClientId = ownerClientId;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody == null) return;

        // If the owner is the same as the one that dealt the damage, return
        if(other.attachedRigidbody.TryGetComponent(out NetworkObject networkObject))
        {
            if (networkObject.OwnerClientId == ownerClientId) return;
        }

        if (other.attachedRigidbody.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
        }
    }
}
