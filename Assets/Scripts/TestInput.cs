using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour
{
    [SerializeField] InputReader _input;

    
    void Start()
    {
        _input.MoveEvent += HandleMove;
        _input.PrimaryFireEvent += HandlePrimaryFire;
    }


    void HandleMove(Vector2 move)
    {
        Debug.Log(move);
    }

    void HandlePrimaryFire(bool isFiring)
    {
        if(isFiring)
        {
            Debug.Log("Firing");
        }
        else
        {
            Debug.Log("Not Firing");
        }
    }

    void OnDestroy()
    {
        _input.MoveEvent -= HandleMove;
        _input.PrimaryFireEvent -= HandlePrimaryFire;
    }
}
