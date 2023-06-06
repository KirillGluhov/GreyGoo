using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 0.5f;
    public float jumpForce = 5f;
    private bool isJumping = false;
    private Rigidbody playerRigidbody;
    private Camera mainCamera;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0;
        cameraForward = cameraForward.normalized;

 
        Vector3 movement = (cameraForward * moveVertical + mainCamera.transform.right * moveHorizontal) * speed;

        playerRigidbody.AddForce(movement);

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}