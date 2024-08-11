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

    private Dictionary<SoundEventEnum, EventInstance> _eventInstancesDict = new();
    private Dictionary<EventEnum, EventInstance> _enumInstancesDict = new();

    //initialization
    public void Init()
    {

        foreach (var kvSound in kvSounds) _eventInstancesDict.Add(kvSound.SoundEvent, CreateInstance(kvSound.Sound));
        foreach (var evSound in evSounds) _enumInstancesDict.Add(evSound.eventEnum, CreateInstance(evSound.Sound));
    }

    private EventInstance CreateInstance(EventReference sound)
    {
        return RuntimeManager.CreateInstance(sound);
    }

    //main methods
    public void PlayOneShot(SoundEventEnum soundEventEnum) => _eventInstancesDict[soundEventEnum].start();
    public void PlayOneShot(EventEnum enumAction)
    {
        if (_enumInstancesDict.ContainsKey(enumAction)) _enumInstancesDict[enumAction].start();
    }

    protected override void OnEvent(EventEnum actionEnum) => PlayOneShot(actionEnum);

    //settings
    public void ToggleSound(bool toggle) => RuntimeManager.GetBus("bus:/").setVolume(toggle ? 1 : 0);
}