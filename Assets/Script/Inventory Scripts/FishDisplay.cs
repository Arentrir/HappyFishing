using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishDisplay : MonoBehaviour {

    public Fish fish;

    public TMP_Text fishName;
    public TMP_Text fishWeight;

    public Image fishImage;
    public RectTransform RarityBackgroundSpinner;
    public int rarity;
    public float dynamicWeight;
    public float rarityRotationSpeed;


    void Start()
    {
        fishName.text = fish.fishName;
        fishWeight.text = "Weight: " + dynamicWeight.ToString("f2");
        fishImage.sprite = fish.image;
        UnityEngine.ColorUtility.TryParseHtmlString(FishInventoryDisplay.GetRarityColor(rarity), out Color color);
        RarityBackgroundSpinner.GetComponent<Image>().color = color;
    }
    public void Update() 
    {
        RarityBackgroundSpinner.localEulerAngles += new Vector3(0, 0, Time.deltaTime * rarityRotationSpeed);
    }
}
