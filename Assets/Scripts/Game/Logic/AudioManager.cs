using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

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

    private Dictionary<SoundEventEnum, EventInstance> _enumInstancesDict = new();
    private Dictionary<EventEnum, EventInstance> _eventInstancesDict = new();

    public void Init()
    {
        foreach (var kvSound in kvSounds) _enumInstancesDict.Add(kvSound.SoundEvent, CreateInstance(kvSound.Sound));
        foreach (var evSound in evSounds) _eventInstancesDict.Add(evSound.eventEnum, CreateInstance(evSound.Sound));

        ToggleSound(Settings.HasSound);
        levelMusic.Init(_enumInstancesDict[SoundEventEnum.Music]);
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

    protected override void OnEvent(EventEnum actionEnum) => PlayOneShot(actionEnum);

    //settings
    public void ToggleSound(bool toggle)
    {
        ToggleSetBus("bus:/", toggle);

        levelMusic.enabled = toggle;
    }

    //other
    private void OnApplicationPause(bool pauseStatus)
    {
        if (levelMusic.enabled) ToggleSetBus("bus:/", pauseStatus);
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        if (levelMusic.enabled) ToggleSetBus("bus:/", hasFocus);
    }

    private void ToggleSetBus(string busName, bool toggle) => RuntimeManager.GetBus(busName).setVolume(toggle ? 1 : 0);

}