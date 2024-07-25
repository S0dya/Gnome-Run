using UnityEngine;
using System;
using Zenject;
using ButchersGames;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxMoneyAmountOnLevel;
    [SerializeField] private int curMoneyAmount;

    [SerializeField] private int[] curMoneyGoals;
    [SerializeField] private int curGoalIndex;

    private Player _player;
    private UIInGame _uiInGame;

    [Inject]
    public void Construct(Player player, UIInGame uiInGame)
    {
        _player = player;
        _uiInGame = uiInGame;
    }
    public void Init()
    {
        LevelManager.Default.OnLevelStarted += OnStartLevel;
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

    private void OnStartLevel()
    {
        _player.enabled = true;
    }
}
