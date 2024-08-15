using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class KVCameraPosition
{
    public EventEnum CameraPositionEventEnum;
    public Transform CameraPositionTransform;
}

public class CameraController : SubjectMonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float cameraSpeed = 4;

    [Header("Other")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private KVCameraPosition[] cameraPositions;

    [SerializeField] private Transform cameraShopPosition;

    private Transform _curLookAtTransform;

    private Dictionary<EventEnum, Transform> _cameraPositionDict = new();

    private Coroutine _moveToCoroutine;

    private void Awake()
    {
        foreach (var cameraPosition in cameraPositions)
        {
            _cameraPositionDict.Add(cameraPosition.CameraPositionEventEnum, cameraPosition.CameraPositionTransform);
        }

        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.ShopOpened, OnShopOpened},
            { EventEnum.ShopClosed, OnShopClosed},
        });
    }

    private void Start()
    {
        SetCameraLookAt(_curLookAtTransform);
    }

    protected override void OnEnable() => Observer.OnEvent += OnEvent;
    protected override void OnDisable() => Observer.OnEvent -= OnEvent;

    protected override void OnEvent(EventEnum eventEnum)
    {
        base.OnEvent(eventEnum);

        if (_cameraPositionDict.ContainsKey(eventEnum)) MoveCameraToTransform(eventEnum);
    }

    private void MoveCameraToTransform(EventEnum eventEnum)
    {
        StopRoutine(_moveToCoroutine);
        _moveToCoroutine = StartCoroutine(LerpMoveLocalTransformCoroutine
            (cameraTransform, cameraSpeed, _cameraPositionDict[eventEnum].localPosition));
    }

    public void SetNewCharacter(Transform cameraLookAtTransform)
    {
        _curLookAtTransform = cameraLookAtTransform;
    }

    private void OnShopOpened() => SetCameraLookAt(cameraShopPosition);
    private void OnShopClosed() => SetCameraLookAt(_curLookAtTransform);

    private void SetCameraLookAt(Transform transform) => virtualCamera.LookAt = transform;
}
