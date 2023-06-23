using UnityEngine;

public class AnimalNPC : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float avoidanceRadius = 5f;
    public float fleeRadius = 10f;
    public float minFleeDuration = 2f;
    public float maxFleeDuration = 5f;

    private Transform player;
    private bool isFleeing = false;
    private float fleeTimer = 0f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isFleeing)
        {
            FleeFromPlayer();
        }
        else
        {
            CheckPlayerDistance();
        }
    }

    private void CheckPlayerDistance()
    {
        Vector3 direction = transform.position - player.position;
        float distance = direction.magnitude;

        if (distance < fleeRadius)
        {
            StartFleeing();
        }
    }

    private void StartFleeing()
    {
        isFleeing = true;
        fleeTimer = Random.Range(minFleeDuration, maxFleeDuration);
    }

    private void FleeFromPlayer()
    {
        fleeTimer -= Time.deltaTime;

        if (fleeTimer <= 0f)
        {
            isFleeing = false;
        }
        else
        {
            Vector3 direction = transform.position - player.position;
            Vector3 moveDirection = direction.normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            transform.Rotate(Vector3.up, 180f);
        }
    }
}
