using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Zenject;

using LevelsRelated;

public class LevelManager : SubjectMonoBehaviour
{
    public int CurrentLevelIndex { get; private set; }

    [Header("Settings")]
    [SerializeField] private SO_LocationAndLevels[] locationsAndLevels;

    [Header("Other")]
    [SerializeField] private Transform locationParent;
    [SerializeField] private Transform levelParent;

    private List<Level> _curLevels = new();
    private Level _curLevel;
    private GameObject _curLocation;

    private GameManager _gameManager;
    private Player _player;

    [Inject]
    public void Construct(GameManager gameManager, Player player)
    {
        _gameManager = gameManager;
        _player = player;
    }

    private void Awake()
    {

        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.LevelRestarted, OnRestartLevel},
        });
    }


    public void Init()
    {
        Settings.CurrentLocation = 0;

        _curLevels = locationsAndLevels[Settings.CurrentLocation].levelsList;
        _curLocation = locationsAndLevels[Settings.CurrentLocation].locationPrefab;// change later

        Instantiate(_curLocation, locationParent);

        SelectLevel(Settings.LastLevelIndex);
        if (Settings.LastLevelIndex != Settings.CurrentLevel) Settings.CurrentAttempt = 0;
    }
    public void NextLevel()
    {
        Settings.CurrentLevel++;
        SelectLevel(CurrentLevelIndex + 1);
    }

    public void SelectLevel(int levelIndex, bool indexCheck = true)
    {
        if (indexCheck) levelIndex = GetCorrectedIndex(levelIndex);

        var level = _curLevels[levelIndex];

        if (level)
        {
            SelLevelParams(level);
            CurrentLevelIndex = levelIndex;
        }
    }

    public void SetCharacter() => _curLevel.SetInteractionDetails(Settings.CurCharacterI);

    private void OnRestartLevel()
    {
        SelectLevel(CurrentLevelIndex, false);
    }
    private int GetCorrectedIndex(int levelIndex)
    {
        int levelId = Settings.CurrentLevel;
        if (levelId > _curLevels.Count - 1)
        {
            if (locationsAndLevels[Settings.CurrentLocation].randomizedLevels)
            {
                var levels = Enumerable.Range(0, locationsAndLevels[Settings.CurrentLocation].levelsList.Count).ToList();
                levels.RemoveAt(CurrentLevelIndex);

                return levels[UnityEngine.Random.Range(0, levels.Count)];
            }
            else 
                return levelIndex % locationsAndLevels[Settings.CurrentLocation].levelsList.Count;
        }
        return levelId;
    }

    private void SelLevelParams(Level level)
    {
        if (level)
        {
            ClearChildren(levelParent);

            _curLevel = Instantiate(level, levelParent).GetComponent<Level>();

            _gameManager.AssignNewLevel(level.MaxMoney);
            _player.SetPlayerSpawnPosition(level.PlayerSpawnPoint.position);
            SetCharacter();
        }
    }
}