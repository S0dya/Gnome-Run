using ButchersGames;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIGameMain : MonoBehaviour
{
    [SerializeField] private GameObject gameMenuUIElement;
    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private RectTransform tutorialTransform;
    [SerializeField] private float tutorialDistance = -170f;

    Tween _tutorialTweener;

    Vector3 _tutorialInitialPos;

    public void Init()
    {
        LevelManager.Default.OnLevelStarted += OnStartLevel;
        LevelManager.Default.OnLevelRestarted += OnRestartLevel;

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
        LevelManager.Default.StartLevel();
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
        if (_tutorialTweener != null && _tutorialTweener.IsPlaying())
            _tutorialTweener.Kill();
    }
}
