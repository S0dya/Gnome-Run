using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using Random = UnityEngine.Random;

public class UIGameMain : SubjectMonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject gameMenuUIElement;
    [SerializeField] private TextMeshProUGUI moneyText;

    [Header("Tutorial")]
    [SerializeField] private RectTransform tutorialTransform;
    [SerializeField] private float tutorialDistance = -170f;

    [Header("Settings")]
    [SerializeField] private CanvasGroup SettingsVibrationCG;
    [SerializeField] private CanvasGroup SettingsSoundCG;
    [SerializeField] private CanvasGroup SettingsMusicCG;

    [Header("Shop")]
    [SerializeField] private UIShopCharacter[] charactersVisual;

    [Header("Other")]
    [SerializeField] private GameObject SettingsUIObj;
    [SerializeField] private GameObject ShopUIObj;

    private Vector3 _tutorialInitialPos;
    private Tween _tutorialTweener;

    private int _curShopCharacterI;

    private void Awake()
    {

        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.LevelStarted, OnStartLevel},
            { EventEnum.LevelRestarted, OnRestartLevel},
        });
    }
    public void Init()
    {
        //menu
        SetMoney();

        //tutorial
        _tutorialInitialPos = tutorialTransform.anchoredPosition;
        AnimateTutorial();

        //shop
        foreach (int i in Settings.ShopUnlockedCharacters)
        {
            charactersVisual[i].UnlockCharacter();
        }
        _curShopCharacterI = Settings.CurCharacterI;
    }

    //buttons
    public void OnPressedToStartButton()
    {
        Observer.OnHandleEvent(EventEnum.LevelStarted);
    }
    public void OnShopButton()
    {
        ShopUIObj.SetActive(true);

        Observer.OnHandleEvent(EventEnum.ShopOpened);
    }
    public void OnSettingsButton()
    {
        SettingsUIObj.SetActive(true);
    }

    //shop buttons
    public void OnSelectCharacter(int i)
    {
        if (_curShopCharacterI == i) return;

        charactersVisual[_curShopCharacterI].DeselectCharacter();
        _curShopCharacterI = i;
        charactersVisual[_curShopCharacterI].SelectCharacter();
    }
    public void OnShopBuyButton()
    {
        int randomI = Random.Range(0, charactersVisual.Length);

        Settings.ShopUnlockedCharacters.Add(randomI);
        charactersVisual[randomI].UnlockCharacter();

    }
    public void OnShopWatchAdButton()
    {
        //sdk._RewardShow(1);
    }
    public void OnCloseShopButton()
    {
        ShopUIObj.SetActive(false);

        Observer.OnHandleEvent(EventEnum.ShopClosed);
    }

    //settings buttons
    public void OnSettingsVibrationButton()
    {

    }
    public void OnSettingsSoundButton()
    {

    }
    public void OnSettingsMusicButton()
    {

    }
    public void OnCloseSettingsButton()
    {
        SettingsUIObj.SetActive(false);
    }

    //main methods
    public void SetMoney()
    {
        moneyText.text = Settings.MoneyAmount.ToString();
    }

    public void RerwardPlayerForWatchingAd()
    {
        Settings.MoneyAmount += 1000; SetMoney();
    }

    //events
    private void OnStartLevel()
    {
        StopTutorial();

        gameMenuUIElement.SetActive(false);
    }
    private void OnRestartLevel()
    {
        gameMenuUIElement.SetActive(true);

        AnimateTutorial();
    }

    //other
    private void AnimateTutorial()
    {
        StopTutorial();

        tutorialTransform.anchoredPosition = _tutorialInitialPos;

        _tutorialTweener = tutorialTransform.DOAnchorPosX(tutorialDistance, 1)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
    private void StopTutorial()
    {
        _tutorialTweener?.Kill();
    }
}
