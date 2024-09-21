using YG;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalYG : SubjectMonoBehaviour
{
    private void Awake()
    {

        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.LevelStarted, OnStartLevel},

            { EventEnum.LevelRestarted, OnStopLevel},
            { EventEnum.LevelFinishedVictory, OnStopLevel},
            { EventEnum.LevelFinishedGameover, OnStopLevel},
        });
    }

    private void Start()
    {
        YandexGame.InitGRA();
    }

    private void OnStartLevel()
    {
        YandexGame.GameplayStart();
    }
    private void OnStopLevel()
    {
        YandexGame.GameplayStop();
    }
}
