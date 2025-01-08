using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class FishInventoryDisplay : MonoBehaviour
{
    public StoredFish ThisFish;
    public Image ThisFishImage;
    public Image ThisFishBackgroundOutline;
    public Image DetailsImageSlot;
    public TMP_Text DetailsFishNameSlot;
    public TMP_Text DetailsFishWeightSlot;
    public TMP_Text DetailsFishPriceSlot;
    public TMP_Text DetailsFishFlavourTextSlot;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ThisFishImage.sprite = Fish.GetItemData(ThisFish.storedFishID).image;
        UnityEngine.ColorUtility.TryParseHtmlString(GetRarityColor(ThisFish.storedFishRarity), out Color color);
        ThisFishBackgroundOutline.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InspectButton()
    {
        // Assign image
        DetailsImageSlot.sprite = ThisFishImage.sprite;
        Color newColor = DetailsImageSlot.color;
        newColor.a = 1;
        DetailsImageSlot.color = newColor;

        // Get the fish data
        Fish fishData = Fish.GetItemData(ThisFish.storedFishID);

        if (fishData == null)
        {
            Debug.LogError("Fish data not found for ID: " + ThisFish.storedFishID);
            return;
        }

        // Set fish name with color and stars
        int rarity = ThisFish.storedFishRarity; // Assuming "rarity" is a property in Fish
        string rarityColor = GetRarityColor(rarity);
        string stars = new string('★', rarity);

        DetailsFishNameSlot.text = $"<color={rarityColor}>{fishData.fishName} {stars}</color>";

        // Set other details
        DetailsFishWeightSlot.text = "Weight: " + ThisFish.storedFishWeight.ToString();
        DetailsFishPriceSlot.text = "Price: " + ThisFish.storedFishPrice.ToString();
        DetailsFishFlavourTextSlot.text = fishData.flavourText;
    }

    private string GetRarityColor(int rarity)
    {
        switch (rarity)
        {
            case 0: return "#6EC7FF"; // LIGHT BLUE
            case 1: return "#585FFF"; // DarkBlue
            case 2: return "#E95950"; // Red
            case 3: return "#FFD700"; // Gold
            case 4: return "#FF00E9"; // Purple
            default: return "#FFFFFF"; // Default to White
        }
    }

}
