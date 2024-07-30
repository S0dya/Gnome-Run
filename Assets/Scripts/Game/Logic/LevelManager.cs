using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using Zenject;

public class LevelManager : SubjectMonoBehaviour
{
    const string CurrentLevel_PrefsKey = "Current Level";
    const string CompleteLevelCount_PrefsKey = "Complete Lvl Count";
    const string LastLevelIndex_PrefsKey = "Last Level Index";
    const string CurrentAttempt_PrefsKey = "Current Attempt";

    private int _currentLevel { get { return (CompleteLevelCount < _levels.Count ? CurrentLevelIndex : CompleteLevelCount) + 1; ; } set { PlayerPrefs.SetInt(CurrentLevel_PrefsKey, value); } }
    private int CompleteLevelCount { get { return PlayerPrefs.GetInt(CompleteLevelCount_PrefsKey); } set { PlayerPrefs.SetInt(CompleteLevelCount_PrefsKey, value); } }
    private int LastLevelIndex { get { return PlayerPrefs.GetInt(LastLevelIndex_PrefsKey); } set { PlayerPrefs.SetInt(LastLevelIndex_PrefsKey, value); } }
    private int CurrentAttempt { get { return PlayerPrefs.GetInt(CurrentAttempt_PrefsKey); } set { PlayerPrefs.SetInt(CurrentAttempt_PrefsKey, value); } }

    public int CurrentLevelIndex { get; private set; }

    [SerializeField] bool editorMode = false;
    [SerializeField] SO_LevelsList levels;
    private List<Level> _levels => levels.lvls;

    private Player _player;

    [Inject]
    public void Construct(Player player)
    {
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
#if !UNITY_EDITOR
        editorMode = false;
#endif
        if (!editorMode) SelectLevel(LastLevelIndex);
        if (LastLevelIndex != _currentLevel) CurrentAttempt = 0;
    }

    private void OnDestroy()
    {
        LastLevelIndex = CurrentLevelIndex;
    }

    private void OnApplicationQuit()
    {
        LastLevelIndex = CurrentLevelIndex;
    }


    public void OnRestartLevel()
    {
        SelectLevel(CurrentLevelIndex, false);
    }

    public void NextLevel()
    {
        if (!editorMode) _currentLevel++;
        SelectLevel(CurrentLevelIndex + 1);
    }

    public void SelectLevel(int levelIndex, bool indexCheck = true)
    {
        if (indexCheck)
            levelIndex = GetCorrectedIndex(levelIndex);

        if (_levels[levelIndex] == null)
        {
            Debug.Log("<color=red>There is no prefab attached!</color>"); return;
        }

        var level = _levels[levelIndex];

        if (level)
        {
            SelLevelParams(level);
            CurrentLevelIndex = levelIndex;
        }
    }

    public void PrevLevel() => SelectLevel(CurrentLevelIndex - 1);

    private int GetCorrectedIndex(int levelIndex)
    {
        if (editorMode)
            return levelIndex > _levels.Count - 1 || levelIndex <= 0 ? 0 : levelIndex;
        else
        {
            int levelId = _currentLevel;
            if (levelId > _levels.Count - 1)
            {
                if (levels.randomizedLvls)
                {
                    List<int> lvls = Enumerable.Range(0, levels.lvls.Count).ToList();
                    lvls.RemoveAt(CurrentLevelIndex);

                    return lvls[UnityEngine.Random.Range(0, lvls.Count)];
                }
                else 
                    return levelIndex % levels.lvls.Count;
            }
            return levelId;
        }
    }

    private void SelLevelParams(Level level)
    {
        if (level)
        {
            ClearChilds();
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                Instantiate(level, transform);
            }
            else
            {
                PrefabUtility.InstantiatePrefab(level, transform);
            }
#else
            Instantiate(level, transform);
#endif

            _player.SetPlayerSpawnPosition(level.GetSpawnPosition());
        }
    }

    private void ClearChilds()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject destroyObject = transform.GetChild(i).gameObject;
            DestroyImmediate(destroyObject);
        }
    }
}