using FMOD.Studio;
using System;
using System.Collections.Generic;

public class LevelMusic : SubjectMonoBehaviour
{
    private EventInstance _musicInstance;

    public void Init(EventInstance instance)
    {
        Init(new Dictionary<EventEnum, Action>
        {
            { EventEnum.LevelStarted, OnStartLevel},
            { EventEnum.LevelRestarted, OnFinishLevel},
        });
        
        _musicInstance = instance;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }

    private void OnStartLevel() => _musicInstance.start();
    private void OnFinishLevel() => _musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
}
