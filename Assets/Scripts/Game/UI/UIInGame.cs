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

    [SerializeField] private TextMeshProUGUI curLevelText;
    [SerializeField] private TextMeshProUGUI curMoneyText;

    [SerializeField] private TextMeshProUGUI playerProgressBarText;
    [SerializeField] private string[] playerProgressStatuses;
    [SerializeField] private Image playerProgressBarImage;
    [SerializeField] private Gradient playerProgressBarColor;

    [SerializeField] private UITextPopUp playerStatusTestPopUp;

    [SerializeField] private UIMoneyCollect GoodMoneyCollect;
    [SerializeField] private UIMoneyCollect BadMoneyCollect;

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

    public void OnExitButton()
    {

    }

    public void SetCurLevelIndex(int value)
    {
        curLevelText.text = value.ToString();
    }
    public void SetCurMoneyAmount(int value)
    {
        curMoneyText.text = value.ToString();
    }

    public void SetProgressBar(float value)
    {
        playerProgressBarImage.fillAmount = value;

        _curPlayerPregressBarColor = playerProgressBarColor.Evaluate(value);

        playerProgressBarImage.color = playerProgressBarText.color = _curPlayerPregressBarColor;
    }

    public void SetNewStatus(int index)
    {
        playerStatusTestPopUp.AnimateNewStatus(_curPlayerPregressBarColor, playerProgressStatuses[index]);

        playerProgressBarText.text = playerProgressStatuses[index];
    }

    public void OnMoneyCollected(int value)
    {
        if (value > 0)
        {
            GoodMoneyCollect.AnimateCollect('+' + value.ToString());
        }
        else
        {
            BadMoneyCollect.AnimateCollect(value.ToString());
        }
    }

    private void OnStartLevel()
    {
        SetCurLevelIndex(0);
        SetCurMoneyAmount(0);
        SetProgressBar(0);

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
