using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextOfScore : MonoBehaviour
{
    public GameObject scoreText;

    void Update()
    {
        scoreText.GetComponent<Text>().text = "�� ������\r\n��� ����: " + (Score.numberOfKilled*100 - Score.numberOfDamage*10).ToString();
    }
}
