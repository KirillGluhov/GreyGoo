using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimalInfo
{
    public Transform orientation;
    public string typeOfAnimal;
}

public class aliveAnimal
{
    public GameObject animal;
    public string typeOfAnimal;
}

public class InfoAboutArrays : MonoBehaviour
{
    public float[,] matrixOfPlitsAndWater = new float[17,17];
    public int[,] matrixOfTrees = new int[17,17];
    public List<aliveAnimal> localAnimals = new List<aliveAnimal>();
    public int axisX;
    public int axisZ;
    public float grubost;
    public List<AnimalInfo> InformationAboutAnimal = new List<AnimalInfo>();
    public List<GameObject> treesWaterAndPlits = new List<GameObject>();

}
