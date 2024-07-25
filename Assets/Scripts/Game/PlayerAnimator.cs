using ButchersGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] statusMeshes;
    [SerializeField] private Animator animator;

    private int _lastStatus = 1;

    public void Init()
    {
        LevelManager.Default.OnLevelStarted += OnStartLevel;

        LevelManager.Default.OnLevelFinishedVictory += OnFinishLevelVictory;
        LevelManager.Default.OnLevelFinishedGameover += OnFinishLevelGameover;
        
        LevelManager.Default.OnLevelRestarted += OnRestartLevel;
    }

    public void SetStatus(int index)
    {
        index++;

        statusMeshes[_lastStatus].enabled = false;
        statusMeshes[index].enabled = true;

        _lastStatus = index;
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
