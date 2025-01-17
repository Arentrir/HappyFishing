using System;
using System.Collections.Generic;
using System.Linq;
using Radishmouse;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Barrier
{
    public int Slot;
    public int Clicks;
    public GameObject BarrierObject;
}

public class Minigame : MonoBehaviour
{
    public FilledTrapezoid ProgressBar;
    public FilledTrapezoid FailBar;
    public GameObject ClickBarrier;
    public MoveScreen FailMoveScreen;
    public MoveScreen StartMoveScreen;
    public MoveScreen SuccessMoveScreen;
    public GameManagingScript GameManagingScript;
    public FishDisplay ChooseFish;
    public GameObject FishPrefab;
    private GameObject FishSlot;
    public InventoryStorage InventoryStorage;
    public GameBalance gameBalance;
    public TMP_Text DebugText;
    public RectTransform Spinner;


    [Range(0, 100)]
    public float Progress;
    public float ReelTime;
    public float Offset;
    public float BarrierHit;
    public float CurrentClicks;
    public List<Barrier> BarrierList;
    public float FailTime;
    public float HeadStartTime;
    public float FailSpeed;
    public bool isFishFail;
    public float FailTextUptime;
    public float FailTextTimer;
    public bool isFishCaught;
    
    public float[] RarityPercentages;


    public FishDisplay GenerateFish()
    {
        // Getting a random fish from the possible fish in current map
        int CurrentMap = InventoryStorage.CurrentMap;
        int RandomFishIndex = UnityEngine.Random.Range(0, gameBalance.AllFishMaps[CurrentMap].Length);
        Fish selectedFish = Fish.GetItemData(gameBalance.AllFishMaps[CurrentMap][RandomFishIndex]);     

        FishDisplay fishToReturn = new FishDisplay();
        fishToReturn.fish = selectedFish;
        //fishToReturn.dynamicWeight = selectedFish.weight 

        // Calculate the cumulative sum of weights
        float cumulativeSum = 0f;
        float randomNumber = 0;
        float totalPercentages = 0;
        int[] adjustedRarityPercentages = new int[RarityPercentages.Length];

        for (int i = 0; i < adjustedRarityPercentages.Length; i++)
        {
            adjustedRarityPercentages[i] = Mathf.RoundToInt(RarityPercentages[i] + ((i + 1) * gameBalance.FishChance));
        }

        for (int i = 0; i < adjustedRarityPercentages.Length; i++)
        {
            totalPercentages += adjustedRarityPercentages[i];
        }
        randomNumber = UnityEngine.Random.Range(0, totalPercentages);

        for (int i = 0; i < adjustedRarityPercentages.Length; i++)
        {
            cumulativeSum += adjustedRarityPercentages[i];
            if (randomNumber < cumulativeSum)
            {
                fishToReturn.rarity = i;
                break;
            }
        }
        fishToReturn.dynamicWeight = GetRandomNumber(selectedFish.weight, fishToReturn.rarity + 1, selectedFish.weight / 3, selectedFish.weight * 5);
        return fishToReturn; 
    } 

    public void StartMinigame() 
    {
        isFishFail = false;
        isFishCaught = false;
        Progress = 0;
        FailTime = 0 - (28.5f * HeadStartTime);
        FailTextTimer = 0;
        StartMoveScreen.StartSliding();
        ProgressBar.SetPoints(
                new Vector3(20, 150, -15),  // Top-left
                new Vector3(20, 150, -15),   // Top-right
                new Vector3(35, 150, -15), // Bottom-right
                new Vector3(35, 150, -15) // Bottom-left
            );

        FailBar.SetPoints(
                new Vector3(20, 150, -16),  // Top-left
                new Vector3(20, 150, -16),   // Top-right
                new Vector3(35, 150, -16), // Bottom-right
                new Vector3(35, 150, -16) // Bottom-left
            );
        ChooseFish = GenerateFish();
        int minimumAmountBarriers = 1;
        if ((ChooseFish.rarity + 1) > 3)
        {
            minimumAmountBarriers = 2;
        }
        SpawnRandomObjects(UnityEngine.Random.Range(minimumAmountBarriers, 6),
            Mathf.RoundToInt(Mathf.Lerp(1, 25, ChooseFish.rarity / 4f)),
            Mathf.RoundToInt(Mathf.Lerp(5, 125, ChooseFish.rarity / 4f))
            );
    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
       // StartMinigame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFishCaught)
        {
            FailTime += Time.deltaTime * (100 / FailSpeed);
        }
        if (FailTime > Progress && !isFishFail) 
        {
            isFishFail = true;
            FailMoveScreen.StartSliding();
        }
        if (!isFishFail && !isFishCaught)
        {
            if (FailTime > 0)
            {
                float t = FailTime / 100f;

                FailBar.trapezoidPoints[0] = Vector3.Lerp(new Vector3(20, 150, -16), new Vector3(5, 0, -16), t);
                FailBar.trapezoidPoints[3] = Vector3.Lerp(new Vector3(35, 150, -16), new Vector3(50, 0, -16), t);

                FailBar.UpdateMesh();
            }
            // if (!(Progress > BarrierHit))
            // {
            //     if (Input.GetKey(KeyCode.Mouse0) && Progress < 100)
            //     {
            //         HoldMinigame();
            //     }
            // }
            // else
            // {
            //     if (Input.GetKeyDown(KeyCode.Mouse0))
            //     {
            //         ClickMinigame();
            //     }
            // }
        }
        else 
        {
            if (!isFishCaught)
            {
                FailTextTimer += Time.deltaTime;
                if (FailTextTimer > FailTextUptime)
                {
                    foreach (var barrier in BarrierList)
                    {
                        Destroy(barrier.BarrierObject);
                    }
                    GameManagingScript.ResetFishing();
                    this.gameObject.SetActive(false);
                }
            }
           
        }
    }

    public void ExtractCaughtFishToInventory()
    {
        isFishCaught = false;
        Destroy(FishSlot);
        GameManagingScript.ResetFishing();
        this.gameObject.SetActive(false);
        StoredFish tempFish = new StoredFish();
        tempFish.storedFishID = ChooseFish.fish.ID;
        tempFish.storedFishWeight = (float)System.Math.Round(ChooseFish.dynamicWeight, 2);
        tempFish.storedFishRarity = ChooseFish.rarity;


        InventoryStorage.storedFishList.Add(tempFish);
        InventoryStorage.SaveFishList();
        // Add Current Chosen Fish to the Inventory.
    }

    public void FailToFishResetClick()
    {
        if (FailMoveScreen.slideDuration < FailTextTimer)
        {
            FailTextTimer += FailTextUptime;
        }
    }

    public void HoldMinigame()
    {
        Progress += Time.deltaTime * (100 / ReelTime);
        float t = Progress / 100f;



        float newZRotation = Spinner.localEulerAngles.z + 400 * Time.deltaTime;

        // Apply the rotation
        Vector3 newRotation = Spinner.localEulerAngles;
        newRotation.z = newZRotation;
        Spinner.localEulerAngles = newRotation;

       // Spinner.localEulerAngles = new Vector3(0, 0, Spinner.rotation.z + (Time.deltaTime * 180));



        ProgressBar.trapezoidPoints[0] = Vector3.Lerp(new Vector3(20, 150, -15), new Vector3(5, 0, -15), t);
        ProgressBar.trapezoidPoints[3] = Vector3.Lerp(new Vector3(35, 150, -15), new Vector3(50, 0, -15), t);

        ProgressBar.UpdateMesh();
        if (Progress >= 100)                // Instantiates Caught Fish Object, sets the object above the InputPanel
        {
            isFishCaught = true;
            SuccessMoveScreen.StartSliding();
            GameObject CaughtFish = Instantiate(FishPrefab, new Vector3(0, 0, 0), Quaternion.identity, this.transform.parent);
            CaughtFish.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
            CaughtFish.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 33.5f, -100);
            CaughtFish.GetComponent<FishDisplay>().fish = ChooseFish.fish;
            CaughtFish.GetComponent<FishDisplay>().rarity = ChooseFish.rarity;
            CaughtFish.GetComponent<FishDisplay>().dynamicWeight = ChooseFish.dynamicWeight;
            CaughtFish.GetComponent<ScaleChange>().StartScaling();
            FishSlot = CaughtFish;
        }
    }

    public void ClickMinigame()
    {
        CurrentClicks -= gameBalance.ClickStrength;
        TMP_Text ClickText = BarrierList[0].BarrierObject.GetComponentInChildren<TMP_Text>();
        ClickText.text = CurrentClicks.ToString();
        if (CurrentClicks <= 0)
        {
            Destroy(BarrierList[0].BarrierObject);                                       // Destroying the current Barrier GameObject
            Progress += 3;
            BarrierList.RemoveAt(0);                                                     // Removing the Reference of that Barrier from the list
            if (BarrierList.Count <= 0)
            {
                BarrierHit = 101;
            }
            else
            {
                BarrierHit = (10 + (BarrierList[0].Slot * 8)) - 3;                            // New Barrier Limit
                CurrentClicks = BarrierList[0].Clicks;                                  // Getting how many clicks required from the lowest Barrier

            }
        }
    }

    public void SpawnRandomObjects(int HowMany, int MinClicks, int MaxClicks)
    {
        Vector2 StartPoint = new Vector2(25 - (Offset * 2), 150);
        Vector2 EndPoint = new Vector2(5 - (Offset * 2) + 5, 0);

        Vector2 StartPoint1 = new Vector2(35 + Offset, 150);
        Vector2 EndPoint1 = new Vector2(50 + Offset, 0);

        // Generate a list of slot indices
        List<Barrier> slotIndices = new List<Barrier>();
        for (int i = 0; i < 11; i++)
        {
            slotIndices.Add(new Barrier { Slot = i, Clicks = UnityEngine.Random.Range(MinClicks, MaxClicks) });
        }

        // Shuffle the slot indices
        for (int i = slotIndices.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            Barrier temp = slotIndices[i];
            slotIndices[i] = slotIndices[randomIndex];
            slotIndices[randomIndex] = temp;
        }

        // Select a random number of slots
        int numberOfObjects = Mathf.Min(HowMany, slotIndices.Count);
        List<Barrier> selectedSlots = slotIndices.GetRange(0, numberOfObjects);

        // Ensure no duplicate slots (debugging only)
        HashSet<int> usedSlots = new HashSet<int>();
        foreach (Barrier barrier in selectedSlots)
        {
            if (!usedSlots.Add(barrier.Slot))
            {
                Debug.LogError($"Duplicate Slot detected: {barrier.Slot}");
            }
        }

        // Spawn objects at the selected slots
        foreach (Barrier slotIndex in selectedSlots)
        {
            float t = ((slotIndex.Slot * 8f) + 10f) / 100f;

            // Calculate spawn positions
            Vector2 pointA = Vector2.Lerp(StartPoint, EndPoint, t);
            Vector2 pointB = Vector2.Lerp(StartPoint1, EndPoint1, t);

            // Set spawn position (adjust if necessary)
            Vector3 spawnPosition = new Vector3(0, 0, 0);

            // Get the first child of this GameObject
            Transform FirstChild = transform.GetChild(0);

            // Instantiate the object and set its points
            GameObject SpawnedObject = Instantiate(ClickBarrier, spawnPosition, Quaternion.identity, FirstChild);
            SpawnedObject.GetComponent<UILineRenderer>().points = new Vector2[]
            {
            pointA,
            pointB
            };

            slotIndex.BarrierObject = SpawnedObject;

            TMP_Text ClickText = SpawnedObject.GetComponentInChildren<TMP_Text>();
            ClickText.text = slotIndex.Clicks.ToString();
            ClickText.rectTransform.anchoredPosition = new Vector3(-57 + pointA.x, pointA.y + 8, 0);
            SpawnedObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, -30);
        }

        BarrierList = selectedSlots.OrderBy(b => b.Slot).ToList();
        BarrierHit = (10 + (BarrierList[0].Slot * 8)) - 3;
        CurrentClicks = BarrierList[0].Clicks;
    }

    public float GetRandomNumber(float baseWeight, int rarity, float min, float max)
    {
        // Map rarity (1-5) to standard deviation
        // Higher rarity -> Larger standard deviation
        float stddev = Mathf.Lerp(5f, 20f, (rarity - 1) / 4f);

        // Adjust the mean with a smaller bias to max for higher rarities
        // This ensures low weights remain possible but less likely as rarity increases
        float adjustedMean = Mathf.Lerp(baseWeight, (baseWeight + max) / 2f, (rarity - 1) / 4f);

        // Generate two random numbers between 0 and 1
        float u1 = UnityEngine.Random.value;
        float u2 = UnityEngine.Random.value;

        // Box-Muller transform to generate a standard normal distribution (mean = 0, stddev = 1)
        float standardNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);

        // Adjust the distribution using adjusted mean and rarity-based standard deviation
        float result = adjustedMean + standardNormal * stddev;

        // Clamp the result to the specified range
        return Mathf.Clamp(result, min, max);
    }




}
