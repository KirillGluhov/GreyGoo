using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenDisplay : MonoBehaviour
{
    public static float oxygenAmount = 100f;
    public float oxygenLossRate = 1f;
    public float oxygenGainRate = 2f;
    public Text oxygenText;
    private bool inWater = false;

    private void Update()
    {
        if (inWater)
        {
            oxygenAmount -= oxygenLossRate * Time.deltaTime;
            UpdateOxygenDisplay();
        }
        else
        {
            oxygenAmount += oxygenGainRate * Time.deltaTime;
            oxygenAmount = Mathf.Clamp(oxygenAmount, 0f, 100f);
            UpdateOxygenDisplay();
        }
    }

    private void UpdateOxygenDisplay()
    {
        oxygenText.text = "Oxygen: " + Mathf.RoundToInt(oxygenAmount).ToString();
    }

    private void FixedUpdate()
    {
        float waterLevel = TerrainGenerator.waterLevel * ChunkGenerator.BlockScale - 0.5f * ChunkGenerator.BlockScale;

        if (transform.position.y < waterLevel)
        {
            if (!inWater)
            {
                inWater = true;
            }
        }
        else
        {
            if (inWater)
            {
                inWater = false;
            }
        }
    }
}
