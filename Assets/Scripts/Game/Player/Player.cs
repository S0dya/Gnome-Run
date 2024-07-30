using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SubjectMonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAnimator playerAnimator;

    private void Awake()
    {

        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.LevelStarted, OnStartLevel},
            { EventEnum.LevelFinishedVictory, OnFinishLevelVictory},
            { EventEnum.LevelFinishedGameover, OnFinishLevelGameover},
        });
    }

    public void SetPlayerSpawnPosition(Vector3 position)
    {
        playerMovement.transform.position = position;
    }

    public void SetStatus(int statusIndex)
    {
        //myb do a little spin

        playerAnimator.SetStatus(statusIndex);
    }


    private void OnStartLevel()
    {
        playerMovement.enabled = true;
    }

    private void OnFinishLevelVictory()
    {
        playerMovement.enabled = false;
    }
    private void OnFinishLevelGameover()
    {
        playerMovement.enabled = false;
    }
}
