using ButchersGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameMain : MonoBehaviour
{
    [SerializeField] private GameObject gameMenuUIElement;


    public void Init()
    {
        LevelManager.Default.OnLevelStarted += OnStartLevel;
    }

    public void OnPressedToStartButton()
    {
        LevelManager.Default.StartLevel();
    }

    private void OnStartLevel()
    {
        gameMenuUIElement.SetActive(false);
    }
}
