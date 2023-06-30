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
    private Rigidbody playerRigidbody;
    public GameObject hungryText;
    public string nextScene = "Death Score";
    public string previousScene;
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();

        RectTransform rectTransform = hungryText.GetComponent<RectTransform>();

        rectTransform.anchorMin = new Vector2(0f, 1f);
        rectTransform.anchorMax = new Vector2(0f, 1f);

        rectTransform.anchoredPosition = new Vector2(90f, -60f);
        Score.numberOfDamage = 0;
        Score.numberOfKilled = 0;
    }
    void Update()
    {
        if (Score.oxygenAmount <= 99f && Score.numberOfKilled > 0)
        {
            hungryText.GetComponent<Text>().text = "Hungry: " + ((int)levelOfHungry).ToString() + "\n" + "Current Score: " + (Score.numberOfKilled * 100 - Score.numberOfDamage * 10).ToString() + "\n" + "Oxygen: " + Mathf.RoundToInt(Score.oxygenAmount).ToString();
        }
        else if (Score.oxygenAmount <= 99f)
        {
            hungryText.GetComponent<Text>().text = "Hungry: " + ((int)levelOfHungry).ToString() + "\n" + "Oxygen: " + Mathf.RoundToInt(Score.oxygenAmount).ToString();
        }
        else if (Score.numberOfKilled > 0)
        {
            hungryText.GetComponent<Text>().text = "Hungry: " + ((int)levelOfHungry).ToString() + "\n" + "Current Score: " + (Score.numberOfKilled * 100 - Score.numberOfDamage * 10).ToString();
        }
        else
        {
            hungryText.GetComponent<Text>().text = "Hungry: " + ((int)levelOfHungry).ToString();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(previousScene);
        }
    }
    private void FixedUpdate()
    {
        levelOfHungry -= 0.02f;

        if (playerRigidbody.transform.position.y < -10)
        {
            playerRigidbody.transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 0));
            playerRigidbody.position = new Vector3(0, 8.25f, 0);
            playerRigidbody.transform.localScale = new Vector3(1f, 1f, 1f);
            radius = 1f;
            levelOfHungry = 100f;
            Score.oxygenAmount = 100f;
        }

        if (levelOfHungry <= 0 || Score.oxygenAmount <= 0)
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mob"))
        {
            Destroy(collision.gameObject);

            levelOfHungry += 20f;
            Score.numberOfKilled++;

            radius = Mathf.Pow(1.0f + ((1.0f / 5.0f) * Score.numberOfKilled), 1.0f / 3.0f);
            playerRigidbody.transform.localScale = new Vector3(radius, radius, radius);
        }
        else if (collision.gameObject.CompareTag("Killed"))
        { 
            levelOfHungry -= 20f;
            Score.numberOfDamage++;
            // numberOfKilled++;

            //radius = -Mathf.Pow(1.0f + ((1.0f / 5.0f) * numberOfKilled), 1.0f / 3.0f);
            //playerRigidbody.transform.localScale = new Vector3(radius, radius, radius);
        }
    }

}


