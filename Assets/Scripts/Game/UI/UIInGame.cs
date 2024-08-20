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

    private LanguageManager _languageManager;

    private Color _curPlayerPregressBarColor;

    [Inject]
    public void Construct(LanguageManager languageManager)
    {
        _languageManager = languageManager;
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

    public void SetCurMoneyAmountAndBar(int moneyAmount, float barValue)
    {
        curMoneyText.text = moneyAmount.ToString();

        SetProgressBar(barValue);
    }


    public void SetNewStatus(int index)
    {
        playerStatusTextPopUp.AnimateNewStatus(_curPlayerPregressBarColor, GetLocalizedString(playerProgressStatuses[index]));

        SetStatus(index);
    }
    public void SetStatus(int index) => playerProgressBarText.text = GetLocalizedString(playerProgressStatuses[index]);

    public void OnMoneyCollected(int value)
    {
        if (value > 0)
            GoodMoneyCollect.AnimateCollect(value);
        else
            BadMoneyCollect.AnimateCollect(value);
    }

    private void SetProgressBar(float value)
    {
        playerProgressBarImage.fillAmount = value;

        _curPlayerPregressBarColor = playerProgressBarColor.Evaluate(value);

        playerProgressBarImage.color = playerProgressBarText.color = _curPlayerPregressBarColor;
    }
    private void OnStartLevel()
    {
        ToggleInGameElements(true);

        curLevelText.text = GetLocalizedString("Level") + " " + Settings.CurrentLevel.ToString();
    }
    private void OnFinishLevel() => ToggleInGameElements(false);

    private string GetLocalizedString(string str) => _languageManager.GetLocalizedString(str);

    private void ToggleInGameElements(bool toggle)
    {
        foreach (var element in inGameUIElements) element.SetActive(toggle);
    }
}
