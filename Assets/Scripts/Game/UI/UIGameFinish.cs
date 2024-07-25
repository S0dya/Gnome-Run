using ButchersGames;
using TMPro;
using UnityEngine;

public class UIGameFinish : MonoBehaviour
{
    [SerializeField] private UILevelFinish victoryFinish;
    [SerializeField] private UILevelFinish gameoverFinish;

    [SerializeField] private TextMeshProUGUI moneyAmountText;
    [SerializeField] private TextMeshProUGUI levelIndexText;

    public void Init()
    {
        LevelManager.Default.OnLevelFinishedVictory += OnFinishLevelVictory;
        LevelManager.Default.OnLevelFinishedGameover += OnFinishLevelGameover;
    }

    public void SetProgressOnVictory(int moneyAmount)
    {
        moneyAmountText.text = moneyAmount.ToString();
        levelIndexText.text = "Level " + LevelManager.Default.CurrentLevelIndex.ToString();
    }

    public void OnNextLevelButton()
    {
        LevelManager.Default.NextLevel();
        victoryFinish.Close();
    }
    public void OnRestartGameButton()
    {
        LevelManager.Default.RestartLevel();
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
