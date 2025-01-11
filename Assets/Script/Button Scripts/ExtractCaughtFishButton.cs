using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ExtractCaughtFishButton : MonoBehaviour, IPointerDownHandler
{
    public Minigame miniGame;


    public void OnPointerDown(PointerEventData eventData)
    {
        ExtractCaughtFishClicking();
    }
    public void ExtractCaughtFishClicking()
    {
        if (!miniGame.isFishFail && miniGame.isFishCaught)
        {
            if (miniGame.isFishCaught)
            {
                miniGame.ExtractCaughtFishToInventory();

            }
        }
    }
}