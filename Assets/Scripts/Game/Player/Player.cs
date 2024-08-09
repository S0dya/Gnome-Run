using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : SubjectMonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float spinningSpeed = 12;

    [Header("Other")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private CameraController cameraController;

    [Header("Characters")]
    [SerializeField] private Transform characterHolderTransform;
    [SerializeField] private Transform charactersParentTransform;

    private Transform _curCharacterTransform;
    private Transform _characterMeshesParentTransform;

    private Coroutine _spinPlayerCoroutine;

    public void Init()
    {
        SetCharacter(Settings.CurCharacterI);

        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.LevelStarted, OnStartLevel},
            { EventEnum.LevelFinishedVictory, OnFinishLevelVictory},
            { EventEnum.LevelFinishedGameover, OnFinishLevelGameover},
            { EventEnum.LevelRestarted, OnRestartLevel},
            { EventEnum.ShopOpened, OnShopOpened},
            { EventEnum.ShopClosed, OnShopClosed},
        });
    }

    public void SetCharacter(int i)
    {
        playerAnimator.SetCharacter(i, out Transform newCharacterTransform, 
            out Transform newCharacterMeshesParentTransform, out Transform newCameraFollowPoint);

        if (_curCharacterTransform != null) _curCharacterTransform.SetParent(charactersParentTransform);
        _curCharacterTransform = newCharacterTransform;
        _curCharacterTransform.SetParent(characterHolderTransform);

        _characterMeshesParentTransform = newCharacterMeshesParentTransform;

        playerMovement.SetNewCharacter(newCharacterMeshesParentTransform);
        cameraController.SetNewCharacter(newCameraFollowPoint);
    }

    public void SpinPlayer(float targetRotation)
    {
        StopRoutine(_spinPlayerCoroutine);
        _spinPlayerCoroutine = StartCoroutine(LerpRotateTransform(_characterMeshesParentTransform , spinningSpeed, targetRotation));
    }

    public void SetPlayerSpawnPosition(Vector3 position)
    {
        playerMovement.MoveCharacter(position);
    }

    public void SetStatus(int statusIndex)
    {
        playerAnimator.SetStatus(statusIndex);
    }

    private void OnStartLevel()
    {
        playerMovement.enabled = true;
    }

    private void OnFinishLevelVictory()//rewrite later
    {
        playerMovement.enabled = false;
    }
    private void OnFinishLevelGameover()
    {
        playerMovement.enabled = false;
    }
    private void OnRestartLevel()
    {
        playerMovement.enabled = false;
    }

    private void OnShopOpened()
    {
        SpinPlayer(180);
    }
    private void OnShopClosed()
    {
        SpinPlayer(0);
    }
}
