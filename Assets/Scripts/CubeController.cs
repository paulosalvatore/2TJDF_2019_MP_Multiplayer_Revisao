using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeController : NetworkBehaviour
{
    [SyncVar] public int ownerId;

    private void Start()
    {
        var meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = ownerId == 1 ? Color.red : Color.blue;
    }
}