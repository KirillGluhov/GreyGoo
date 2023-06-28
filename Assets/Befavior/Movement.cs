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
    private CharacterController controller;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        controller = GetComponent<CharacterController>();
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
        isJumping = false;
    }
    private void CollectBlock()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Chunk"))
            {

                Destroy(hit.collider.gameObject); 
            }
        }
    }
}