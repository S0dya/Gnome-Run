using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public GameObject CharacterObj;
    public Transform CharacterMeshesParentTransform;

    public Animator CharacterAnimator;
    public SkinnedMeshRenderer[] CharacterStatusMeshes;

    public Transform CameraFollowPoint;
}

public class PlayerAnimator : SubjectMonoBehaviour
{
    [SerializeField] private CharacterStats[] charactersStats;

    [SerializeField] private GameObject curCharacterObj;

    [SerializeField] private SkinnedMeshRenderer[] curStatusMeshes;
    [SerializeField] private Animator curAnimator;

    //reusable
    private int _lastStatus = 1;

    //hash
    private int _animatorIDIdle;
    private int _animatorIDWalk;
    private int _animatorIDVictory;
    private int _animatorIDLose;

    private void Awake()
    {
        _animatorIDIdle = Animator.StringToHash("Idle");
        _animatorIDWalk = Animator.StringToHash("Walk");
        _animatorIDVictory = Animator.StringToHash("Victory");
        _animatorIDLose = Animator.StringToHash("Lose");

        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.LevelStarted, OnStartLevel},
            { EventEnum.LevelFinishedVictory, OnFinishLevelVictory},
            { EventEnum.LevelFinishedGameover, OnFinishLevelGameover},
            { EventEnum.LevelRestarted, OnRestartLevel},
        });
    }

    public void SetCharacter(int i, out Transform characterMeshesParentTransform, out Transform cameraFollowPoint)
    {
        if (curCharacterObj != null) curCharacterObj.SetActive(false);

        curCharacterObj = charactersStats[i].CharacterObj;
        curAnimator = charactersStats[i].CharacterAnimator;
        curStatusMeshes = charactersStats[i].CharacterStatusMeshes;

        characterMeshesParentTransform = charactersStats[i].CharacterMeshesParentTransform; 
        cameraFollowPoint = charactersStats[i].CameraFollowPoint;

        curCharacterObj.SetActive(true);
    }

    public void SetStatus(int index)
    {
        index++;

        curStatusMeshes[_lastStatus].enabled = false;
        curStatusMeshes[index].enabled = true;

        _lastStatus = index;
    }

    private void OnStartLevel()
    {
        curAnimator.Play(_animatorIDWalk);
    }
    private void OnFinishLevelVictory()
    {
        curAnimator.Play(_animatorIDVictory);
    }
    private void OnFinishLevelGameover()
    {
        curAnimator.Play(_animatorIDLose);
    }
    private void OnRestartLevel()
    {
        curAnimator.Play(_animatorIDIdle);
    }
}
