using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class CharacterStats
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

    private GameObject _curCharacterObj;

    private SkinnedMeshRenderer[] _curStatusMeshes;
    private Animator _curAnimator;

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

    public void SetCharacter(int i, out Transform characterTransform, out Transform characterMeshesParentTransform, out Transform cameraFollowPoint)
    {
        if (_curCharacterObj != null) _curCharacterObj.SetActive(false);

        _curCharacterObj = charactersStats[i].CharacterObj;
        _curAnimator = charactersStats[i].CharacterAnimator;
        _curStatusMeshes = charactersStats[i].CharacterStatusMeshes;

        characterTransform = _curCharacterObj.transform;
        characterMeshesParentTransform = charactersStats[i].CharacterMeshesParentTransform; 
        cameraFollowPoint = charactersStats[i].CameraFollowPoint;

        _curCharacterObj.SetActive(true);
    }

    public void SetStatus(int index)
    {
        _curStatusMeshes[_lastStatus].enabled = false;
        _lastStatus = index;
        _curStatusMeshes[_lastStatus].enabled = true;
    }

    public void OnBadInteraction()
    {
        _curAnimator.Play(_animatorIDLose);
    }
    public void OnGoodInteraction()
    {
        _curAnimator.Play(_animatorIDVictory);
    }
    public void OnInteractionStopped()
    {
        _curAnimator.Play(_animatorIDWalk);
    }

    //events
    private void OnStartLevel()
    {
        _curAnimator.Play(_animatorIDWalk);
    }
    private void OnFinishLevelVictory()
    {
        _curAnimator.Play(_animatorIDVictory);
    }
    private void OnFinishLevelGameover()
    {
        _curAnimator.Play(_animatorIDLose);
    }
    private void OnRestartLevel()
    {
        _curAnimator.Play(_animatorIDIdle);

        SetStatus(1);
    }
}
