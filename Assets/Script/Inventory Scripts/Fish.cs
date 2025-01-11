using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    // Editor-only method to populate the dictionary with ItemData objects
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
    private static void InitializeItemDataDictionaryEditor()
    {
        if (initialized)
            return;

        initialized = true;

        // Adjust the path to your ItemData folder
        string folderPath = "Assets/ItemData";

        // Load all assets from the specified folder
        Fish[] itemDataArray = LoadAssetsFromFolder<Fish>(folderPath);

        foreach (Fish itemData in itemDataArray)
        {
            if (itemDataDictionary.ContainsKey(itemData.ID))
            {
                Debug.LogError("Duplicate itemID found: " + itemData.ID);
            }
            else
            {
                itemDataDictionary.Add(itemData.ID, itemData);
            }
        }
    }
#endif

    // Runtime method to populate the dictionary with ItemData objects
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeItemDataDictionary()
    {
        if (initialized)
            return;

        initialized = true;

        // Adjust the path to your ItemData folder
        string folderPath = "Assets/ItemData";

        // Load all assets from the specified folder
        Fish[] itemDataArray = LoadAssetsFromFolder<Fish>(folderPath);

        foreach (Fish itemData in itemDataArray)
        {
            if (itemDataDictionary.ContainsKey(itemData.ID))
            {
                Debug.LogError("Duplicate itemID found: " + itemData.ID);
            }
            else
            {
                itemDataDictionary.Add(itemData.ID, itemData);
            }
        }
    }

    // Helper method to load assets from a folder
    private static T[] LoadAssetsFromFolder<T>(string folderPath) where T : UnityEngine.Object
    {
        List<T> assets = new List<T>();
#if UNITY_EDITOR

        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name, new[] { folderPath });
        foreach (string guid in guids)
        {
            string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }

#endif
        return assets.ToArray();
    }

    // Method to retrieve ItemData by itemID
    public static Fish GetItemData(int itemID)
    {
        Fish itemData;
        if (itemDataDictionary.TryGetValue(itemID, out itemData))
        {
            return itemData;
        }
        else
        {
            Debug.LogError("ItemData not found for itemID: " + itemID);
            return null;
        }
    }
}




