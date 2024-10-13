using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2.0f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
