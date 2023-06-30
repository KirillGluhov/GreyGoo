using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenDisplay : MonoBehaviour
{
    public float oxygenLossRate = 1f;
    public float oxygenGainRate = 2f;
    public Text oxygenText;
    private bool inWater = false;

    private void Start()
    {
        Score.oxygenAmount = 100f;
    }

    private void Update()
    {
        if (inWater)
        {
            Score.oxygenAmount -= oxygenLossRate * Time.deltaTime;
        }
        else
        {
            Score.oxygenAmount += oxygenGainRate * Time.deltaTime;
            Score.oxygenAmount = Mathf.Clamp(Score.oxygenAmount, 0f, 100f);
        }
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
