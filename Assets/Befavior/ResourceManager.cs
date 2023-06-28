using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager instance;
    public static ResourceManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ResourceManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("ResourceManager");
                    instance = obj.AddComponent<ResourceManager>();
                }
            }
            return instance;
        }
    }

    private Dictionary<string, int> resources = new Dictionary<string, int>();

    public void AddResource(string resourceName, int amount)
    {
        if (resources.ContainsKey(resourceName))
        {
            resources[resourceName] += amount;
        }
        else
        {
            resources.Add(resourceName, amount);
        }

        Debug.Log("Collected " + amount + " " + resourceName + ". Total " + resources[resourceName]);
    }

    public void UseResource(string resourceName, int amount)
    {
        if (resources.ContainsKey(resourceName))
        {
            if (resources[resourceName] >= amount)
            {
                resources[resourceName] -= amount;
                Debug.Log("Used " + amount + " " + resourceName + ". Remaining " + resources[resourceName]);
            }
            else
            {
                Debug.LogWarning("Not enough " + resourceName + " to use.");
            }
        }
        else
        {
            Debug.LogWarning("Resource " + resourceName + " not found.");
        }
    }
}
