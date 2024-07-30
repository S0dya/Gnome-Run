using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : SubjectMonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] statusMeshes;
    [SerializeField] private Animator animator;

    //reusable
    private int _lastStatus = 1;

    //hash
    private int _animatorIDIdle;
    private int _animatorIDWalk;

    private void Awake()
    {
        _animatorIDIdle = Animator.StringToHash("Idle");
        _animatorIDWalk = Animator.StringToHash("Walk");

        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.LevelStarted, OnStartLevel},
            { EventEnum.LevelFinishedVictory, OnFinishLevelVictory},
            { EventEnum.LevelFinishedGameover, OnFinishLevelGameover},
            { EventEnum.LevelRestarted, OnRestartLevel},
        });
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
        animator.Play(_animatorIDWalk);
    }
    private void OnFinishLevelVictory()
    {
    }
    private void OnFinishLevelGameover()
    {
    }
    private void OnRestartLevel()
    {
        animator.Play(_animatorIDIdle);
    }
}
