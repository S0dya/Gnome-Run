using System;
using System.Collections;
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
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private KVCameraPosition[] cameraPositions;


    private Dictionary<EventEnum, Transform> _cameraPositionDict = new();

    private Coroutine _moveToCoroutine;

    void Awake()
    {
        foreach (var cameraPosition in cameraPositions)
        {
            _cameraPositionDict.Add(cameraPosition.CameraPositionEventEnum, cameraPosition.CameraPositionTransform);
        }
    }

    protected override void OnEnable() => Observer.OnEvent += OnEvent;
    protected override void OnDisable() => Observer.OnEvent -= OnEvent;

    protected override void OnEvent(EventEnum eventEnum)
    {
        if (_cameraPositionDict.ContainsKey(eventEnum)) MoveCameraToTransform(eventEnum);
    }

    private void MoveCameraToTransform(EventEnum eventEnum)
    {
        StopRoutine(_moveToCoroutine);
        _moveToCoroutine = StartCoroutine(LerpMoveLocalTransformCoroutine
            (cameraTransform, cameraSpeed, _cameraPositionDict[eventEnum].localPosition));
    }
}
