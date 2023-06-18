using UnityEngine;

public class AnimalNPC : MonoBehaviour
{
    //public float moveSpeed = 3f; // Скорость движения NPC
    //public float avoidanceRadius = 5f; // Радиус, в котором NPC избегает игрока

    //private Transform player; // Трансформ игрока

    //private void Awake()
    //{
    //    player = GameObject.FindGameObjectWithTag("Player").transform; // Находим игрока по тегу "Player"
    //}

    //private void Update()
    //{
    //    // Рассчитываем направление движения NPC
    //    Vector3 direction = transform.position - player.position;

    //    // Если игрок находится в радиусе избегания, двигаемся в противоположную сторону
    //    if (direction.magnitude < avoidanceRadius)
    //    {
    //        Vector3 moveDirection = direction.normalized;
    //        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    //    }
    //}
}