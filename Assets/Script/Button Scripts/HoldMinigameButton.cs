using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldMinigameButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public Minigame miniGame;

    private bool isHolding = false;

    void Update()
    {
        if (isHolding)
        {
          MinigameHolding();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        MinigameClicking();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHolding = false;
    }

    public void MinigameClicking()
    {
        if (!miniGame.isFishFail && !miniGame.isFishCaught)
        {
            if (miniGame.Progress > miniGame.BarrierHit)
            {
                miniGame.ClickMinigame();
            }
        }
    }

    public void MinigameHolding()
    {
        if (!miniGame.isFishFail && !miniGame.isFishCaught)
        {
            if (!(miniGame.Progress > miniGame.BarrierHit))
            {
                miniGame.HoldMinigame();
            }
        }
    }
}
