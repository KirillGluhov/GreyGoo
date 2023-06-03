using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CreationWithShooms : MonoBehaviour
{
    public GameObject blockPrefabFirst;
    public GameObject blockPrefabSecond;
    public class DiamondSquareAlgorithm
    {
        private int size;
        private float[,] heightMap;

        public DiamondSquareAlgorithm(int size)
        {
            this.size = size;
            this.heightMap = new float[size, size];
        }

        public float[,] GenerateHeightMap(float roughness)
        {
            heightMap[0, 0] = RandomOffset(roughness);
            heightMap[0, size - 1] = RandomOffset(roughness);
            heightMap[size - 1, 0] = RandomOffset(roughness);
            heightMap[size - 1, size - 1] = RandomOffset(roughness);

            int stepSize = size - 1;
            float scale = roughness;

            while (stepSize > 1)
            {
                DiamondStep(stepSize, scale);
                SquareStep(stepSize, scale);

                stepSize /= 2;
                scale *= roughness;
            }

            return heightMap;
        }

        private void DiamondStep(int stepSize, float scale)
        {
            int halfStep = stepSize / 2;

            for (int y = halfStep; y <= size - 1; y += stepSize)
            {
                for (int x = halfStep; x <= size - 1; x += stepSize)
                {
                    float average = heightMap[(x - halfStep + size) % size, (y - halfStep + size) % size] +
                                heightMap[(x + halfStep) % size, (y - halfStep + size) % size] +
                                heightMap[(x - halfStep + size) % size, (y + halfStep) % size] +
                                heightMap[(x + halfStep) % size, (y + halfStep) % size];

                    average /= 4.0f;

                    heightMap[x, y] = average + RandomOffset(scale);
                }
            }
        }

        private void SquareStep(int stepSize, float scale)
        {
            int halfStep = stepSize / 2;

            for (int y = 0; y <= size - 1; y += stepSize)
            {
                for (int x = halfStep; x <= size - 1; x += stepSize)
                {
                    float average = heightMap[(x - halfStep + size) % size, y] +
                                heightMap[(x + halfStep) % size, y] +
                                heightMap[x, (y + halfStep) % size] +
                                heightMap[x, (y - halfStep + size) % size];

                    average /= 4.0f;

                    heightMap[x, y] = average + RandomOffset(scale);
                }
            }

            for (int y = halfStep; y <= size - 1; y += stepSize)
            {
                for (int x = 0; x <= size - 1; x += stepSize)
                {
                    float average = heightMap[(x - halfStep + size) % size, y] +
                                heightMap[(x + halfStep) % size, y] +
                                heightMap[x, (y + halfStep) % size] +
                                heightMap[x, (y - halfStep + size) % size];

                    average /= 4.0f;

                    heightMap[x, y] = average + RandomOffset(scale);
                }
            }
        }

        private float RandomOffset(float scale)
        {
            System.Random random = new System.Random();
            return ((float)random.NextDouble() - 0.5f) * scale;
        }
    }
    void Start()
    {
        int size = 17;
        System.Random random = new System.Random();
        float roughness = (float)random.NextDouble();

        DiamondSquareAlgorithm diamondSquare = new DiamondSquareAlgorithm(size);
        float[,] heightMap = diamondSquare.GenerateHeightMap(roughness);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                heightMap[x, y] += 0.4F;
                heightMap[x, y] *= 50;
                Console.WriteLine(heightMap[x, y] + " ");
            }
            Console.WriteLine();

        }

        for (int i = -(size/2); i < size - (size/2); i++)
        {
            for (int j = -(size / 2); j < size - (size / 2); j++)
            {
                int h = 0;
                for (int k = 0; k < heightMap[i + (size / 2), j + (size / 2)]; k++)
                {
                    Vector3 position = new Vector3(j, k * 0.0625F, i);
                    h = k+1;
                    GameObject block = Instantiate(blockPrefabFirst, position, Quaternion.identity);
                }

                Vector3 positionSecond = new Vector3(j, h * 0.0625F, i);
                GameObject blockSecond = Instantiate(blockPrefabSecond, positionSecond, Quaternion.identity);

            }
        }

    }

    void Update()
    {
        
    }
}
