using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Eat : MonoBehaviour
{
    public float levelOfHungry = 10f;
    private float radius = 1f;
    private int numberOfKilled = 0;
    private Rigidbody playerRigidbody;
    public GameObject hungryText;
    public string nextScene;
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();

        RectTransform rectTransform = hungryText.GetComponent<RectTransform>();

        rectTransform.anchorMin = new Vector2(0f, 1f);
        rectTransform.anchorMax = new Vector2(0f, 1f);

        rectTransform.anchoredPosition = new Vector2(90f, -40f);
    }
    void Update()
    {
        hungryText.GetComponent<Text>().text = "Hungry: " + ((int)levelOfHungry).ToString() + "\n" + "Oxygen: " + Mathf.RoundToInt(OxygenDisplay.oxygenAmount).ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
    private void FixedUpdate()
    {
        levelOfHungry -= 0.02f;

        if (levelOfHungry <= 0 || playerRigidbody.transform.position.y < -10)
        {
            playerRigidbody.transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 0));
            playerRigidbody.position = new Vector3(0, 8.25f, 0);
            playerRigidbody.transform.localScale = new Vector3(1f, 1f, 1f);
            radius = 1f;
            levelOfHungry = 100f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mob"))
        {
            Destroy(collision.gameObject);

            levelOfHungry += 20f;
            numberOfKilled++;

            radius = Mathf.Pow(1.0f + ((1.0f / 5.0f) * numberOfKilled), 1.0f / 3.0f);
            playerRigidbody.transform.localScale = new Vector3(radius, radius, radius);
        }
    }
}
