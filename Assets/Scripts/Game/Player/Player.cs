using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

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

    private AudioManager _audioManager;

    private Transform _curCharacterTransform;
    private Transform _characterMeshesParentTransform;

    private Coroutine _spinPlayerCoroutine;

    [Inject]
    public void Construct(AudioManager audioManager)
    {
        _audioManager = audioManager;
    }

    public void Init()
    {
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

    public void SetCharacter()
    {
        bool isDeselected = _curCharacterTransform != null;

        playerAnimator.SetCharacter(Settings.CurCharacterI, out Transform newCharacterTransform,
            out Transform newCharacterMeshesParentTransform, out Transform newCameraFollowPoint);

        if (isDeselected) _curCharacterTransform.SetParent(charactersParentTransform);
        _curCharacterTransform = newCharacterTransform;
        _curCharacterTransform.SetParent(characterHolderTransform);

        _characterMeshesParentTransform = newCharacterMeshesParentTransform;

        playerMovement.SetNewCharacter(newCharacterMeshesParentTransform);
        cameraController.SetNewCharacter(newCameraFollowPoint);

        if (isDeselected) SpinPlayer(180);
    }
    public void DeselectCharacter() => SpinPlayer(0);


    public void SetPlayerSpawnPosition(Vector3 position)
    {
        playerMovement.MoveCharacter(position);
    }

    public void BadInteractionStopMovement()
    {
        playerAnimator.OnBadInteraction();

        ToggleMovement(false);
    }
    public void GoodInteractionStopMovement()
    {
        playerAnimator.OnGoodInteraction();

        ToggleMovement(false);
    }
    public void InteractionStopMovementStopped()
    {
        playerAnimator.OnInteractionStopped();

        ToggleMovement(true);
    }

    public void ToggleMovement(bool toggle) => playerMovement.enabled = toggle;
    public void OnPlayFootstep() => _audioManager.PlayOneShot(SoundEventEnum.PlayerFootstep);

    private void SpinPlayer(float targetRotation)
    {
        StopRoutine(_spinPlayerCoroutine);
        _spinPlayerCoroutine = StartCoroutine(LerpRotateTransform(_characterMeshesParentTransform, spinningSpeed, targetRotation));
    }

    //events
    public void SetStatus(int statusIndex) => playerAnimator.SetStatus(statusIndex);

    private void OnStartLevel()
    {
        ToggleMovement(true);
    }

    private void OnFinishLevelVictory()
    {
        ToggleMovement(false);
        SpinPlayer(180);
    }
    private void OnFinishLevelGameover()
    {
        ToggleMovement(false);
        SpinPlayer(180);
    }
    private void OnRestartLevel()
    {
        ToggleMovement(false);
        SpinPlayer(0);
    }

    private void OnShopOpened() => SpinPlayer(180);
    private void OnShopClosed() => SpinPlayer(0);
}
