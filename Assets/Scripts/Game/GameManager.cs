using UnityEngine;
using System;
using Zenject;
using ButchersGames;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxMoneyAmountOnLevel;
    [SerializeField] private int currentMoneyAmount;

    [SerializeField] private int[] curMoneyGoals;
    [SerializeField] private int curGoalIndex;

    const string MoneyAmount_PrefsKey = "Money Amount";
    public static int MoneyAmount { get { return PlayerPrefs.GetInt(MoneyAmount_PrefsKey); } set { PlayerPrefs.SetInt(MoneyAmount_PrefsKey, value); } }

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
    public void Init()
    {
        LevelManager.Default.OnLevelStarted += OnStartLevel;

        LevelManager.Default.OnLevelFinishedVictory += OnFinishLevelVictory;
        LevelManager.Default.OnLevelFinishedGameover += OnFinishLevelGameover;
    }

    public void SetPlayerSpawnPosition(Vector3 position)
    {
        _player.transform.position = position;
    }

    public void ChangeMoneyAmount(int value)
    {
        bool setsNewStatus = false;

        if (value > 0)
        {
            currentMoneyAmount = Math.Min(maxMoneyAmountOnLevel, currentMoneyAmount + value);

            if (curGoalIndex < curMoneyGoals.Length - 1 && currentMoneyAmount >= curMoneyGoals[curGoalIndex + 1])
            {
                curGoalIndex++;
                setsNewStatus = true;
            }
        }
        else
        {
            currentMoneyAmount = Math.Max(0, currentMoneyAmount + value);

            if (curGoalIndex > 0 && currentMoneyAmount <= curMoneyGoals[curGoalIndex - 1])
            {
                curGoalIndex--;
            }
        }

        if (currentMoneyAmount == 0)
        {
            LevelManager.Default.FinishLevelGameover();
        }

        _uiInGame.OnMoneyCollected(value);
        _uiInGame.SetCurMoneyAmount(currentMoneyAmount);
        _uiInGame.SetProgressBar((float)currentMoneyAmount / (float)maxMoneyAmountOnLevel);
        if (setsNewStatus) _uiInGame.SetNewStatus(curGoalIndex);
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

                LevelManager.Default.FinishLevelVictory();
            }
            else LevelManager.Default.FinishLevelGameover();
        }

        return finishReached;
    }


    public void OnReciveMoney()
    {
        MoneyAmount += currentMoneyAmount;

        _uiGameMain.SetMoney();
    }


    private void OnStartLevel()
    {
        _player.enabled = true;
    }

    private void OnFinishLevelVictory()
    {
        _player.enabled = false;
    }
    private void OnFinishLevelGameover()
    {
        _player.enabled = false;
    }
}
