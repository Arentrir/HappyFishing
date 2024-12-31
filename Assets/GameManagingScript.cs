using UnityEngine;
using UnityEngine.UI;

public class GameManagingScript : MonoBehaviour {
    public Image ChargeBarImage;
    public RectTransform ChargeBar;
    public RectTransform RodRotation;
    public RectTransform Bobber;
    public RectTransform FishingRodSlot;
    public float LineCastTime;
    public float ElapsedTime = 0;
    public AnimationCurve movementCurve;
    public Canvas Canvas;
    public GameObject PreMinigameScreen;
    public SlideText PreMinigameText;

    // States
    [Header("States")]
    public bool isCharging;
    public bool isMoving;
    public bool isWaiting;
    public bool isIdle;    // REMEMBER!!! Set isIdle to True after player catches a fish!
    public bool isFishReady;
    public bool isMinigaming;


    public float ChargeCounter;
    public float BiteCounter;
    public float BiteTime;
    public float ChargeCounter2;
    public float FishFumble;
    public float FishRunTimer;
    public Gradient ChargeBarGradient;

    public void CastLine()
    {
        Debug.Log("Line Cast");
    }

    public void StartCharging()
    {
        if (!isMoving && !isWaiting && !isMinigaming && !isFishReady)
        {
            if (ElapsedTime > ChargeCounter2 && !isWaiting)
            {
                isCharging = true;
                isIdle = false;
                isMinigaming = false;
                isWaiting = false;
                Bobber.SetParent(FishingRodSlot);
                Bobber.anchoredPosition = Vector2.zero;
            }
        }
    }

    public static float MapRange(float value, float sourceMin, float sourceMax, float targetMin, float targetMax)
    {
        return targetMin + (targetMax - targetMin) * ((value - sourceMin) / (sourceMax - sourceMin));
    }


    public void StopCharging() // 565 Max / -65 Min (BOBBER X-AXIS)
    {
        if (!isMoving && !isWaiting && !isMinigaming && !isFishReady)
        {      
            if (ChargeCounter > 0.35f)
            {
                BiteCounter = 0;
                BiteTime = 0.75f + Random.Range(0.75f, 2.5f);
                ChargeCounter2 = ChargeCounter;
                LineCastTime = MapRange(ChargeCounter, 0, 1.1f, -65, 550);
                // Bobber.SetParent(Canvas.transform);
                ElapsedTime = 0;
            } 
            else
            {
                if (!isMoving && !isWaiting)
                {
                  isIdle = true;
                }
                isWaiting = false;
            }
            isCharging = false;
            ChargeCounter = 0;
            ChargeBar.sizeDelta = new Vector2(ChargeCounter * 100, ChargeBar.rect.height);
            ChargeBarImage.color = ChargeBarGradient.Evaluate(ChargeCounter);
            RodRotation.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void Start()
    {
        isIdle = true;
        isCharging = false;
        isMinigaming = false;
        isFishReady = false;
        FishFumble = 10;
        ElapsedTime = 10;
        ChargeCounter2 = 0;
    }


    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            // slow down time from 1 too 0.5
            Time.timeScale = .05f;
        }
        else
        {
            Time.timeScale = 1;
        }

        // Calculates where the Bobber moves to
        if (ElapsedTime <= ChargeCounter2 && isCharging == false)
        {
            // Increment elapsed time
            ElapsedTime += Time.deltaTime;
            isMoving = true;

            // Calculate normalized time (t)
            float normalizedTime = Mathf.Clamp01(ElapsedTime / ChargeCounter2);

            // Evaluate the curve to get the adjusted t value
            float t = movementCurve.Evaluate(normalizedTime);

            // Interpolate position using the adjusted t value
            float x = Mathf.Lerp(0, LineCastTime, t);
            float y = Mathf.Lerp(0, -100, t);


            Bobber.anchoredPosition = new Vector2(x, y);

        }
        else 
        { 
            isMoving = false;
            if (!isIdle && !isCharging && !isMoving && !isMinigaming && !isFishReady) 
            {
                isWaiting = true;
            }
        }

        // ChargeBarImage.color 

        if (isCharging && ChargeCounter < 1.1f) 
        {
            ChargeCounter += Time.deltaTime;
            ChargeBar.sizeDelta = new Vector2(ChargeCounter * 100, ChargeBar.rect.height);
            ChargeBarImage.color = ChargeBarGradient.Evaluate(ChargeCounter);
            RodRotation.rotation = Quaternion.Euler(0, 0, ChargeCounter * 90);
        }

        if (!isIdle && !isCharging && !isMoving && !isMinigaming && !isFishReady) 
        {

            BiteCounter += Time.deltaTime;
            if (BiteCounter > BiteTime) 
            {
                Debug.Log("Fish on the Line!");
                isFishReady = true;
                FishFumble = 0;
                isWaiting = false;
                BiteCounter = 0;
                PreMinigameScreen.SetActive(true);
                PreMinigameText.StartSliding();
            }
        }
        if (isFishReady && FishFumble < FishRunTimer) 
        {
            FishFumble += Time.deltaTime;
        }
        if (isFishReady && FishFumble > FishRunTimer) 
        {
            isFishReady = false;   
            isIdle = true;
            Bobber.anchoredPosition = Vector2.zero;
            PreMinigameScreen.SetActive(false);
            Debug.Log("Fish ran away!");
            // FAIL ANIMATION TRIGGERS HERE!!!
        }
    }
    public void StartMinigame() 
    {
        PreMinigameScreen.SetActive(false);
        isFishReady = false;
        isMinigaming = true;
    }
}
