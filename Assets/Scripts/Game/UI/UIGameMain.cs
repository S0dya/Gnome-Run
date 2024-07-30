using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGameMain : SubjectMonoBehaviour
{
    [SerializeField] private GameObject gameMenuUIElement;
    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private RectTransform tutorialTransform;
    [SerializeField] private float tutorialDistance = -170f;

    Tween _tutorialTweener;

    Vector3 _tutorialInitialPos;


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
        SetMoney();

        _tutorialInitialPos = tutorialTransform.anchoredPosition;
        AnimateTutorial();
    }

    public void SetMoney()
    {
        moneyText.text = GameManager.MoneyAmount.ToString();
    }

    public void OnPressedToStartButton()
    {
        Observer.OnHandleEvent(EventEnum.LevelStarted);
    }

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
