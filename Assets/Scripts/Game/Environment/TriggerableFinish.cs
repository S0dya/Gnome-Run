using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerableFinish : ITriggerable
{
    public int GetFinishIndex();
}

public class TriggerableFinish : Triggerable, ITriggerableFinish
{
    [SerializeField] private int finishIndex;

    public int GetFinishIndex()
    {
        return finishIndex;
    }
}
