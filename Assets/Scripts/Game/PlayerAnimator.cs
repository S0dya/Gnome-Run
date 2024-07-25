using ButchersGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void Init()
    {
        LevelManager.Default.OnLevelStarted += OnStartLevel;

        LevelManager.Default.OnLevelFinishedVictory += OnFinishLevelVictory;
        LevelManager.Default.OnLevelFinishedGameover += OnFinishLevelGameover;
        
        LevelManager.Default.OnLevelRestarted += OnRestartLevel;
    }

    private void OnStartLevel()
    {

    }
    private void OnFinishLevelVictory()
    {
    }
    private void OnFinishLevelGameover()
    {
    }
    private void OnRestartLevel()
    {

    }
}
