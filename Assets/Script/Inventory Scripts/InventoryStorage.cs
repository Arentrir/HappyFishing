using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class StoredFish 
{
    public int storedFishID;
    public float storedFishWeight;
    public int storedFishRarity;
}
public class InventoryStorage : MonoBehaviour
{
    public int money;
    public int[] upgradeList;
    // Upgrade Prices
    public int Rod_Power_Level_1_Price;


    public GameObject InventoryContent;
    public GameObject FishInventoryPrefab;
    public List<StoredFish> storedFishList;

    public Image DetailsImageSlot;
    public TMP_Text DetailsFishNameSlot;
    public TMP_Text DetailsFishWeightSlot;
    public TMP_Text DetailsFishPriceSlot;
    public TMP_Text DetailsFishFlavourTextSlot;


    void Start()
    {
        storedFishList = new List<StoredFish>();
        DetailsImageSlot.sprite = null;
        Color newColor = DetailsImageSlot.color;
        newColor.a = 0;
        DetailsImageSlot.color = newColor;  
        DetailsFishNameSlot.text = "";
        DetailsFishWeightSlot.text = "";
        DetailsFishPriceSlot.text = "";
        DetailsFishFlavourTextSlot.text = "";
        LoadMoney();
        LoadFishList(); 
    }

    void Update()
    {
        
    }


    public void RefreshInventory()
    {
        int children;
        children = InventoryContent.transform.childCount;
        Debug.Log(children);
        if (children >= 1)
        {
            foreach (Transform child in InventoryContent.transform)
            {
                Destroy(child.gameObject);
            }
        }
            if (storedFishList != null)
            {
                foreach (StoredFish storedFish in storedFishList) 
                {
                    GameObject SpawnInventoryFish = Instantiate(FishInventoryPrefab, InventoryContent.transform);
                    FishInventoryDisplay fishInventoryDisplay = SpawnInventoryFish.GetComponent<FishInventoryDisplay>();
                    fishInventoryDisplay.ThisFish = storedFish;
                    fishInventoryDisplay.DetailsImageSlot = DetailsImageSlot;
                    fishInventoryDisplay.DetailsFishNameSlot = DetailsFishNameSlot;
                    fishInventoryDisplay.DetailsFishWeightSlot = DetailsFishWeightSlot;
                    fishInventoryDisplay.DetailsFishPriceSlot = DetailsFishPriceSlot;
                    fishInventoryDisplay.DetailsFishFlavourTextSlot = DetailsFishFlavourTextSlot;
                }
            }
    }

    public void CheckStoreUpgrades()
    {

    }

    public void BuyLevel1RodPowerUpgrade()
    {
        if (money >= Rod_Power_Level_1_Price)
        {
            upgradeList[1] = 1;
            money -= Rod_Power_Level_1_Price;
        }

    }

    public void BuyLevel1AutoCastUpgrade()
    {
        upgradeList[0] = 1;

    }

    public void SaveMoney()
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.Save();
    }

    public void LoadMoney()
    {
        money = PlayerPrefs.GetInt("Money", 0);
    }

    public void SaveFishList()
    {
        string json = JsonConvert.SerializeObject(storedFishList);
        PlayerPrefs.SetString("StoredFishList", json);
        PlayerPrefs.Save();
    }

    public void LoadFishList()
    {
        string json = PlayerPrefs.GetString("StoredFishList");
        storedFishList = JsonConvert.DeserializeObject<List<StoredFish>>(json);
        if (storedFishList == null)
        {
            storedFishList = new List<StoredFish>();    
        }
    }

    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
}