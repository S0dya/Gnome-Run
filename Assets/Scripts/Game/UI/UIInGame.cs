using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Zenject;

public class UIInGame : SubjectMonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private GameObject[] inGameUIElements;

    [Header("Head")]
    [SerializeField] private TextMeshProUGUI curLevelText;
    [SerializeField] private TextMeshProUGUI curMoneyText;

    [Header("Progress")]
    [SerializeField] private TextMeshProUGUI playerProgressBarText;
    [SerializeField] private string[] playerProgressStatuses;
    [SerializeField] private Image playerProgressBarImage;
    [SerializeField] private Gradient playerProgressBarColor;

    [Header("Pop up")]
    [SerializeField] private UITextPopUp playerStatusTextPopUp;

    [SerializeField] private UIMoneyCollect GoodMoneyCollect;
    [SerializeField] private UIMoneyCollect BadMoneyCollect;

    [Header("Other")]
    [SerializeField] private GameObject PauseUIObj;

    private LevelManager _levelManager;
    
    private Color _curPlayerPregressBarColor;

    [Inject]
    public void Construct(LevelManager levelManager)
    {
        _levelManager = levelManager;
    }

    private void Awake()
    {

        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.LevelStarted, OnStartLevel},
            { EventEnum.LevelFinishedVictory, OnFinishLevel},
            { EventEnum.LevelFinishedGameover, OnFinishLevel},
            { EventEnum.LevelRestarted, OnFinishLevel},
        });
    }

    //buttons
    public void OnPauseButton()
    {
        Time.timeScale = 0;
        PauseUIObj.SetActive(true);
    }
    public void OnPauseResumeButton()
    {
        PauseUIObj.SetActive(false);
        Time.timeScale = 1;
    }
    public void OnPauseLeaveButton()
    {
        Observer.OnHandleEvent(EventEnum.LevelRestarted);

        OnPauseResumeButton();
    }

    public void SetCurLevelIndex(int value)
    {
        curLevelText.text = value.ToString();
    }
    public void SetCurMoneyAmountAndBar(int moneyAmount, float barValue)
    {
        curMoneyText.text = moneyAmount.ToString();

        SetProgressBar(barValue);
    }


    public void SetNewStatus(int index)
    {
        playerStatusTextPopUp.AnimateNewStatus(_curPlayerPregressBarColor, playerProgressStatuses[index]);

        SetStatus(index);
    }
    public void SetStatus(int index) => playerProgressBarText.text = playerProgressStatuses[index];

    public void OnMoneyCollected(int value)
    {
        if (value > 0)
            GoodMoneyCollect.AnimateCollect('+' + value.ToString());
        else
            BadMoneyCollect.AnimateCollect(value.ToString());
    }

    private void SetProgressBar(float value)
    {
        playerProgressBarImage.fillAmount = value;

        _curPlayerPregressBarColor = playerProgressBarColor.Evaluate(value);

        playerProgressBarImage.color = playerProgressBarText.color = _curPlayerPregressBarColor;
    }
    private void OnStartLevel()
    {
        SetCurLevelIndex(Settings.CurrentLevel);

        foreach (var element in inGameUIElements)
        {
            element.SetActive(true);
        }

        curLevelText.text = "Level " + _levelManager.CurrentLevelIndex.ToString();
    }
    private void OnFinishLevel()
    {
        foreach (var element in inGameUIElements)
        {
            element.SetActive(false);
        }
    }
}
