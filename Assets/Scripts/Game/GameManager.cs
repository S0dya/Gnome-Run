using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxMoneyAmountOnLevel;
    [SerializeField] private int curMoneyAmount;

    [SerializeField] private int[] curMoneyGoals;
    [SerializeField] private int curGoalIndex;

    private UIInGame _uiInGame;

    [Inject]
    public void Construct(UIInGame uiInGame)
    {
        _uiInGame = uiInGame;
    }

    public void ChangeMoneyAmount(int value)
    {
        bool setsNewStatus = false;

        if (value > 0)
        {
            curMoneyAmount = Math.Min(maxMoneyAmountOnLevel, curMoneyAmount + value);

            if (curGoalIndex < curMoneyGoals.Length - 1 && curMoneyAmount >= curMoneyGoals[curGoalIndex + 1])
            {
                curGoalIndex++;
                setsNewStatus = true;
                Debug.Log("New level");
            }
        }
        else
        {
            curMoneyAmount = Math.Max(0, curMoneyAmount + value);

            if (curGoalIndex > 0 && curMoneyAmount < curMoneyGoals[curGoalIndex - 1])
            {
                curGoalIndex--;
                setsNewStatus = true;
                Debug.Log("New level");
            }
        }

        if (curMoneyAmount == 0)
        {
            Debug.Log("GameOver");
        }

        _uiInGame.OnMoneyCollected(value);
        _uiInGame.SetCurMoneyAmount(curMoneyAmount);
        _uiInGame.SetProgressBar((float)curMoneyAmount / (float)maxMoneyAmountOnLevel);
        if (setsNewStatus) _uiInGame.SetNewStatus(curGoalIndex);
    }

    public bool FinishReached(int finishIndex)
    {
        if (curGoalIndex == finishIndex)
        {
            Debug.Log("Stop game");
        }

        return curGoalIndex == finishIndex;
    }
}
