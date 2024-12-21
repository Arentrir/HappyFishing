using UnityEngine;
using UnityEngine.UI;

public class GameManagingScript : MonoBehaviour {
    public Image ChargeBarImage;
    public RectTransform ChargeBar;
    public RectTransform RodRotation;

    public bool isCharging;
    public float ChargeCounter;
    public float BiteTime;
    public Gradient ChargeBarGradient;

    public void CastLine()
    {
        Debug.Log("Line Cast");
    }

    public void StartCharging()
    {
        isCharging = true;  
    }

    public void StopCharging()
    {
        isCharging = false;
        BiteTime = ChargeCounter;
        ChargeCounter = 0;
        ChargeBar.sizeDelta = new Vector2(ChargeCounter * 100, ChargeBar.rect.height);
        ChargeBarImage.color = ChargeBarGradient.Evaluate(ChargeCounter);
        RodRotation.rotation = Quaternion.Euler(0, 0, 0);
    }

    void Start()
    {
        isCharging = false;
    }

    void FixedUpdate()
    {

        // ChargeBarImage.color 

        if (isCharging && ChargeCounter < 1.1f) 
        {
            ChargeCounter += Time.deltaTime;
            ChargeBar.sizeDelta = new Vector2(ChargeCounter * 100, ChargeBar.rect.height);
            ChargeBarImage.color = ChargeBarGradient.Evaluate(ChargeCounter);
            RodRotation.rotation = Quaternion.Euler(0, 0, ChargeCounter * 90);
        }
        if (BiteTime > 0)
        { 
            BiteTime -= Time.deltaTime;
            if (BiteTime < 0)
            {
                Debug.Log("Caught a fish!");
            }
        }
    }
}
