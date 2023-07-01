using UnityEngine;

public class AnimalNPC : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float fleeDistance = 5f;
    public float boundaryDistance = 10f;

    private Transform player;
    private Vector3 fleeDirection;
    private Rigidbody rb;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;

    }

    private void Update()
    {
        Vector3 moveDirection = CalculateMoveDirection();
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private Vector3 CalculateMoveDirection()
    {
        Vector3 directionToPlayer = player.position - transform.position;

        if (directionToPlayer.magnitude < fleeDistance)
        {
            fleeDirection = -directionToPlayer.normalized;
        }
        else
        {
            if (Mathf.Abs(transform.position.x) > boundaryDistance || Mathf.Abs(transform.position.z) > boundaryDistance)
            {
                fleeDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            }
        }

        return fleeDirection;
    }
}
