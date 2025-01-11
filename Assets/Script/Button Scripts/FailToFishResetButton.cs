using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FailToFishResetButton : MonoBehaviour, IPointerDownHandler
{
    public Minigame miniGame;


    public void OnPointerDown(PointerEventData eventData)
    {
        FailedToFishResetClicking();
    }
    public void FailedToFishResetClicking()
    {
        if (miniGame.isFishFail && !miniGame.isFishCaught)
        {
            if (!miniGame.isFishCaught)
            {
                miniGame.FailToFishResetClick();
            }
        }
    }
}
