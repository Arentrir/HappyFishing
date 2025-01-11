using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class FishSellDisplay : MonoBehaviour
{
    public StoredFish ThisFish;
    public Image ThisFishImage;
    public Image ThisFishBackground;
    public Image ThisFishBackgroundOutline;
    public InventorySell inventorySell;
    public TMP_Text sellPrice;

    public bool isFishSelected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isFishSelected = false;

        ThisFishImage.sprite = Fish.GetItemData(ThisFish.storedFishID).image;
        UnityEngine.ColorUtility.TryParseHtmlString(FishInventoryDisplay.GetRarityColor(ThisFish.storedFishRarity), out Color color);
        ThisFishBackgroundOutline.color = color;
        sellPrice.text = "$" + InventorySell.CalculatePrice(Fish.GetItemData(ThisFish.storedFishID).price, ThisFish.storedFishWeight, Fish.GetItemData(ThisFish.storedFishID).weight, ThisFish.storedFishRarity);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SelectSellFish()
    {
        if (!isFishSelected)
        {
            ThisFishBackground.color = Color.gray;
            inventorySell.selectedFishList.Add(ThisFish);
            isFishSelected = true;
        }
        else 
        {
            ThisFishBackground.color = Color.white;
            inventorySell.selectedFishList.Remove(ThisFish);
            isFishSelected = false;
        }
        inventorySell.updateToBeSoldMoney();
    }

}
