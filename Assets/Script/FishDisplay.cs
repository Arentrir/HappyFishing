using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishDisplay : MonoBehaviour {

    public Fish fish;

    public TMP_Text fishName;
    public TMP_Text fishWeight;

    public Image fishImage;
    void Start()
    {
        fishName.text = fish.fishName;
        fishWeight.text = "Weight: " + fish.weight.ToString();
        fishImage.sprite = fish.image;
    }
}
