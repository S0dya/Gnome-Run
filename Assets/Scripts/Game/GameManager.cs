using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxMoneyAmountOnLevel;
    [SerializeField] private int curMoneyAmount;

    [SerializeField] private int[] curMoneyGoals;
    [SerializeField] private int curGoalIndex;



    public void ChangeMoneyAmount(int value)
    {
        curMoneyAmount = Math.Max(0, Math.Min(maxMoneyAmountOnLevel, curMoneyAmount + value));

        if (curMoneyAmount == 0)
        {
            Debug.Log("GameOver");
        }
        else if (curMoneyAmount >= curMoneyGoals[curGoalIndex + 1])
        {
            curGoalIndex++;
            Debug.Log("New level");
        }
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
