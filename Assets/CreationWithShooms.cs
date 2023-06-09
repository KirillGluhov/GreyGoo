using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreationWithShooms : MonoBehaviour
{
    public GameObject blockPrefabFirst;
    public GameObject blockPrefabSecond;
    public int heightOfCentrePlits;
    public string nextScene;

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

    public GameObject Water;
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

    public float[,] improveHeightMap(int size, float[,] heightMap)
    {
        int min = 0;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                heightMap[x, y] += 0.4F;
                heightMap[x, y] *= 50;
                heightMap[x, y] = (int)heightMap[x, y];

                min = Math.Min(min, (int)heightMap[x, y]);
            }

        }

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                heightMap[x, y] -= min;
            }

        }

        return heightMap;
    }

    public int[,] createMatrixOfLocationMobsandTrees(float[,] heightMap, int size, System.Random newRandom)
    {
        int[,] matrix;
        matrix = new int[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (heightMap[i, j] > 16 && newRandom.NextDouble() > 0.9)
                {
                    matrix[i, j] = (int)UnityEngine.Random.Range(0, 10);
                }
                else matrix[i, j] = -1;
                Debug.Log(matrix[i, j] + "\t");
            }
            Debug.Log("\n");
        }

        return matrix;
    }

    public int[,] improveMatrixWithTreesAndMobs(int size, int[,] matrix)
    {
        for (int i = 0; i < size; i++ )
        {
            for (int j = 0; j < size; j++)
            {
                if (matrix[i, j] > 5)
                {
                    switch(matrix[i, j])
                    {
                        case 6:
                            if (i - 1 > 0 && j - 1 > 0)
                            {
                                if (matrix[i - 1, j - 1] > 5)
                                {
                                    matrix[i - 1, j - 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i + 1 < size && j + 1 < size)
                            {
                                if (matrix[i + 1, j + 1] > 5)
                                {
                                    matrix[i + 1, j + 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i - 1 > 0 && j + 1 < size)
                            {
                                if (matrix[i - 1, j + 1] > 5)
                                {
                                    matrix[i - 1, j + 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i + 1 < size && j - 1 > 0)
                            {
                                if (matrix[i + 1, j - 1] > 5)
                                {
                                    matrix[i + 1, j - 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i - 1 > 0)
                            {
                                if (matrix[i - 1, j] > 5)
                                {
                                    matrix[i - 1, j] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (j - 1 > 0)
                            {
                                if (matrix[i, j - 1] > 5)
                                {
                                    matrix[i, j - 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i + 1 < size)
                            {
                                if (matrix[i + 1, j] > 5)
                                {
                                    matrix[i + 1, j] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (j + 1 < size)
                            {
                                if (matrix[i, j + 1] > 5)
                                {
                                    matrix[i, j + 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i-1 > 0 && j-2>0)
                            {
                                if (matrix[i-1, j-2] > 5)
                                {
                                    matrix[i-1, j-2] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (j - 2 > 0)
                            {
                                if (matrix[i, j - 2] > 5)
                                {
                                    matrix[i, j - 2] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i + 1 < size && j - 2 > 0)
                            {
                                if (matrix[i + 1, j - 2] > 5)
                                {
                                    matrix[i + 1, j - 2] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i - 1 > 0 && j + 2 < size)
                            {
                                if (matrix[i - 1, j + 2] > 5)
                                {
                                    matrix[i - 1, j + 2] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (j + 2 < size)
                            {
                                if (matrix[i, j + 2] > 5)
                                {
                                    matrix[i, j + 2] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i + 1 < size && j + 2 < size)
                            {
                                if (matrix[i + 1, j + 2] > 5)
                                {
                                    matrix[i + 1, j + 2] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }
                            break;
                        case 7:
                            if (i - 1 > 0 && j - 1 > 0)
                            {
                                if (matrix[i - 1, j - 1] > 5)
                                {
                                    matrix[i - 1, j - 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i + 1 < size && j + 1 < size)
                            {
                                if (matrix[i + 1, j + 1] > 5)
                                {
                                    matrix[i + 1, j + 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i - 1 > 0 && j + 1 < size)
                            {
                                if (matrix[i - 1, j + 1] > 5)
                                {
                                    matrix[i - 1, j + 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i + 1 < size && j - 1 > 0)
                            {
                                if (matrix[i + 1, j - 1] > 5)
                                {
                                    matrix[i + 1, j - 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i - 1 > 0)
                            {
                                if (matrix[i - 1, j] > 5)
                                {
                                    matrix[i - 1, j] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (j - 1 > 0)
                            {
                                if (matrix[i, j - 1] > 5)
                                {
                                    matrix[i, j - 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i + 1 < size)
                            {
                                if (matrix[i + 1, j] > 5)
                                {
                                    matrix[i + 1, j] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (j + 1 < size)
                            {
                                if (matrix[i, j + 1] > 5)
                                {
                                    matrix[i, j + 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i + 2 < size && j - 1 > 0)
                            {
                                if (matrix[i+2, j-1] > 5)
                                {
                                    matrix[i+2, j-1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i + 2 < size)
                            {
                                if (matrix[i + 2, j] > 5)
                                {
                                    matrix[i + 2, j] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i + 2 < size && j + 1 < size)
                            {
                                if (matrix[i + 2, j + 1] > 5)
                                {
                                    matrix[i + 2, j + 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            break;
                        case 8:

                            if (i-1 > 0 && j-1>0)
                            {
                                if (matrix[i-1,j-1] > 5)
                                {
                                    matrix[i - 1, j - 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i + 1 < size && j + 1  < size)
                            {
                                if (matrix[i + 1, j + 1] > 5)
                                {
                                    matrix[i + 1, j + 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i - 1 > 0 && j + 1 < size)
                            {
                                if (matrix[i - 1, j + 1] > 5)
                                {
                                    matrix[i - 1, j + 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i + 1 < size && j - 1 > 0)
                            {
                                if (matrix[i + 1, j - 1] > 5)
                                {
                                    matrix[i + 1, j - 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i-1>0)
                            {
                                if (matrix[i - 1, j] > 5)
                                {
                                    matrix[i - 1, j] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (j-1>0)
                            {
                                if (matrix[i, j-1] > 5)
                                {
                                    matrix[i, j-1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i + 1 < size)
                            {
                                if (matrix[i + 1, j] > 5)
                                {
                                    matrix[i + 1, j] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (j + 1 < size)
                            {
                                if (matrix[i, j + 1] > 5)
                                {
                                    matrix[i, j + 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }
                            break;
                        case 9:
                            if (i - 1 > 0 && j - 1 > 0)
                            {
                                if (matrix[i - 1, j - 1] > 5)
                                {
                                    matrix[i - 1, j - 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i + 1 < size && j + 1 < size)
                            {
                                if (matrix[i + 1, j + 1] > 5)
                                {
                                    matrix[i + 1, j + 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i - 1 > 0 && j + 1 < size)
                            {
                                if (matrix[i - 1, j + 1] > 5)
                                {
                                    matrix[i - 1, j + 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i + 1 < size && j - 1 > 0)
                            {
                                if (matrix[i + 1, j - 1] > 5)
                                {
                                    matrix[i + 1, j - 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i - 1 > 0)
                            {
                                if (matrix[i - 1, j] > 5)
                                {
                                    matrix[i - 1, j] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (j - 1 > 0)
                            {
                                if (matrix[i, j - 1] > 5)
                                {
                                    matrix[i, j - 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (i + 1 < size)
                            {
                                if (matrix[i + 1, j] > 5)
                                {
                                    matrix[i + 1, j] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }

                            if (j + 1 < size)
                            {
                                if (matrix[i, j + 1] > 5)
                                {
                                    matrix[i, j + 1] = (int)UnityEngine.Random.Range(0, 6);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        return matrix;
    }

    public void generateMobsTreesAndGoo(int i, int j, int h, int[,] matrix, int size)
    {
        if (i == 0 && j == 0)
        {
            heightOfCentrePlits = h;
            GameObject.Find("Goo").transform.position = new Vector3(0, (h + 1) * 0.0625F, 0);
        }
        else
        {
            switch(matrix[i + size/2, j + size/2])
            {
                case 0:
                    GameObject newMuskrat = Instantiate(Muskrat, new Vector3(j, h * 0.0625F, i), Quaternion.identity);
                    break;
                case 1:
                    GameObject newColobus = Instantiate(Colobus, new Vector3(j, h * 0.0625F, i), Quaternion.identity);
                    break;
                case 2:
                    GameObject newGecko = Instantiate(Gecko, new Vector3(j, h * 0.0625F, i), Quaternion.identity);
                    break;
                case 3:
                    GameObject newPurdu = Instantiate(Purdu, new Vector3(j, h * 0.0625F, i), Quaternion.identity);
                    break;
                case 4:
                    GameObject newSparrow = Instantiate(Sparrow, new Vector3(j, h * 0.0625F, i), Quaternion.identity);
                    break;
                case 5:
                    GameObject newTaipan = Instantiate(Taipan, new Vector3(j, h * 0.0625F, i), Quaternion.identity);
                    break;
                case 6:
                    GameObject newOak = Instantiate(Oak, new Vector3(j, h * 0.0625F, i), Quaternion.identity);
                    break;
                case 7:
                    GameObject newPalm = Instantiate(Palm, new Vector3(j, h * 0.0625F, i), Quaternion.identity);
                    break;
                case 8:
                    GameObject newPoplar = Instantiate(Poplar, new Vector3(j, h * 0.0625F, i), Quaternion.identity);
                    break;
                case 9:
                    GameObject newFir = Instantiate(Fir, new Vector3(j, h * 0.0625F, i), Quaternion.identity);
                    break;
                default:
                    break;

            }
        }
    }
    void Start()
    {
        int size = 17;
        System.Random newRandom = new System.Random();
        float roughness = (float)newRandom.NextDouble();

        DiamondSquareAlgorithm diamondSquare = new DiamondSquareAlgorithm(size);
        float[,] heightMap = improveHeightMap(size, diamondSquare.GenerateHeightMap(roughness));
        int[,] probablyTreesAndMobs = improveMatrixWithTreesAndMobs(size, createMatrixOfLocationMobsandTrees(heightMap, size, newRandom));

        for (int i = -(size / 2); i < size - (size / 2); i++)
        {
            for (int j = -(size / 2); j < size - (size / 2); j++)
            {
                int h = 0;
                for (int k = 0; k < heightMap[i + (size / 2), j + (size / 2)]; k++)
                {
                    Vector3 position = new Vector3(j, k * 0.0625F, i);
                    h = k + 1;
                    GameObject block = Instantiate(blockPrefabFirst, position, Quaternion.identity);
                }

                Vector3 positionSecond = new Vector3(j, h * 0.0625F, i);
                GameObject blockSecond = Instantiate(blockPrefabSecond, positionSecond, Quaternion.identity);

                if (h < 16)
                {
                    GameObject newWater = Instantiate(Water, new Vector3(j, 16 * 0.0625F, i), Quaternion.identity);
                    newWater.transform.Rotate(90, 0, 0);
                }

                generateMobsTreesAndGoo(i, j, h, probablyTreesAndMobs, size);
                
            }
        }
    }

    void Update()
    {
        if (GameObject.Find("Goo").transform.position.y < -10)
        {
            GameObject.Find("Goo").transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 0));
            GameObject.Find("Goo").transform.position = new Vector3(0, (heightOfCentrePlits + 2) * 0.0625F, 0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
