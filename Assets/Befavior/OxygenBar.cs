using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OxygenDisplay : MonoBehaviour
{
    public float oxygenAmount = 100f; 
    public float oxygenLossRate = 1f; 
    public Text oxygenText; 
    private bool inWater = false;
    private void Update()
    {
        if (inWater)
        {
            oxygenAmount -= oxygenLossRate * Time.deltaTime;
            UpdateOxygenDisplay(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            inWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            inWater = false; 
        }
    }

    private void UpdateOxygenDisplay()
    {
        oxygenText.text = "Oxygen: " + Mathf.RoundToInt(oxygenAmount).ToString(); 
    }
}

