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
        // Перемещение камеры за игроком
        transform.position = player.position + offset;

        // Поворот камеры в соответствии с поворотом игрока
        transform.LookAt(player.position);

        // Отслеживание ввода для поворота игрока
        float rotationInput = Input.GetAxis("Horizontal");
        player.Rotate(Vector3.up * rotationInput * rotationSpeed);
    }
}
