using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySell : MonoBehaviour
{
    public int soldMoney;


    public GameObject InventoryContent;
    public GameObject FishSellPrefab;
    public List<StoredFish> selectedFishList;
    public TMP_Text toBeSoldMoney;
    public TMP_Text shopDisplayMoney;

    public InventoryStorage inventoryStorage;


    void Start()
    {
        selectedFishList = new List<StoredFish>();

    }

    void Update()
    {
        
    }


    public void RefreshSellInventory()
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
            if (inventoryStorage.storedFishList.Count > 0)
            {
                foreach (StoredFish storedFish in inventoryStorage.storedFishList) 
                {
                    GameObject SpawnInventoryFish = Instantiate(FishSellPrefab, InventoryContent.transform);
                    FishSellDisplay fishSellDisplay = SpawnInventoryFish.GetComponent<FishSellDisplay>();
                    fishSellDisplay.ThisFish = storedFish;
                    fishSellDisplay.inventorySell = this;
                }
            }
            selectedFishList.Clear();
        toBeSoldMoney.text = "$0";
    }

    public void updateToBeSoldMoney()
    {
        int tempMoney = 0;
        if (selectedFishList.Count > 0)
        {
            foreach (StoredFish storedFish in selectedFishList)
            {
                tempMoney += CalculatePrice(Fish.GetItemData(storedFish.storedFishID).price, storedFish.storedFishWeight, Fish.GetItemData(storedFish.storedFishID).weight, storedFish.storedFishRarity); 
            }
        }
        else
        {
            tempMoney = 0;
        }
        toBeSoldMoney.text = "$" + tempMoney;
    }

    public void UpdateShopMoney()
    {
        shopDisplayMoney.text = "$" + inventoryStorage.money;
    }

    public void SellButton()
    {
        soldMoney = 0;
        if (selectedFishList.Count > 0)
        {
            foreach (StoredFish storedFish in selectedFishList)
            {
                soldMoney += CalculatePrice(Fish.GetItemData(storedFish.storedFishID).price, storedFish.storedFishWeight, Fish.GetItemData(storedFish.storedFishID).weight, storedFish.storedFishRarity);
            }
            foreach (StoredFish storedFish in selectedFishList)
            {
                inventoryStorage.storedFishList.Remove(storedFish);
            }
            selectedFishList.Clear();
         inventoryStorage.money += soldMoney;
        }
        soldMoney = 0;
        UpdateShopMoney();
    }

    public static int CalculatePrice(int basePrice, float dynamicWeight, float baseWeight, int rarity)
    {
        float weightRatio = dynamicWeight / baseWeight;

        int finalPrice = Mathf.RoundToInt(basePrice * weightRatio * (rarity + 1));

        return finalPrice;
    }
}