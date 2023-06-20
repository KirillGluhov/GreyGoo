using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject Muskrat;
    public GameObject Colobus;
    public GameObject Gecko;
    public GameObject Purdu;
    public GameObject Sparrow;
    public GameObject Taipan;
    public GameObject Oak;
    public GameObject Palm;
    public GameObject Poplar;
    public GameObject Fir;

    public float BaseHeight = 8f;
    public NoiseOctaveSettings[] Octaves;
    public NoiseOctaveSettings DomainWarp;

    [Serializable]
    public class NoiseOctaveSettings
    {
        public FastNoiseLite.NoiseType noiseType;
        public float Frequency = 0.2f;
        public float Amplitude = 1;
    }

    private FastNoiseLite[] octaveNoises;

    private FastNoiseLite warpNoise;

    public void Init()
    {
        octaveNoises = new FastNoiseLite[Octaves.Length];

        for (int i = 0; i < octaveNoises.Length; i++)
        {
            octaveNoises[i] = new FastNoiseLite();
            octaveNoises[i].SetNoiseType(Octaves[i].noiseType);
            octaveNoises[i].SetFrequency(Octaves[i].Frequency);
        }

        warpNoise = new FastNoiseLite();
        warpNoise.SetNoiseType(DomainWarp.noiseType);
        warpNoise.SetFrequency(DomainWarp.Frequency);
        warpNoise.SetDomainWarpAmp(DomainWarp.Amplitude);
    }
    public infoaboutChunk GenerateTerrain(float xOffset, float zOffset)
    {
        var resultOfHeights = new BlockType[ChunkGenerator.ChunkWidth, ChunkGenerator.ChunkHeight, ChunkGenerator.ChunkWidth];
        var result = new List<GameObject>();

        for (int x = 0; x < ChunkGenerator.ChunkWidth; x++)
        {
            for (int z = 0; z < ChunkGenerator.ChunkWidth; z++)
            {
                float height = GetHeight((x * ChunkGenerator.BlockScale + xOffset), (z * ChunkGenerator.BlockScale + zOffset));
                float grassLayerHeight = 1;
                int waterLevel = 30;
                Random random = new Random();

                for (int y = 0; y < height/ChunkGenerator.BlockScale; y++)
                {
                    if (height - y*ChunkGenerator.BlockScale < grassLayerHeight)
                    {
                        resultOfHeights[x, y, z] = BlockType.Grass;
                    }
                    else
                    {
                        resultOfHeights[x, y, z] = BlockType.Stone;
                    }
                }

                if (resultOfHeights[x, waterLevel, z] != BlockType.Stone && resultOfHeights[x, waterLevel, z] != BlockType.Grass)
                {
                    resultOfHeights[x, waterLevel, z] = BlockType.Water;
                }

                if (random.NextDouble() > 0.999 && height / ChunkGenerator.BlockScale > waterLevel)
                {
                    float chooseAnimal = UnityEngine.Random.Range(0, 10);
                    Vector3 place = new Vector3(x * ChunkGenerator.BlockScale + xOffset, height, z * ChunkGenerator.BlockScale + zOffset);

                    if (chooseAnimal < 1)
                    {
                        GameObject newMuskrat = Instantiate(Muskrat, place, Quaternion.identity);
                        result.Add(newMuskrat);
                    }
                    else if (chooseAnimal < 2)
                    {
                        GameObject newColobus = Instantiate(Colobus, place, Quaternion.identity);
                        result.Add(newColobus);
                    }
                    else if (chooseAnimal < 3)
                    {
                        GameObject newGecko = Instantiate(Gecko, place, Quaternion.identity);
                        result.Add(newGecko);
                    }
                    else if (chooseAnimal < 4)
                    {
                        GameObject newPurdu = Instantiate(Purdu, place, Quaternion.identity);
                        result.Add(newPurdu);
                    }
                    else if (chooseAnimal < 5)
                    {
                        GameObject newSparrow = Instantiate(Sparrow, place, Quaternion.identity);
                        result.Add(newSparrow);
                    }
                    else if (chooseAnimal < 6)
                    {
                        GameObject newTaipan = Instantiate(Taipan, place, Quaternion.identity);
                        result.Add(newTaipan);
                    }
                    else if (chooseAnimal < 7)
                    {
                        resultOfHeights[x, (int)((height / ChunkGenerator.BlockScale) + ChunkGenerator.BlockScale), z] = BlockType.Wood;
                        //GameObject newOak = Instantiate(Oak, place, Quaternion.identity);
                        //result.Add(newOak);
                    }
                    else if (chooseAnimal < 8)
                    {
                        resultOfHeights[x, (int)((height / ChunkGenerator.BlockScale) + ChunkGenerator.BlockScale), z] = BlockType.Wood;
                        //GameObject newPalm = Instantiate(Palm, place, Quaternion.identity);
                        //result.Add(newPalm);
                    }
                    else if (chooseAnimal < 9)
                    {
                        resultOfHeights[x, (int)((height / ChunkGenerator.BlockScale) + ChunkGenerator.BlockScale), z] = BlockType.Wood;
                        //GameObject newPoplar = Instantiate(Poplar, place, Quaternion.identity);
                        //result.Add(newPoplar);
                    }
                    else if (chooseAnimal < 10)
                    {
                        resultOfHeights[x, (int)((height / ChunkGenerator.BlockScale) + ChunkGenerator.BlockScale), z] = BlockType.Wood;
                        //GameObject newFir = Instantiate(Fir, place, Quaternion.identity);
                        //result.Add(newFir);
                    }
                }


            }
        }

        infoaboutChunk newInfo = new infoaboutChunk();
        newInfo.Blocks = resultOfHeights;
        newInfo.AnimalsAndTrees = result;

        return newInfo;
    }

    private float GetHeight(float x, float y)
    {
        warpNoise.DomainWarp(ref x,ref y);

        float result = BaseHeight;

        for (int i = 0; i < Octaves.Length; i++)
        {
            float noise = octaveNoises[i].GetNoise(x, y);
            result += noise * Octaves[i].Amplitude/2;
        }

        return result;
    }
}
