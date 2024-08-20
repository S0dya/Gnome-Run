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
    private AudioManager _audioManager;

    [Inject]
    public void Construct(Player player, UIGameMain uiGameMain, UIInGame uiInGame, UIGameFinish uiGameFinish, AudioManager audioManager)
    {
        _player = player;

        _uiGameMain = uiGameMain;
        _uiInGame = uiInGame;
        _uiGameFinish = uiGameFinish;

        _audioManager = audioManager;
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

        for (int i = 0; i < 4; i++)
            curMoneyGoals[i] = (int)((float)maxMoneyAmountOnLevel * (i + 1) * 0.25f);
    }


    public void ChangeMoneyAmount(int value)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Settings.HasVibration)
        {
            Handheld.Vibrate();
        }
#endif

        HandleChangeMoney(value);

        if (currentMoneyAmount == 0)
        {
            Observer.OnHandleEvent(EventEnum.LevelFinishedGameover); return;
        }

        _uiInGame.OnMoneyCollected(value);
        _uiInGame.SetCurMoneyAmountAndBar(currentMoneyAmount, (float)currentMoneyAmount / (float)maxMoneyAmountOnLevel);
    }
    private void HandleChangeMoney(int value)
    {
        if (value > 0)
        {
            currentMoneyAmount = Math.Min(maxMoneyAmountOnLevel, currentMoneyAmount + value);

            if (curGoalIndex < curMoneyGoals.Length - 1 && currentMoneyAmount >= curMoneyGoals[curGoalIndex + 1])
            {
                curGoalIndex++;

                _player.SetStatus(curGoalIndex); _uiInGame.SetNewStatus(curGoalIndex);

                _audioManager.PlayOneShot(SoundEventEnum.GoodCollect);
            }
        }
        else
        {
            currentMoneyAmount = Math.Max(0, currentMoneyAmount + value);

            if (curGoalIndex > 0 && currentMoneyAmount <= curMoneyGoals[curGoalIndex - 1])
            {
                curGoalIndex--;

                _player.SetStatus(curGoalIndex); _uiInGame.SetStatus(curGoalIndex);

                _audioManager.PlayOneShot(SoundEventEnum.BadCollect);
            }
        }
    }

    public bool FinishReached(int finishIndex)
    {
        bool finishReached = curGoalIndex <= finishIndex;

        if (finishReached)
        {
            if (curGoalIndex > 0)
            {
                currentMoneyAmount *= curGoalIndex + 1;
                _uiGameFinish.SetProgressOnVictory(currentMoneyAmount);

                Observer.OnHandleEvent(EventEnum.LevelFinishedVictory);
            }
            else
            {
                currentMoneyAmount = 0;

                Observer.OnHandleEvent(EventEnum.LevelFinishedGameover);
            }
        }
        else
        {
            _audioManager.PlayOneShot(SoundEventEnum.GateOpen);
        }

        return finishReached;
    }

    public void MultiplyEarnedMoney(int n)
    {
        currentMoneyAmount *= n;

        _uiGameFinish.SetProgressOnVictory(currentMoneyAmount);
    }
    public void AddEarnedMoney()
    {
        Settings.MoneyAmount += currentMoneyAmount;

        _uiGameMain.SetMoney();
    }


    private void OnStartLevel()
    {
        curGoalIndex = 1;
        currentMoneyAmount = curMoneyGoals[curGoalIndex];

        _uiInGame.SetStatus(curGoalIndex);
        _uiInGame.SetCurMoneyAmountAndBar(currentMoneyAmount, (float)currentMoneyAmount / (float)maxMoneyAmountOnLevel);
    }
}
