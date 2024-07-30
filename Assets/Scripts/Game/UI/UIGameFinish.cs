using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class UIGameFinish : SubjectMonoBehaviour
{
    [SerializeField] private UILevelFinish victoryFinish;
    [SerializeField] private UILevelFinish gameoverFinish;

    [SerializeField] private TextMeshProUGUI moneyAmountText;
    [SerializeField] private TextMeshProUGUI levelIndexText;

    private LevelManager _levelManager;

    [Inject]
    public void Constuct(LevelManager levelManager)
    {
        _levelManager = levelManager;
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

    public void OnNextLevelButton()
    {
        Observer.OnHandleEvent(EventEnum.LevelRestarted);

        _levelManager.NextLevel();

        victoryFinish.Close();
    }
    public void OnRestartGameButton()
    {
        Observer.OnHandleEvent(EventEnum.LevelRestarted);

        gameoverFinish.Close();
    }


    private void OnFinishLevelVictory()
    {
        victoryFinish.Open();
    }
    private void OnFinishLevelGameover()
    {
        gameoverFinish.Open();
    }

}
