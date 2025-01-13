using UnityEngine;

public class GameBalance : MonoBehaviour
{
    [Header("Barrier Health")]
    public int BarrierClicksMultiplier;

    [Header("Click Strength")]
    public int ClickStrength;
    public float ClickMultiplier;
    public int BaseClickStrengthUpgradePrice;
    public float ClickStrengthUpgradePriceMultiplier;
    public int MaxRodStrengthUpgrade;

    [Header("Fish Chance")]
    public int FishChance;
    public float FishChanceMultiplier;
    public int BaseFishChanceUpgradePrice;
    public float FishChanceUpgradePriceMultiplier;
    public int MaxFishChanceUpgrade;

    [Header("Auto Cast")]
    public bool hasPurchasedAutoCast;
    public int BaseAutoCastUpgradePrice;

    [Header("Map0")]
    public int[] map0PossibleFish;

    [Header("Map1")]
    public bool hasPurchasedMap1;
    public int BaseMap1UpgradePrice;
    public int[] map1PossibleFish;

    [Header("Map2")]
    public bool hasPurchasedMap2;
    public int BaseMap2UpgradePrice;
    public int[] map2PossibleFish;

    [Header("Map3")]
    public bool hasPurchasedMap3;
    public int BaseMap3UpgradePrice;
    public int[] map3PossibleFish;

    public int[][] AllFishMaps;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AllFishMaps = new int[4][];
        AllFishMaps[0] = map0PossibleFish;
        AllFishMaps[1] = map1PossibleFish;
        AllFishMaps[2] = map2PossibleFish;
        AllFishMaps[3] = map3PossibleFish;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
