using AdsSystem;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class UIGameFinish : SubjectMonoBehaviour
{
    [SerializeField] private UILevelFinishVictory victoryFinish;
    [SerializeField] private UILevelFinish gameoverFinish;

    [Header("Other")]
    [SerializeField] private TextMeshProUGUI moneyAmountText;
    [SerializeField] private TextMeshProUGUI levelIndexText;

    private LevelManager _levelManager;
    private GameManager _gameManager;
    private AdsManager _adsManager;

    [Inject]
    public void Constuct(LevelManager levelManager, GameManager gameManager, AdsManager adsManager)
    {
        _levelManager = levelManager;
        _gameManager = gameManager;
        _adsManager = adsManager;
    }

    private void Awake()
    {

        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.LevelFinishedVictory, OnFinishLevelVictory},
            { EventEnum.LevelFinishedGameover, OnFinishLevelGameover},
        });
    }

    public void SetProgressOnVictory(int moneyAmount)
    {
        moneyAmountText.text = moneyAmount.ToString();
        levelIndexText.text = "Level " + _levelManager.CurrentLevelIndex.ToString();
    }

    public void OnWatchRewardAdButton()
    {
        _adsManager.ShowRewardAd(OnRewardLevelFinishVictoryAdWatched);
    }
    public void OnNextLevelButton()
    {
        Observer.OnHandleEvent(EventEnum.LevelRestarted);

        if (Settings.CompleteLevelCount % 2 == 0) _adsManager.ShowAd();

        _levelManager.NextLevel();

        victoryFinish.Close();
    }
    public void OnRestartGameButton()
    {
        Observer.OnHandleEvent(EventEnum.LevelRestarted);

        gameoverFinish.Close();
    }

    //actions
    private void OnRewardLevelFinishVictoryAdWatched()
    {
        victoryFinish.ToggleRewardAdButton(false);

        _gameManager.MultiplyEarnedMoney(3);
    }

    //events
    private void OnFinishLevelVictory()
    {
        victoryFinish.Open();
    }
    private void OnFinishLevelGameover()
    {
        gameoverFinish.Open();
    }

}
