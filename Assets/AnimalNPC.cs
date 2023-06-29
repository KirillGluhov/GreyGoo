using UnityEngine;

public class AnimalNPC : MonoBehaviour
{
    public float moveSpeed = 3f; // �������� �������� NPC
    public float fleeDistance = 5f; // ���������, �� ������� NPC ������ ������� �� ������
    public float boundaryDistance = 10f; // ��������� �� ������� �����, �� ������� NPC ����� �������� �����������

    private Transform player; // ��������� ������
    private Vector3 fleeDirection; // ����������� ��� �������� �� ������

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // ��������� ����������� �������� NPC
        Vector3 moveDirection = CalculateMoveDirection();

        // ���������� NPC � �������� �����������
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private Vector3 CalculateMoveDirection()
    {
        // ��������� ������ ����������� �� NPC � ������
        Vector3 directionToPlayer = player.position - transform.position;

        // ���������, ���� ����� ��������� ���������� ������, ����� NPC ����� �������
        if (directionToPlayer.magnitude < fleeDistance)
        {
            // ��������� ����������� �������� �� ������
            fleeDirection = -directionToPlayer.normalized;
        }
        else
        {
            // ���������, ���� NPC ��������� ������ � ������� �����
            if (Mathf.Abs(transform.position.x) > boundaryDistance || Mathf.Abs(transform.position.z) > boundaryDistance)
            {
                // ��������� ����� ��������� ����������� �������� ������ ������� �����
                fleeDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            }
        }

        return fleeDirection;
    }
}
