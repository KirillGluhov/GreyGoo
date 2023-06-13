using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class CreationWithShooms : MonoBehaviour
{
    public GameObject blockPrefabFirst;
    public GameObject blockPrefabSecond;
    public static int heightOfCentrePlits;
    public static List<GameObject> Mobs = new List<GameObject>();
    public static List<InfoAboutArrays> allChunks = new List<InfoAboutArrays>();
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
            }
        }

        return matrix;
    }

    public int[,] improveMatrixWithTreesAndMobs(int size, int[,] matrix)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (matrix[i, j] > 5)
                {
                    switch (matrix[i, j])
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

                            if (i - 1 > 0 && j - 2 > 0)
                            {
                                if (matrix[i - 1, j - 2] > 5)
                                {
                                    matrix[i - 1, j - 2] = (int)UnityEngine.Random.Range(0, 6);
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
                                if (matrix[i + 2, j - 1] > 5)
                                {
                                    matrix[i + 2, j - 1] = (int)UnityEngine.Random.Range(0, 6);
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
                        case 9:
                            if (i - 1 > 0 && j - 1 > 0)
                            {
                                if (matrix[i - 1, j - 1] > 5)
                                {
                                    matrix[i - 1, j - 1] = 11;
                                }
                            }

                            if (i + 1 < size && j + 1 < size)
                            {
                                if (matrix[i + 1, j + 1] > 5)
                                {
                                    matrix[i + 1, j + 1] = 11;
                                }
                            }

                            if (i - 1 > 0 && j + 1 < size)
                            {
                                if (matrix[i - 1, j + 1] > 5)
                                {
                                    matrix[i - 1, j + 1] = 11;
                                }
                            }

                            if (i + 1 < size && j - 1 > 0)
                            {
                                if (matrix[i + 1, j - 1] > 5)
                                {
                                    matrix[i + 1, j - 1] = 11;
                                }
                            }

                            if (i - 1 > 0)
                            {
                                if (matrix[i - 1, j] > 5)
                                {
                                    matrix[i - 1, j] = 11;
                                }
                            }

                            if (j - 1 > 0)
                            {
                                if (matrix[i, j - 1] > 5)
                                {
                                    matrix[i, j - 1] = 11;
                                }
                            }

                            if (i + 1 < size)
                            {
                                if (matrix[i + 1, j] > 5)
                                {
                                    matrix[i + 1, j] = 11;
                                }
                            }

                            if (j + 1 < size)
                            {
                                if (matrix[i, j + 1] > 5)
                                {
                                    matrix[i, j + 1] = 11;
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

    public void generateMobsTreesAndGoo(int i, int j, int h, int[,] matrix, int size, float[,] heightMap, InfoAboutArrays chunk, int AxisX, int AxisZ)
    {
        if (i == 0 && j == 0 && AxisZ == 0 && AxisX == 0)
        {
            heightOfCentrePlits = h;
            GameObject.Find("Goo").transform.position = new UnityEngine.Vector3(0, (h + 1) * 0.0625F, 0);
        }
        else
        {
            switch (matrix[i + size / 2, j + size / 2])
            {
                case 0:
                    GameObject newMuskrat = Instantiate(Muskrat, new UnityEngine.Vector3(j + AxisX * size, h * 0.0625F, i + AxisZ * size), UnityEngine.Quaternion.identity);
                    aliveAnimal newMuskratAll = new aliveAnimal();
                    Mobs.Add(newMuskrat);

                    newMuskratAll.animal = newMuskrat;
                    newMuskratAll.typeOfAnimal = "Muskrat";
                    chunk.localAnimals.Add(newMuskratAll);
                    break;
                case 1:
                    GameObject newColobus = Instantiate(Colobus, new UnityEngine.Vector3(j + AxisX * size, h * 0.0625F, i + AxisZ * size), UnityEngine.Quaternion.identity);
                    aliveAnimal newColobusAll = new aliveAnimal();
                    Mobs.Add(newColobus);

                    newColobusAll.animal = newColobus;
                    newColobusAll.typeOfAnimal = "Colobus";
                    chunk.localAnimals.Add(newColobusAll);
                    break;
                case 2:
                    GameObject newGecko = Instantiate(Gecko, new UnityEngine.Vector3(j + AxisX * size, h * 0.0625F, i + AxisZ * size), UnityEngine.Quaternion.identity);
                    aliveAnimal newGeckoAll = new aliveAnimal();
                    Mobs.Add(newGecko);

                    newGeckoAll.animal = newGecko;
                    newGeckoAll.typeOfAnimal = "Gecko";
                    chunk.localAnimals.Add(newGeckoAll);
                    break;
                case 3:
                    GameObject newPurdu = Instantiate(Purdu, new UnityEngine.Vector3(j + AxisX * size, h * 0.0625F, i + AxisZ * size), UnityEngine.Quaternion.identity);
                    aliveAnimal newPurduAll = new aliveAnimal();
                    Mobs.Add(newPurdu);

                    newPurduAll.animal = newPurdu;
                    newPurduAll.typeOfAnimal = "Purdu";
                    chunk.localAnimals.Add(newPurduAll);
                    break;
                case 4:
                    GameObject newSparrow = Instantiate(Sparrow, new UnityEngine.Vector3(j + AxisX * size, h * 0.0625F, i + AxisZ * size), UnityEngine.Quaternion.identity);
                    aliveAnimal newSparrowAll = new aliveAnimal();
                    Mobs.Add(newSparrow);

                    newSparrowAll.animal = newSparrow;
                    newSparrowAll.typeOfAnimal = "Sparrow";
                    chunk.localAnimals.Add(newSparrowAll);
                    break;
                case 5:
                    GameObject newTaipan = Instantiate(Taipan, new UnityEngine.Vector3(j + AxisX * size, h * 0.0625F, i + AxisZ * size), UnityEngine.Quaternion.identity);
                    aliveAnimal newTaipanAll = new aliveAnimal();
                    Mobs.Add(newTaipan);

                    newTaipanAll.animal = newTaipan;
                    newTaipanAll.typeOfAnimal = "Taipan";
                    chunk.localAnimals.Add(newTaipanAll);
                    break;
                case 6:
                    GameObject newOak = Instantiate(Oak, new UnityEngine.Vector3(j + AxisX * size, h * 0.0625F, i + AxisZ * size), UnityEngine.Quaternion.identity);
                    break;
                case 7:
                    GameObject newPalm = Instantiate(Palm, new UnityEngine.Vector3(j + AxisX * size, h * 0.0625F, i + AxisZ * size), UnityEngine.Quaternion.identity);
                    break;
                case 8:
                    GameObject newPoplar = Instantiate(Poplar, new UnityEngine.Vector3(j + AxisX * size, h * 0.0625F, i + AxisZ * size), UnityEngine.Quaternion.identity);
                    break;
                case 9:
                    GameObject newFir = Instantiate(Fir, new UnityEngine.Vector3(j + AxisX * size, h * 0.0625F, i + AxisZ * size), UnityEngine.Quaternion.identity);
                    break;
                default:
                    break;

            }
        }
    }

    public void regenerateMobsTreesAndGoo(int i, int j, int h, InfoAboutArrays chunk, int size, int AxisX, int AxisZ)
    {
        switch (chunk.matrixOfTrees[i + size / 2, j + size / 2])
        {
            case 6:
                GameObject newOak = Instantiate(Oak, new UnityEngine.Vector3(j + AxisX * size, h * 0.0625F, i + AxisZ * size), UnityEngine.Quaternion.identity);
                chunk.treesWaterAndPlits.Add(newOak);
                break;
            case 7:
                GameObject newPalm = Instantiate(Palm, new UnityEngine.Vector3(j + AxisX * size, h * 0.0625F, i + AxisZ * size), UnityEngine.Quaternion.identity);
                chunk.treesWaterAndPlits.Add(newPalm);
                break;
            case 8:
                GameObject newPoplar = Instantiate(Poplar, new UnityEngine.Vector3(j + AxisX * size, h * 0.0625F, i + AxisZ * size), UnityEngine.Quaternion.identity);
                chunk.treesWaterAndPlits.Add(newPoplar);
                break;
            case 9:
                GameObject newFir = Instantiate(Fir, new UnityEngine.Vector3(j + AxisX * size, h * 0.0625F, i + AxisZ * size), UnityEngine.Quaternion.identity);
                chunk.treesWaterAndPlits.Add(newFir);
                break;
            default:
                break;

        }

        for (int k = 0; k < chunk.InformationAboutAnimal.Count; k++)
        {
            switch(chunk.InformationAboutAnimal[k].typeOfAnimal)
            {
                case "Muskrat":
                    GameObject newMuskrat = Instantiate(Muskrat, chunk.InformationAboutAnimal[k].orientation.position, UnityEngine.Quaternion.identity);
                    newMuskrat.transform.rotation = chunk.InformationAboutAnimal[k].orientation.rotation;

                    aliveAnimal newMuskratAll = new aliveAnimal();
                    Mobs.Add(newMuskrat);

                    newMuskratAll.animal = newMuskrat;
                    newMuskratAll.typeOfAnimal = "Muskrat";
                    chunk.localAnimals.Add(newMuskratAll);

                    break;
                case "Colobus":
                    GameObject newColobus = Instantiate(Colobus, chunk.InformationAboutAnimal[k].orientation.position, UnityEngine.Quaternion.identity);
                    newColobus.transform.rotation = chunk.InformationAboutAnimal[k].orientation.rotation;

                    aliveAnimal newColobusAll = new aliveAnimal();
                    Mobs.Add(newColobus);

                    newColobusAll.animal = newColobus;
                    newColobusAll.typeOfAnimal = "Colobus";
                    chunk.localAnimals.Add(newColobusAll);
                    break;
                case "Gecko":
                    GameObject newGecko = Instantiate(Gecko, chunk.InformationAboutAnimal[k].orientation.position, UnityEngine.Quaternion.identity);
                    newGecko.transform.rotation = chunk.InformationAboutAnimal[k].orientation.rotation;

                    aliveAnimal newGeckoAll = new aliveAnimal();
                    Mobs.Add(newGecko);

                    newGeckoAll.animal = newGecko;
                    newGeckoAll.typeOfAnimal = "Gecko";
                    chunk.localAnimals.Add(newGeckoAll);
                    break;
                case "Purdu":
                    GameObject newPurdu = Instantiate(Purdu, chunk.InformationAboutAnimal[k].orientation.position, UnityEngine.Quaternion.identity);
                    newPurdu.transform.rotation = chunk.InformationAboutAnimal[k].orientation.rotation;

                    aliveAnimal newPurduAll = new aliveAnimal();
                    Mobs.Add(newPurdu);

                    newPurduAll.animal = newPurdu;
                    newPurduAll.typeOfAnimal = "Purdu";
                    chunk.localAnimals.Add(newPurduAll);
                    break;
                case "Sparrow":
                    GameObject newSparrow = Instantiate(Sparrow, chunk.InformationAboutAnimal[k].orientation.position, UnityEngine.Quaternion.identity);
                    newSparrow.transform.rotation = chunk.InformationAboutAnimal[k].orientation.rotation;

                    aliveAnimal newSparrowAll = new aliveAnimal();
                    Mobs.Add(newSparrow);

                    newSparrowAll.animal = newSparrow;
                    newSparrowAll.typeOfAnimal = "Sparrow";
                    chunk.localAnimals.Add(newSparrowAll);
                    break;
                case "Taipan":
                    GameObject newTaipan = Instantiate(Taipan, chunk.InformationAboutAnimal[k].orientation.position, UnityEngine.Quaternion.identity);
                    newTaipan.transform.rotation = chunk.InformationAboutAnimal[k].orientation.rotation;

                    aliveAnimal newTaipanAll = new aliveAnimal();
                    Mobs.Add(newTaipan);

                    newTaipanAll.animal = newTaipan;
                    newTaipanAll.typeOfAnimal = "Taipan";
                    chunk.localAnimals.Add(newTaipanAll);
                    break;
                default:
                    break;
            }
        }

        chunk.InformationAboutAnimal.Clear();
    }

    ///////////////////////////////////

    public void deleteThisChunk(int AxisX, int AxisZ, InfoAboutArrays chunk)
    {
        chunk.InformationAboutAnimal.Clear();
        for (int i = 0; i < chunk.localAnimals.Count; i++)
        {
            AnimalInfo newAnimal = new AnimalInfo();
            newAnimal.orientation = chunk.localAnimals[i].animal.GetComponent<Transform>();
            newAnimal.typeOfAnimal = chunk.localAnimals[i].typeOfAnimal;
            chunk.InformationAboutAnimal.Add(newAnimal);
        }

        for (int i = 0;i < chunk.localAnimals.Count;i++)
        {
            Destroy(chunk.localAnimals[i].animal);
        }

        chunk.localAnimals.Clear();


        for (int i = 0; i < chunk.treesWaterAndPlits.Count;i++)
        {
            Destroy(chunk.treesWaterAndPlits[i]);
        }

        chunk.treesWaterAndPlits.Clear();
    }

    public void deleteChunk(int AxisX, int AxisZ)
    {
        for (int i = 0; i < allChunks.Count; i++)
        {
            if (allChunks[i].axisX == AxisX && allChunks[i].axisZ == AxisZ)
            {
                deleteThisChunk(AxisX, AxisZ, allChunks[i]);
                break;
            }
        }
    }

    public void recreateChunk(int AxisX, int AxisZ, InfoAboutArrays chunk, int size)
    {
        for (int i = -(size / 2); i < size - (size / 2); i++)
        {
            for (int j = -(size / 2); j < size - (size / 2); j++)
            {
                int h = 0;

                for (int k = 0; k < chunk.matrixOfPlitsAndWater[i + (size / 2), j + (size / 2)]; k++)
                {
                    UnityEngine.Vector3 position = new UnityEngine.Vector3(j + AxisX * size, k * 0.0625F, i + AxisZ * size);
                    h = k + 1;
                    GameObject block = Instantiate(blockPrefabFirst, position, UnityEngine.Quaternion.identity);
                    chunk.treesWaterAndPlits.Add(block);
                }

                UnityEngine.Vector3 positionSecond = new UnityEngine.Vector3(j + AxisX * size, h * 0.0625F, i + AxisZ * size);
                GameObject blockSecond = Instantiate(blockPrefabSecond, positionSecond, UnityEngine.Quaternion.identity);
                chunk.treesWaterAndPlits.Add(blockSecond);

                if (h < 16)
                {
                    GameObject newWater = Instantiate(Water, new UnityEngine.Vector3(j + AxisX * size, 16 * 0.0625F, i + AxisZ * size), UnityEngine.Quaternion.identity);
                    newWater.transform.Rotate(90, 0, 0);
                    chunk.treesWaterAndPlits.Add(newWater);
                }

                regenerateMobsTreesAndGoo(i, j, h, chunk, size, AxisX, AxisZ);

            }
        }
    }


    public void createChunk(int AxisX, int AxisZ)
    {
        int size = 17;
        bool flag = false;
        for (int i = 0; i < allChunks.Count; i++)
        {
            if (allChunks[i].axisX == AxisX && allChunks[i].axisZ == AxisZ)
            {
                recreateChunk(AxisX, AxisZ, allChunks[i], size);
                flag = true;
                break;
            }
        }

        if (!flag)
        {
            System.Random newRandom = new System.Random();
            InfoAboutArrays newChunk = new InfoAboutArrays();
            float roughness;

            if (AxisX == 0 && AxisZ == 0)
            {
                roughness = (float)newRandom.NextDouble();
                newChunk.grubost = roughness;
            }
            else
            {
                roughness = allChunks[0].grubost + ((float)newRandom.NextDouble() / 10) * Math.Max(AxisX, AxisZ);
                newChunk.grubost = roughness;
            }


            DiamondSquareAlgorithm diamondSquare = new DiamondSquareAlgorithm(size);
            float[,] heightMap = improveHeightMap(size, diamondSquare.GenerateHeightMap(roughness));
            int[,] probablyTreesAndMobs = improveMatrixWithTreesAndMobs(size, createMatrixOfLocationMobsandTrees(heightMap, size, newRandom));

            newChunk.matrixOfPlitsAndWater = heightMap;
            newChunk.matrixOfTrees = probablyTreesAndMobs;
            newChunk.axisX = AxisX;
            newChunk.axisZ = AxisZ;

            for (int i = -(size / 2); i < size - (size / 2); i++)
            {
                for (int j = -(size / 2); j < size - (size / 2); j++)
                {
                    int h = 0;
                    for (int k = 0; k < heightMap[i + (size / 2), j + (size / 2)]; k++)
                    {
                        UnityEngine.Vector3 position = new UnityEngine.Vector3(j + AxisX * size, k * 0.0625F, i + AxisZ * size);
                        h = k + 1;
                        GameObject block = Instantiate(blockPrefabFirst, position, UnityEngine.Quaternion.identity);
                        newChunk.treesWaterAndPlits.Add(block);
                    }

                    UnityEngine.Vector3 positionSecond = new UnityEngine.Vector3(j + AxisX * size, h * 0.0625F, i + AxisZ * size);
                    GameObject blockSecond = Instantiate(blockPrefabSecond, positionSecond, UnityEngine.Quaternion.identity);
                    newChunk.treesWaterAndPlits.Add(blockSecond);

                    if (h < 16)
                    {
                        GameObject newWater = Instantiate(Water, new UnityEngine.Vector3(j + AxisX * size, 16 * 0.0625F, i + AxisZ * size), UnityEngine.Quaternion.identity);
                        newWater.transform.Rotate(90, 0, 0);
                        newChunk.treesWaterAndPlits.Add(newWater);
                    }

                    generateMobsTreesAndGoo(i, j, h, probablyTreesAndMobs, size, heightMap, newChunk, AxisX, AxisZ);

                }
            }

            allChunks.Add(newChunk);
        }
    }
    void Start()
    {
        createChunk(0, 0);
        createChunk(0, 1);
        createChunk(0, -1);
        createChunk(1, 0);
        createChunk(1, 1);
        createChunk(1, -1);
        createChunk(-1, 0);
        createChunk(-1, 1);
        createChunk(-1, -1);
    }

    void Update()
    {
        float positionGamerY = GameObject.Find("Goo").transform.position.y;
        float positionGamerX = GameObject.Find("Goo").transform.position.x;

        int numberOfChunkX = ((int)positionGamerX - ((int)positionGamerX % 17)) / 17; // 0
        int numberOfChunkY = ((int)positionGamerY - ((int)positionGamerY % 17)) / 17; // 0

        int nearX = ((int)(positionGamerX-17) - ((int)(positionGamerX-17) % 17)) / 17; // -1
        int nearY = ((int)(positionGamerY - 17) - ((int)(positionGamerY - 17) % 17)) / 17;// -1

        /*if (numberOfChunkX - 1 == nearX && numberOfChunkY - 1 == nearY)
        {
            createChunk(numberOfChunkX - 1, numberOfChunkY - 1);
        }
        else
        {
            deleteChunk(nearX, nearY);
        }
        
        if (numberOfChunkX + 1 == -nearX &&  numberOfChunkY + 1 == -nearY)
        {
            createChunk(numberOfChunkX + 1, numberOfChunkY + 1);
        }
        else
        {
            deleteChunk(nearX, nearY);
        }

        if (numberOfChunkX - 1 == nearX && numberOfChunkY + 1 == -nearY)
        {
            createChunk(numberOfChunkX - 1, numberOfChunkY + 1);
        }
        else
        {
            deleteChunk(nearX, nearY);
        }

        if (numberOfChunkX + 1 == -nearX && numberOfChunkY - 1 == nearY)
        {
            createChunk(numberOfChunkX + 1, numberOfChunkY - 1);
        }
        else
        {
            deleteChunk(nearX, nearY);
        }

        if (numberOfChunkY - 1 == nearY)
        {
            createChunk(numberOfChunkX, numberOfChunkY - 1);
        }
        else
        {
            deleteChunk(nearX, nearY);
        }

        if (numberOfChunkX - 1 == nearX)
        {
            createChunk(numberOfChunkX - 1, numberOfChunkY);
        }
        else
        {
            deleteChunk(nearX, nearY);
        }

        if (numberOfChunkY + 1 == -nearY)
        {
            createChunk(numberOfChunkX, numberOfChunkY + 1);
        }
        else
        {
            deleteChunk(nearX, nearY);
        }

        if (numberOfChunkX + 1 == -nearX)
        {
            createChunk(numberOfChunkX + 1, numberOfChunkY);
        }
        else
        {
            deleteChunk(nearX, nearY);
        }*/


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
