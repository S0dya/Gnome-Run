using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using ButchersGames;

public class UIInGame : MonoBehaviour
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

    private Color _curPlayerPregressBarColor;


    public void Init()
    {
        LevelManager.Default.OnLevelStarted += OnStartLevel;
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
        foreach (var element in inGameUIElements)
        {
            element.SetActive(true);
        }
    }
}
