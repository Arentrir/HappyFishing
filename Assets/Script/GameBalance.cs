using UnityEngine;

public class GameBalance : MonoBehaviour
{
    [Header("Barrier Health")]
    public float BarrierClicksMultiplier;

    [Header("Click Strength")]
    public float ClickMultiplier;
    public int BaseClickStrengthUpgradePrice;
    public float ClickStrengthUpgradePriceMultiplier;
    public int MaxRodStrengthUpgrade;

    [Header("Fish Chance")]
    public float FishChanceMultiplier;
    public int BaseFishChanceUpgradePrice;
    public float FishChanceUpgradePriceMultiplier;
    public int MaxFishChanceUpgrade;

    [Header("Auto Cast")]
    public bool hasPurchasedAutoCast;
    public int BaseAutoCastUpgradePrice;

    [Header("Map1")]
    public bool hasPurchasedMap1;
    public int BaseMap1UpgradePrice;

    [Header("Map2")]
    public bool hasPurchasedMap2;
    public int BaseMap2UpgradePrice;

    [Header("Map3")]
    public bool hasPurchasedMap3;
    public int BaseMap3UpgradePrice;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
