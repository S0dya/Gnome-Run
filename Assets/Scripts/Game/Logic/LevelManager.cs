using System.Collections.Generic;
using UnityEditor;
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

    private int _currentLevel { get { return (_completeLevelCount < _curLevels.Count ? CurrentLevelIndex : _completeLevelCount) + 1; ; } set { PlayerPrefs.SetInt(Settings.CurrentLevel_PrefsKey, value); } }
    private int _completeLevelCount { get { return PlayerPrefs.GetInt(Settings.CompleteLevelCount_PrefsKey); } set { PlayerPrefs.SetInt(Settings.CompleteLevelCount_PrefsKey, value); } }
    private int _lastLevelIndex { get { return PlayerPrefs.GetInt(Settings.LastLevelIndex_PrefsKey); } set { PlayerPrefs.SetInt(Settings.LastLevelIndex_PrefsKey, value); } }
    private int _currentLocation { get { return PlayerPrefs.GetInt(Settings.CurrentLocation_PrefsKey); } set { PlayerPrefs.SetInt(Settings.CurrentLocation_PrefsKey, value); } }
    private int _currentAttempt { get { return PlayerPrefs.GetInt(Settings.CurrentAttempt_PrefsKey); } set { PlayerPrefs.SetInt(Settings.CurrentAttempt_PrefsKey, value); } }

    private List<Level> _curLevels = new();
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
        _currentLocation = 0;

        _curLevels = locationsAndLevels[_currentLocation].levelsList;
        _curLocation = locationsAndLevels[_currentLocation].locationPrefab;// change later

        Instantiate(_curLocation, locationParent);

        SelectLevel(_lastLevelIndex);
        if (_lastLevelIndex != _currentLevel) _currentAttempt = 0;
    }

    private void OnDestroy()
    {
        _lastLevelIndex = CurrentLevelIndex;
    }

    private void OnApplicationQuit()
    {
        _lastLevelIndex = CurrentLevelIndex;
    }


    public void OnRestartLevel()
    {
        SelectLevel(CurrentLevelIndex, false);
    }

    public void NextLevel()
    {
        _currentLevel++;
        SelectLevel(CurrentLevelIndex + 1);
    }

    public void SelectLevel(int levelIndex, bool indexCheck = true)
    {
        if (indexCheck)
            levelIndex = GetCorrectedIndex(levelIndex);

        var level = _curLevels[levelIndex];

        if (level)
        {
            SelLevelParams(level);
            CurrentLevelIndex = levelIndex;
        }
    }

    public void PrevLevel() => SelectLevel(CurrentLevelIndex - 1);

    private int GetCorrectedIndex(int levelIndex)
    {
        int levelId = _currentLevel;
        if (levelId > _curLevels.Count - 1)
        {
            if (locationsAndLevels[_currentLocation].randomizedLevels)
            {
                var levels = Enumerable.Range(0, locationsAndLevels[_currentLocation].levelsList.Count).ToList();
                levels.RemoveAt(CurrentLevelIndex);

                return levels[UnityEngine.Random.Range(0, levels.Count)];
            }
            else 
                return levelIndex % locationsAndLevels[_currentLocation].levelsList.Count;
        }
        return levelId;
    }

    private void SelLevelParams(Level level)
    {
        if (level)
        {
            ClearChildren(levelParent);

            Instantiate(level, levelParent);

            _gameManager.AssignNewLevel(level.MaxMoney);
            _player.SetPlayerSpawnPosition(level.PlayerSpawnPoint.position);
        }
    }
}