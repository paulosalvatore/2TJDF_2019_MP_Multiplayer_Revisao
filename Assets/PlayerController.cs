using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    [SyncVar] private int playerId = 0;

    private static int playerIdAvailable = 0;

    public float moveSpeed = 2f;
    public float rotateSpeed = 150f;

    private void Start()
    {
        LoadPlayerId();

        LoadMaterial();
    }

    private void LoadPlayerId()
    {
        // Only Load Player Id if is server
        if (isServer)
        {
            if (hasAuthority)
            {
                // Reset playerIdAvailable if current player
                // has authority. That means we're on host player
                playerIdAvailable = 0;
            }

            // Change playerIdAvailable for all players
            playerIdAvailable++;

            // Change current player's ID variable
            playerId = playerIdAvailable;
        }
    }

    private void LoadMaterial()
    {
        // Get Mesh Renderer Component
        var meshRenderer = GetComponent<MeshRenderer>();

        // Apply on mainMaterial a color based on playerId
        // If playerId is equal to, we're on host, and set color red
        // Otherwhise, we set color blue, no matter playerId
        meshRenderer.material.color = playerId == 1 ? Color.red : Color.blue;
    }

    private void Update()
    {
        MovePlayer();

        if (hasAuthority && Input.GetKeyDown(KeyCode.Space))
        {
            CmdSpawnCube();
        }
    }

    private void MovePlayer()
    {
        // Only move player if we are on player that we have authority
        if (hasAuthority)
        {
            // Get axis values and multiply by public speed values
            var v = Input.GetAxis("Vertical") * moveSpeed;
            var h = Input.GetAxis("Horizontal") * rotateSpeed;

            // Move with 'Translate'
            transform.Translate(Vector3.forward * v * Time.deltaTime);

            // Spin with 'Rotate'
            transform.Rotate(Vector3.up * h * Time.deltaTime);
        }
    }

    public GameObject cubePrefab;

    [Command]
    private void CmdSpawnCube()
    {
        // Instantiate 'cubePrefab' and hold it into 'cube' variable
        var cube = Instantiate(cubePrefab);

        // Change position of created cube to be in front of player owner
        //  transform.position: Represent player's position
        //  transform.forward: Represent player's "front" with magnitude 1
        //  transform.forward * 2: Change magninude of player's forward by 2
        //      That means cube will be positioned 2 units away from the player
        //  All this math will be applied to cube's position
        cube.transform.position = transform.position + transform.forward * 2;

        // Change rotation of created cube, to match player's rotation
        cube.transform.rotation = transform.rotation;

        // Since cube is only instantiated on server because of [Command] annotation,
        //  we have to spawn it on entire server, so all players can see this new cube
        // NOTE: This will only work if cube is registered as a 'SpawnablePrefab'
        //  on NetworkManager
        NetworkServer.Spawn(cube);
    }
}