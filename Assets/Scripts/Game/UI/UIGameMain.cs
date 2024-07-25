using ButchersGames;
using TMPro;
using UnityEngine;

public class UIGameMain : MonoBehaviour
{
    [SerializeField] private GameObject gameMenuUIElement;
    [SerializeField] private TextMeshProUGUI moneyText;


    public void Init()
    {
        LevelManager.Default.OnLevelStarted += OnStartLevel;

        LevelManager.Default.OnLevelRestarted += OnRestartLevel;

        SetMoney();
    }

    public void SetMoney()
    {
        //moneyText.text = GameManager.MoneyAmount.ToString();
    }

    public void OnPressedToStartButton()
    {
        LevelManager.Default.StartLevel();
    }

    private void OnStartLevel()
    {
        gameMenuUIElement.SetActive(false);
    }
    private void OnRestartLevel()
    {
        gameMenuUIElement.SetActive(true);
    }
}
