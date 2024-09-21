using AdsSystem;
using Saving;
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
    private LanguageManager _languageManager;
    private SaveManager _saveManager;

    [Inject]
    public void Constuct(LevelManager levelManager, GameManager gameManager, AdsManager adsManager, LanguageManager languageManager, SaveManager saveManager)
    {
        _levelManager = levelManager;
        _gameManager = gameManager;
        _adsManager = adsManager;
        _languageManager = languageManager;
        _saveManager = saveManager;
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
        levelIndexText.text = _languageManager.GetLocalizedString("Level") + " " + _levelManager.CurrentLevelIndex.ToString();
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
        _gameManager.AddEarnedMoney();

        victoryFinish.Close();

        _saveManager.Save();
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

        OnNextLevelButton();
    }

    //events
    private void OnFinishLevelVictory() => victoryFinish.Open();
    private void OnFinishLevelGameover() => gameoverFinish.Open();
}
