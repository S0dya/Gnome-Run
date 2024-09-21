using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;
using YG;
using System.Collections;

public enum SoundEventEnum
{
    none,
    PlayerFootstep,
    GoodCollect,
    BadCollect,
    Music,
    GateOpen,
}

[System.Serializable]
class KeyValueSound
{
    [SerializeField] public SoundEventEnum SoundEvent;
    [SerializeField] public EventReference Sound;
}

[System.Serializable]
class EnumValueSound
{
    [SerializeField] public EventEnum eventEnum;
    [SerializeField] public EventReference Sound;
}

public class AudioManager : SubjectMonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private KeyValueSound[] kvSounds;
    [SerializeField] private EnumValueSound[] evSounds;

    [Header("Other")]
    [SerializeField] private LevelMusic levelMusic;
    [SerializeField] private Bank banks;

    private Dictionary<SoundEventEnum, EventInstance> _enumInstancesDict = new();
    private Dictionary<EventEnum, EventInstance> _eventInstancesDict = new();

    private void Awake()
    {
        StartCoroutine(LoadGameCoroutine());
    }
    IEnumerator LoadGameCoroutine()
    {
        while (!RuntimeManager.HaveAllBanksLoaded)
        {
            yield return null;
        }

        while (RuntimeManager.AnySampleDataLoading())
        {
            yield return null;
        }

        InitAudio();
    }
    private void InitAudio()
    {
        foreach (var kvSound in kvSounds) _enumInstancesDict.Add(kvSound.SoundEvent, CreateInstance(kvSound.Sound));
        foreach (var evSound in evSounds) _eventInstancesDict.Add(evSound.eventEnum, CreateInstance(evSound.Sound));

        levelMusic.Init(_enumInstancesDict[SoundEventEnum.Music]);

        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.AdOpened, OnAdOpened},
            { EventEnum.AdClosed, OnAdClosed},

            { EventEnum.LevelStarted, OnLevelStart},
        });
    }
    public void Init()
    {
        ToggleSound(Settings.HasSound);
    }
    private EventInstance CreateInstance(EventReference sound)
    {
        return RuntimeManager.CreateInstance(sound);
    }

    //main 
    public void PlayOneShot(SoundEventEnum soundEventEnum)
    {
        if (Settings.HasSound) _enumInstancesDict[soundEventEnum].start();
    }
    public void PlayOneShot(EventEnum enumAction)
    {
        if (Settings.HasSound && _eventInstancesDict.ContainsKey(enumAction)) _eventInstancesDict[enumAction].start();
    }

    protected override void OnEvent(EventEnum actionEnum)
    {
        base.OnEvent(actionEnum);

        PlayOneShot(actionEnum);
    }

    //settings
    public void ToggleSound(bool toggle)
    {
        RuntimeManager.GetBus("bus:/").setVolume(toggle ? 1 : 0);

        levelMusic.enabled = toggle;
    }

    //events
    private void OnAdOpened() => ToggleResumeSound(false);
    private void OnAdClosed() => ToggleResumeSound(true);
    private void OnLevelStart() => ToggleResumeSound(true);

    //other

    private void OnApplicationPause(bool value)
    {
        if (Settings.HasSound) ToggleResumeSound(!value);
    }
    private void OnApplicationFocus(bool value)
    {
        if (Settings.HasSound) ToggleResumeSound(value);
    }
    
    public void ToggleResumeSound(bool toggle)
    {
        RuntimeManager.PauseAllEvents(!toggle);

        if (toggle) RuntimeManager.CoreSystem.mixerResume();
        else RuntimeManager.CoreSystem.mixerSuspend();

        RuntimeManager.MuteAllEvents(!toggle);
    }
}