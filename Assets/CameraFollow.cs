using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float cameraDistance = 1f;
    public float cameraHeight = 1f;
    public float rotationSpeed = 5f;

    private Vector3 offset;

    void Start()
    {
        offset = new Vector3(0f, cameraHeight, -cameraDistance);
    }

    void LateUpdate()
    {
        transform.position = player.position + offset;

        transform.LookAt(player.position);

        float rotationInput = Input.GetAxis("Horizontal");
        player.Rotate(Vector3.up * rotationInput * rotationSpeed);
    }
}
