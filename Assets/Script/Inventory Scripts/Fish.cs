using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Fish", menuName = "Scriptable Objects/Fish")]
public class Fish : ScriptableObject
{
    public int ID;
    public string fishName;
    public Sprite image;
    public float weight;
    public int price;
    public string flavourText;

    private static bool initialized = false;
    private static Dictionary<int, Fish> itemDataDictionary = new Dictionary<int, Fish>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeItemDataDictionary()
    {
        if (initialized)
            return;

        initialized = true;

        // Load all Fish assets from the "ItemData" folder in Resources
        Fish[] fishArray = Resources.LoadAll<Fish>("ItemData");

        foreach (Fish fish in fishArray)
        {
            if (itemDataDictionary.ContainsKey(fish.ID))
            {
                Debug.LogError($"Duplicate Fish ID found: {fish.ID}");
            }
            else
            {
                itemDataDictionary.Add(fish.ID, fish);
            }
        }

        Debug.Log($"Loaded {itemDataDictionary.Count} fish into the dictionary.");
    }

    // Method to retrieve Fish by ID
    public static Fish GetItemData(int itemID)
    {
        if (itemDataDictionary.TryGetValue(itemID, out Fish fish))
        {
            return fish;
        }
        else
        {
            Debug.LogError($"Fish with ID {itemID} not found.");
            return null;
        }
    }
}