using UnityEngine;

public class AN : MonoBehaviour
{
    public float moveSpeed = 0.25f;
    public float detectionRadius = 10f;
    public float minChaseDuration = 2f;
    public float maxChaseDuration = 5f;

    private Transform player;
    private bool isChasing = false;
    private float chaseTimer = 0f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            CheckPlayerDistance();
        }
    }

    private void CheckPlayerDistance()
    {
        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (distance < detectionRadius)
        {
            StartChasing();
        }
    }

    private void StartChasing()
    {
        isChasing = true;
        chaseTimer = Random.Range(minChaseDuration, maxChaseDuration);
    }

    private void ChasePlayer()
    {
        chaseTimer -= Time.deltaTime;

        if (chaseTimer <= 0f)
        {
            isChasing = false;
        }
        else
        {
            Vector3 direction = player.position - transform.position;
            Vector3 moveDirection = direction.normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chunk"))
        {
            transform.Rotate(Vector3.up, 180f);
        }
    }
}
