using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSplitScreenController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotateSpeed = 150f;

    // Default value is 1. We have to change that on Unity for player 2
    [Range(1, 2)]
    public int playerId = 1;

    // Update is called once per frame
    private void Update()
    {
        var axisVertical = playerId == 1 ? "Vertical" : "Vertical2";
        var axisHorizontal = playerId == 1 ? "Horizontal" : "Horizontal2";

        // Get axis values and multiply by public speed values
        var v = Input.GetAxis(axisVertical) * moveSpeed;
        var h = Input.GetAxis(axisHorizontal) * rotateSpeed;

        // Move with 'Translate'
        transform.Translate(Vector3.forward * v * Time.deltaTime);

        // Spin with 'Rotate'
        transform.Rotate(Vector3.up * h * Time.deltaTime);
    }
}