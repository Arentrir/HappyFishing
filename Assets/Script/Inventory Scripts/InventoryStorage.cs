using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System;

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
    public GameBalance gameBalance;
    public InventorySell inventorySell;

    public Image DetailsImageSlot;
    public TMP_Text DetailsFishNameSlot;
    public TMP_Text DetailsFishWeightSlot;
    public TMP_Text DetailsFishPriceSlot;
    public TMP_Text DetailsFishFlavourTextSlot;

    public Image UpgradeImageSlot;
    public TMP_Text UpgradeNameSlot;
    public TMP_Text UpgradeDescriptionSlot;
    public TMP_Text UpgradePriceSlot;
    public Button UpgradeBuyButton;

    public Image RodStrengthUpgradeImage;
    public Image FishChanceUpgradeImage;
    public Image AutoCastUpgradeImage;
    public Image Map1Image;
    public Image Map2Image;
    public Image Map3Image;

    private bool isRodStrengthSelected;
    private bool isFishChanceSelected;
    private bool isAutoCastSelected;
    private bool isMap1Selected;
    private bool isMap2Selected;
    private bool isMap3Selected;

    public Image RodStrengthBackground;
    public Image FishChanceBackground;
    public Image AutoCastBackground;
    public Image Map1Background;
    public Image Map2Background;
    public Image Map3Background;

    public TMP_Text RodStrengthPrice;
    public TMP_Text FishChancePrice;
    public TMP_Text AutoCastPrice;
    public TMP_Text Map1Price;
    public TMP_Text Map2Price;
    public TMP_Text Map3Price;

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
        UpdateAllUpgradePrices();
        LoadUpgradeList();
        Screen.autorotateToLandscapeLeft = true;
        UpgradeImageSlot.sprite = null;
        Color newColor1 = UpgradeImageSlot.color;
        newColor1.a = 0;
        UpgradeImageSlot.color = newColor1;
        UpgradeNameSlot.text = "";
        UpgradeDescriptionSlot.text = "";
        UpgradePriceSlot.text = "";
        UpgradeBuyButton.gameObject.SetActive(false);
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
    public void UpdateAllUpgradePrices()
    {
        if (upgradeList[0] < gameBalance.MaxRodStrengthUpgrade)
        {
            RodStrengthPrice.text = "$" + Mathf.RoundToInt(gameBalance.BaseClickStrengthUpgradePrice * ((upgradeList[0] + 1) * gameBalance.ClickStrengthUpgradePriceMultiplier)).ToString();
        }
        else
        {
            RodStrengthPrice.text = "MAX";
        }
        if (upgradeList[1] < gameBalance.MaxFishChanceUpgrade)
        {
            FishChancePrice.text = "$" + Mathf.RoundToInt(gameBalance.BaseFishChanceUpgradePrice * ((upgradeList[1] + 1) * gameBalance.FishChanceUpgradePriceMultiplier)).ToString();
        }
        else
        {
            FishChancePrice.text = "MAX";
        }
        if (upgradeList[2] < 1)
        {
            AutoCastPrice.text = "$" + gameBalance.BaseAutoCastUpgradePrice.ToString();
        }
        else
        {
            AutoCastPrice.text = "MAX";
        }
        if (upgradeList[3] < 1)
        {
            Map1Price.text = "$" + gameBalance.BaseMap1UpgradePrice.ToString();
        }
        else
        {
            Map1Price.text = "MAX";
        }
        if (upgradeList[4] < 1)
        {
            Map2Price.text = "$" + gameBalance.BaseMap2UpgradePrice.ToString();
        }
        else
        {
            Map2Price.text = "MAX";
        }
        if (upgradeList[5] < 1)
        {
            Map3Price.text = "$" + gameBalance.BaseMap3UpgradePrice.ToString();
        }
        else
        {
            Map3Price.text = "MAX";
        }
    }

    // Rod Strength
    public void SelectRodPowerUpgrade()
    {
        UpgradeImageSlot.sprite = RodStrengthUpgradeImage.sprite;
        UpdateDetailsToRodPower();
        UpgradeBuyButton.onClick.RemoveAllListeners();
        UpgradeBuyButton.onClick.AddListener(() => SetBuyButtonRodPowerUpgrade());
        UnselectAllUpgrades();
        RodStrengthBackground.color = Color.gray;
        UpgradeBuyButton.gameObject.SetActive(true);
        Color newColor1 = UpgradeImageSlot.color;
        newColor1.a = 1;
        UpgradeImageSlot.color = newColor1;
    }

    public void UpdateDetailsToRodPower()
    {
        if (upgradeList[0] < gameBalance.MaxRodStrengthUpgrade)
        {
            UpgradeNameSlot.text = "Rod Strength Lvl " + (upgradeList[0] + 1);
            UpgradeDescriptionSlot.text = "Increase Click Strength from \n" + (upgradeList[0] * gameBalance.ClickMultiplier) + " > " + ((upgradeList[0] + 1) * gameBalance.ClickMultiplier);
            UpgradePriceSlot.text = "$" + Mathf.RoundToInt(gameBalance.BaseClickStrengthUpgradePrice * ((upgradeList[0] + 1) * gameBalance.ClickStrengthUpgradePriceMultiplier));
        }
        else
        {
            UpgradeNameSlot.text = "Rod Strength Lvl MAX";
            UpgradeDescriptionSlot.text = "Click Strength is " + (upgradeList[0] * gameBalance.ClickMultiplier) + " (MAX)";
            UpgradePriceSlot.text = "MAXED";
        }
    }

    public void SetBuyButtonRodPowerUpgrade()
    {
        int upgradePrice = Mathf.RoundToInt(gameBalance.BaseClickStrengthUpgradePrice * ((upgradeList[0] + 1) * gameBalance.ClickStrengthUpgradePriceMultiplier));
        if (money > upgradePrice && upgradeList[0] < gameBalance.MaxRodStrengthUpgrade)
        {
            money -= upgradePrice;
            upgradeList[0]++;
            SaveMoney();
            SaveUpgradeList();
            inventorySell.UpdateShopMoney();
            UpdateDetailsToRodPower();
        }
    }

    // Fish Chance
    public void SelectFishChanceUpgrade()
    {
        UpgradeImageSlot.sprite = FishChanceUpgradeImage.sprite;
        UpdateDetailsToFishChance();
        UpgradeBuyButton.onClick.RemoveAllListeners();
        UpgradeBuyButton.onClick.AddListener(() => SetBuyButtonFishChanceUpgrade());
        UnselectAllUpgrades();
        FishChanceBackground.color = Color.gray;
        UpgradeBuyButton.gameObject.SetActive(true);
        Color newColor1 = UpgradeImageSlot.color;
        newColor1.a = 1;
        UpgradeImageSlot.color = newColor1;
    }

    public void UpdateDetailsToFishChance()
    {
        if (upgradeList[1] < gameBalance.MaxFishChanceUpgrade)
        {
            UpgradeNameSlot.text = "Fish Chance Lvl " + (upgradeList[1] + 1);
            UpgradeDescriptionSlot.text = "Increase Fish Chance from " + (upgradeList[1] * gameBalance.FishChanceMultiplier) + " > " + ((upgradeList[1] + 1) * gameBalance.FishChanceMultiplier);
            UpgradePriceSlot.text = "$" + Mathf.RoundToInt(gameBalance.BaseFishChanceUpgradePrice * ((upgradeList[1] + 1) * gameBalance.FishChanceUpgradePriceMultiplier));
        }
        else
        {
            UpgradeNameSlot.text = "Fish Chance Lvl MAX";
            UpgradeDescriptionSlot.text = "Fish Chance is " + (upgradeList[1] * gameBalance.FishChanceMultiplier) + " (MAX)";
            UpgradePriceSlot.text = "MAXED";
        }
    }

    public void SetBuyButtonFishChanceUpgrade()
    {
        int upgradePrice = Mathf.RoundToInt(gameBalance.BaseFishChanceUpgradePrice * ((upgradeList[1] + 1) * gameBalance.FishChanceUpgradePriceMultiplier));
        if (money > upgradePrice && upgradeList[1] < gameBalance.MaxFishChanceUpgrade)
        {
            money -= upgradePrice;
            upgradeList[1]++;
            SaveMoney();
            SaveUpgradeList();
            inventorySell.UpdateShopMoney();
            UpdateDetailsToFishChance();
        }
    }

    // Auto Cast
    public void SelectAutoCastUpgrade()
    {
        UpgradeImageSlot.sprite = AutoCastUpgradeImage.sprite;
        UpdateDetailsToAutoCast();
        UpgradeBuyButton.onClick.RemoveAllListeners();
        UpgradeBuyButton.onClick.AddListener(() => SetBuyButtonAutoCastUpgrade());
        UnselectAllUpgrades();
        AutoCastBackground.color = Color.gray;
        UpgradeBuyButton.gameObject.SetActive(true);
        Color newColor1 = UpgradeImageSlot.color;
        newColor1.a = 1;
        UpgradeImageSlot.color = newColor1;
    }

    public void UpdateDetailsToAutoCast()
    {
        if (upgradeList[2] < 1)
        {
            UpgradeNameSlot.text = "Auto Cast Upgrade";
            UpgradeDescriptionSlot.text = "Enables automatic casting of the fishing rod";
            UpgradePriceSlot.text = "$" + gameBalance.BaseAutoCastUpgradePrice;
        }
        else
        {
            UpgradeNameSlot.text = "Auto Cast MAX";
            UpgradeDescriptionSlot.text = "Auto Cast Upgrade is Purchased!";
            UpgradePriceSlot.text = "MAXED";
        }
    }

    public void SetBuyButtonAutoCastUpgrade()
    {
        int upgradePrice = gameBalance.BaseAutoCastUpgradePrice;
        if (money > upgradePrice && upgradeList[2] < 1)
        {
            money -= upgradePrice;
            upgradeList[2]++;
            SaveMoney();
            SaveUpgradeList();
            inventorySell.UpdateShopMoney();
            UpdateDetailsToAutoCast();
        }
    }

    // Map 1
    public void SelectMap1Upgrade()
    {
        UpgradeImageSlot.sprite = Map1Image.sprite;
        UpdateDetailsToMap1();
        UpgradeBuyButton.onClick.RemoveAllListeners();
        UpgradeBuyButton.onClick.AddListener(() => SetBuyButtonMap1Upgrade());
        UnselectAllUpgrades();
        Map1Background.color = Color.gray;
        UpgradeBuyButton.gameObject.SetActive(true);
        Color newColor1 = UpgradeImageSlot.color;
        newColor1.a = 1;
        UpgradeImageSlot.color = newColor1;
    }

    public void UpdateDetailsToMap1()
    {
        if (upgradeList[3] < 1)
        {
            UpgradeNameSlot.text = "Map1";
            UpgradeDescriptionSlot.text = "Allows you to travel to Map1";
            UpgradePriceSlot.text = "$" + gameBalance.BaseMap1UpgradePrice;
        }
        else
        {
            UpgradeNameSlot.text = "Map1 PURCHASED";
            UpgradeDescriptionSlot.text = "Map1 is Purchased!";
            UpgradePriceSlot.text = "PURCHASED";
        }
    }

    public void SetBuyButtonMap1Upgrade()
    {
        int upgradePrice = gameBalance.BaseMap1UpgradePrice;
        if (money > upgradePrice && upgradeList[3] < 1)
        {
            money -= upgradePrice;
            upgradeList[3]++;
            SaveMoney();
            SaveUpgradeList();
            inventorySell.UpdateShopMoney();
            UpdateDetailsToMap1();
        }
    }

    // Map 2
    public void SelectMap2Upgrade()
    {
        UpgradeImageSlot.sprite = Map2Image.sprite;
        UpdateDetailsToMap2();
        UpgradeBuyButton.onClick.RemoveAllListeners();
        UpgradeBuyButton.onClick.AddListener(() => SetBuyButtonMap2Upgrade());
        UnselectAllUpgrades();
        Map2Background.color = Color.gray;
        UpgradeBuyButton.gameObject.SetActive(true);
        Color newColor2 = UpgradeImageSlot.color;
        newColor2.a = 1;
        UpgradeImageSlot.color = newColor2;
    }

    public void UpdateDetailsToMap2()
    {
        if (upgradeList[4] < 1)
        {
            UpgradeNameSlot.text = "Map2";
            UpgradeDescriptionSlot.text = "Allows you to travel to Map2";
            UpgradePriceSlot.text = "$" + gameBalance.BaseMap2UpgradePrice;
        }
        else
        {
            UpgradeNameSlot.text = "Map2 PURCHASED";
            UpgradeDescriptionSlot.text = "Map2 is Purchased!";
            UpgradePriceSlot.text = "PURCHASED";
        }
    }

    public void SetBuyButtonMap2Upgrade()
    {
        int upgradePrice = gameBalance.BaseMap2UpgradePrice;
        if (money > upgradePrice && upgradeList[4] < 1)
        {
            money -= upgradePrice;
            upgradeList[4]++;
            SaveMoney();
            SaveUpgradeList();
            inventorySell.UpdateShopMoney();
            UpdateDetailsToMap2();
        }
    }

    // Map 3
    public void SelectMap3Upgrade()
    {
        UpgradeImageSlot.sprite = Map3Image.sprite;
        UpdateDetailsToMap3();
        UpgradeBuyButton.onClick.RemoveAllListeners();
        UpgradeBuyButton.onClick.AddListener(() => SetBuyButtonMap3Upgrade());
        UnselectAllUpgrades();
        Map3Background.color = Color.gray;
        UpgradeBuyButton.gameObject.SetActive(true);
        Color newColor3 = UpgradeImageSlot.color;
        newColor3.a = 1;
        UpgradeImageSlot.color = newColor3;
    }

    public void UpdateDetailsToMap3()
    {
        if (upgradeList[5] < 1)
        {
            UpgradeNameSlot.text = "Map3";
            UpgradeDescriptionSlot.text = "Allows you to travel to Map3";
            UpgradePriceSlot.text = "$" + gameBalance.BaseMap3UpgradePrice;
        }
        else
        {
            UpgradeNameSlot.text = "Map3 PURCHASED";
            UpgradeDescriptionSlot.text = "Map3 is Purchased!";
            UpgradePriceSlot.text = "PURCHASED";
        }
    }

    public void SetBuyButtonMap3Upgrade()
    {
        int upgradePrice = gameBalance.BaseMap3UpgradePrice;
        if (money > upgradePrice && upgradeList[5] < 1)
        {
            money -= upgradePrice;
            upgradeList[5]++;
            SaveMoney();
            SaveUpgradeList();
            inventorySell.UpdateShopMoney();
            UpdateDetailsToMap3();
        }
    }

    public void UnselectAllUpgrades()
    {
        isRodStrengthSelected = false;
        isFishChanceSelected = false;
        isAutoCastSelected = false;
        isMap1Selected = false;
        isMap2Selected = false;
        isMap3Selected = false;
        RodStrengthBackground.color = Color.white;
        FishChanceBackground.color = Color.white;
        AutoCastBackground.color = Color.white;
        Map1Background.color = Color.white;
        Map2Background.color = Color.white;
        Map3Background.color = Color.white;
    }

    public void SaveUpgradeList()
    {
        // Convert the int array to a comma-separated string
        string arrayString = string.Join(",", upgradeList);

        // Save the string to PlayerPrefs
        PlayerPrefs.SetString("UpgradeList", arrayString);

        // Save PlayerPrefs to ensure it's written
        PlayerPrefs.Save();
        UpdateAllUpgradePrices();
    }
    public void LoadUpgradeList()
    {
        // Get the string from PlayerPrefs
        string arrayString = PlayerPrefs.GetString("UpgradeList", "0,0,0,0,0,0");

        // Split the string into an array of strings
        string[] stringArray = arrayString.Split(',');

        // Convert the string array to an int array
        upgradeList = Array.ConvertAll(stringArray, int.Parse);

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
        string json = PlayerPrefs.GetString("StoredFishList1235jerma");
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