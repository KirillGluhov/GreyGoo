using UnityEngine;

public class AnimalNPC : MonoBehaviour
{
    public float moveSpeed = 3f; // —корость движени€ NPC
    public float fleeDistance = 5f; // ƒистанци€, на которой NPC начнет убегать от игрока
    public float boundaryDistance = 10f; // ƒистанци€ от границы карты, на которой NPC будет измен€ть направление

    private Transform player; // “рансформ игрока
    private Vector3 fleeDirection; // Ќаправление дл€ убегани€ от игрока
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
        // ¬ычисл€ем направление движени€ NPC
        Vector3 moveDirection = CalculateMoveDirection();

        // ѕеремещаем NPC в заданном направлении
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private Vector3 CalculateMoveDirection()
    {
        // ¬ычисл€ем вектор направлени€ от NPC к игроку
        Vector3 directionToPlayer = player.position - transform.position;

        // ѕровер€ем, если игрок находитс€ достаточно близко, чтобы NPC начал убегать
        if (directionToPlayer.magnitude < fleeDistance)
        {
            // ¬ычисл€ем направление убегани€ от игрока
            fleeDirection = -directionToPlayer.normalized;
        }
        else
        {
            // ѕровер€ем, если NPC находитс€ близко к границе карты
            if (Mathf.Abs(transform.position.x) > boundaryDistance || Mathf.Abs(transform.position.z) > boundaryDistance)
            {
                // ¬ычисл€ем новое случайное направление движени€ внутри границы карты
                fleeDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            }
        }

        return fleeDirection;
    }
}
