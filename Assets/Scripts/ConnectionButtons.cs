using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ConnectionButtons : MonoBehaviour
{
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void StartHost()
    {
        Debug.Log("Starting Host");
        NetworkManager.Singleton.StartHost();
    }
}
