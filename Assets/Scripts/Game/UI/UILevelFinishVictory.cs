using UnityEngine;

public class UILevelFinishVictory : UILevelFinish
{
    [Header("Victory")]
    [SerializeField] private CanvasGroup watchRewardAdCG;

    public override void Open()
    {
        ToggleRewardAdButton(true);
     
        base.Open();
    }
    
    public void ToggleRewardAdButton(bool toggle)
    {
        watchRewardAdCG.blocksRaycasts = toggle;  watchRewardAdCG.alpha = toggle ? 1 : 0.75f;
    }
}