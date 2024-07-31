using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SubjectMonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float spinningSpeed = 12;

    [Header("Other")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAnimator playerAnimator;

    [SerializeField] private Transform characterTransform;

    private Coroutine _spinPlayerCoroutine;

    private void Awake()
    {

        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.LevelStarted, OnStartLevel},
            { EventEnum.LevelFinishedVictory, OnFinishLevelVictory},
            { EventEnum.LevelFinishedGameover, OnFinishLevelGameover},
        });
    }

    private void Start()
    {
        SpinPlayer(180);
    }

    public void SpinPlayer(float targetRotation)
    {
        StopRoutine(_spinPlayerCoroutine);
        _spinPlayerCoroutine = StartCoroutine(LerpRotateTransform(characterTransform, spinningSpeed, targetRotation));
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
