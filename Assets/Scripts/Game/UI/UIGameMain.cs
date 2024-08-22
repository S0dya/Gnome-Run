using AdsSystem;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

using Random = UnityEngine.Random;

public class UIGameMain : SubjectMonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject gameMenuUIElement;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI curLevelText;

    [Header("Tutorial")]
    [SerializeField] private RectTransform tutorialTransform;
    [SerializeField] private float tutorialDistance = -170f;

    [Header("Settings")]
    [SerializeField] private Image vibrationImage;
    [SerializeField] private GameObject vibrationField;
    [SerializeField] private Image soundImage;
    [SerializeField] private Image languageImage;

    [SerializeField] private Sprite[] vibrationSprites;
    [SerializeField] private Sprite[] soundSprites;
    [SerializeField] private Sprite[] languageSprites;

    [Header("Shop")]
    [SerializeField] private UIShopCharacter[] charactersVisual;

    [Header("Other")]
    [SerializeField] private GameObject SettingsUIObj;
    [SerializeField] private GameObject ShopUIObj;

    private AdsManager _adsManager;
    private LanguageManager _languageManager;
    private AudioManager _audioManager;
    private Player _player;
    private LevelManager _levelManager;

    private Vector3 _tutorialInitialPos;
    private Tween _tutorialTweener;

    private int _curShopCharacterI = -1;

    [Inject]
    public void Construct(AdsManager adsManager, LanguageManager languageManager, AudioManager audioManager, Player player, LevelManager levelManager)
    {
        _adsManager = adsManager;
        _languageManager = languageManager;
        _audioManager = audioManager;
        _player = player;
        _levelManager = levelManager;
    }

    public void Init()
    {
        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.LevelStarted, OnStartLevel},
            { EventEnum.LevelRestarted, OnRestartLevel},
            { EventEnum.LanguageChanged, OnLanguageChanged},
        });

        //menu
        SetMoney(); SetLevel();

        //tutorial
        _tutorialInitialPos = tutorialTransform.anchoredPosition;
        AnimateTutorial();

#if UNITY_WEBGL
        vibrationField.SetActive(false);
#endif

        //settings
        SetSettingImage(vibrationImage, vibrationSprites, Settings.HasVibration);
        SetSettingImage(soundImage, soundSprites, Settings.HasSound);
        if (Settings.LanguageIndex != -1) SetSettingLanguageImage();

        //shop
        foreach (int i in Settings.ShopUnlockedCharacters)
            charactersVisual[i].UnlockCharacter();
        OnSelectCharacter(Settings.CurCharacterI);
    }

    //buttons
    public void OnPressedToStartButton() => Observer.OnHandleEvent(EventEnum.LevelStarted);
    public void OnShopButton()
    {
        ShopUIObj.SetActive(true);

        Observer.OnHandleEvent(EventEnum.ShopOpened);
    }
    public void OnSettingsButton() => SettingsUIObj.SetActive(true);

    //shop buttons
    public void OnSelectCharacter(int i)
    {
        if (_curShopCharacterI == i) return;

        if (_curShopCharacterI != -1)
        {
            charactersVisual[_curShopCharacterI].DeselectCharacter();
            _player.DeselectCharacter();
        }
        Settings.CurCharacterI = _curShopCharacterI = i;
        charactersVisual[_curShopCharacterI].SelectCharacter();

        _player.SetCharacter();
        _levelManager.SetCharacter();
    }
    public void OnShopBuyButton()
    {
        var lockedCharactersList = Enumerable.Range(0, charactersVisual.Length).ToList();
        foreach (int unlockedCharacterI in Settings.ShopUnlockedCharacters) lockedCharactersList.Remove(unlockedCharacterI);
        if (lockedCharactersList.Count == 0 || Settings.MoneyAmount < 10000) return;

        Settings.MoneyAmount -= 10000; SetMoney();

        int randomI = lockedCharactersList[charactersVisual.Length > 1 ? 0 : Random.Range(0, charactersVisual.Length)];

        Settings.ShopUnlockedCharacters.Add(randomI);
        charactersVisual[randomI].UnlockCharacter();
    }
    public void OnShopWatchAdButton() =>_adsManager.ShowRewardAd(OnShopRewardAdWatched);
    public void OnCloseShopButton()
    {
        ShopUIObj.SetActive(false);

        Observer.OnHandleEvent(EventEnum.ShopClosed);
    }

    //settings buttons
    public void OnSettingsVibrationButton()
    {
        Settings.HasVibration = !Settings.HasVibration;

        SetSettingImage(vibrationImage, vibrationSprites, Settings.HasVibration);
    }
    public void OnSettingsSoundButton()
    {
        Settings.HasSound = !Settings.HasSound;

        SetSettingImage(soundImage, soundSprites, Settings.HasSound);

        _audioManager.ToggleSound(Settings.HasSound);
    }
    public void OnSettingsLanguageButton()
    {
        if (_languageManager.ChangeLanguageIfPossible()) SetSettingLanguageImage();
    }
    public void OnCloseSettingsButton()
    {
        SettingsUIObj.SetActive(false);
    }

    //main methods
    public void SetMoney() => moneyText.text = Math.Min(Settings.MoneyAmount, 999999).ToString();
    public void SetLevel() =>
        curLevelText.text = _languageManager.GetLocalizedString("Level") + " " + Settings.CurrentLevel.ToString();

    public void RerwardPlayerForWatchingAd()
    {
        Settings.MoneyAmount += 1000; SetMoney();
    }


    //actions

    //shop
    private void OnShopRewardAdWatched()
    {
        Settings.MoneyAmount += 2000; SetMoney();
    }

    //events
    private void OnStartLevel()
    {
        StopTutorial();

        gameMenuUIElement.SetActive(false);
    }
    private void OnRestartLevel()
    {
        SetLevel();

        gameMenuUIElement.SetActive(true);

        AnimateTutorial();
    }
    private void OnLanguageChanged()
    {
        SetLevel();
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
    private void StopTutorial() => _tutorialTweener?.Kill();

    private void SetSettingImage(Image image, Sprite[] sprites, bool value) => image.sprite = sprites[value ? 1 : 0];
    private void SetSettingLanguageImage() => languageImage.sprite = languageSprites[Settings.LanguageIndex];
}
