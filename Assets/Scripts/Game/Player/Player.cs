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

    [SerializeField] private Transform[] charactersTransforms;
    [SerializeField] private Transform charactersParentTransform;

    private Transform _characterMeshesParentTransform;

    private int _curCharacterI = -1;

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
        if (_curCharacterI > 0) charactersTransforms[_curCharacterI].SetParent(charactersParentTransform);
        _curCharacterI = i;
        charactersTransforms[_curCharacterI].SetParent(characterHolderTransform);

        playerAnimator.SetCharacter(i, 
            out Transform newCharacterMeshesParentTransform, out Transform newCameraFollowPoint);

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
        playerMovement.transform.position = new Vector3(position.x, transform.position.y, position.z);
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
