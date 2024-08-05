using UnityEngine;
using System;
using Zenject;
using System.Collections.Generic;

public class GameManager : SubjectMonoBehaviour
{
    [SerializeField] private int maxMoneyAmountOnLevel;
    [SerializeField] private int currentMoneyAmount;

    [SerializeField] private int[] curMoneyGoals;
    [SerializeField] private int curGoalIndex;

    private Player _player;
    private UIGameMain _uiGameMain;
    private UIInGame _uiInGame;
    private UIGameFinish _uiGameFinish;

    [Inject]
    public void Construct(Player player, UIGameMain uiGameMain, UIInGame uiInGame, UIGameFinish uiGameFinish)
    {
        _player = player;

        _uiGameMain = uiGameMain;
        _uiInGame = uiInGame;
        _uiGameFinish = uiGameFinish;
    }
    private void Awake()
    {

        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.LevelStarted, OnStartLevel},
        });
    }

    public void AssignNewLevel(int maxMoney)
    {
        maxMoneyAmountOnLevel = maxMoney;
    }


    public void ChangeMoneyAmount(int value)
    {
        HandleChangeMoney(value);

        if (currentMoneyAmount == 0)
        {
            Observer.OnHandleEvent(EventEnum.LevelFinishedGameover); return;
        }

        _uiInGame.OnMoneyCollected(value);
        _uiInGame.SetCurMoneyAmount(currentMoneyAmount);
        _uiInGame.SetProgressBar((float)currentMoneyAmount / (float)maxMoneyAmountOnLevel);
    }
    private void HandleChangeMoney(int value)
    {
        if (value > 0)
        {
            currentMoneyAmount = Math.Min(maxMoneyAmountOnLevel, currentMoneyAmount + value);

            if (curGoalIndex < curMoneyGoals.Length - 1 && currentMoneyAmount >= curMoneyGoals[curGoalIndex + 1])
            {
                curGoalIndex++;

                _player.SetStatus(curGoalIndex);
                _uiInGame.SetNewStatus(curGoalIndex);
            }
        }
        else
        {
            currentMoneyAmount = Math.Max(0, currentMoneyAmount + value);

            if (curGoalIndex > 0 && currentMoneyAmount <= curMoneyGoals[curGoalIndex - 1])
            {
                curGoalIndex--;

                _player.SetStatus(curGoalIndex);
            }
        }
    }

    public bool FinishReached(int finishIndex)
    {
        bool finishReached = curGoalIndex == finishIndex;

        if (finishReached)
        {
            if (curGoalIndex > 0)
            {
                currentMoneyAmount *= curGoalIndex + 1;
                _uiGameFinish.SetProgressOnVictory(currentMoneyAmount);

                Observer.OnHandleEvent(EventEnum.LevelFinishedVictory);
            }
            else 
                Observer.OnHandleEvent(EventEnum.LevelFinishedGameover);
        }

        return finishReached;
    }


    public void OnReciveMoney()
    {
        Settings.MoneyAmount += currentMoneyAmount;

        _uiGameMain.SetMoney();
    }


    private void OnStartLevel()
    {
        currentMoneyAmount = 0;
        curGoalIndex = 0;
    }
}
