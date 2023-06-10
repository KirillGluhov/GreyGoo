using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class Eat : MonoBehaviour
{
    public float levelOfHungry = 10f;
    private Rigidbody playerRigidbody;
    public GameObject hungryText;
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();

        RectTransform rectTransform = hungryText.GetComponent<RectTransform>();

        rectTransform.anchorMin = new Vector2(0f, 1f);
        rectTransform.anchorMax = new Vector2(0f, 1f);

        rectTransform.anchoredPosition = new Vector2(90f, -20f);
    }
    void Update()
    {
        hungryText.GetComponent<Text>().text = "Hungry: " + ((int)levelOfHungry).ToString();
    }
    private void FixedUpdate()
    {
        levelOfHungry -= 0.02f;

        if (levelOfHungry <= 0 || playerRigidbody.transform.position.y < -10)
        {
            playerRigidbody.transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 0));
            playerRigidbody.position = new Vector3(0, (CreationWithShooms.heightOfCentrePlits + 2) * 0.0625F, 0);
            levelOfHungry = 100f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mob"))
        {
            Destroy(collision.gameObject);
            levelOfHungry += 20f;
        }
    }
}
