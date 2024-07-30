using DG.Tweening;
using UnityEngine;

public class UIMoneyCollect : UITextEffect
{
    [Header("Settings")]
    [SerializeField] private float movementDirection = 150;
    [SerializeField] private float duration = 1.5f;

    public void AnimateCollect(string valueText)
    {
        PrepareAnimate(valueText);

        _tweener = DOTween.Sequence()
            .Append(rectTransform.DOScale(1.3f, 0.2f).SetEase(Ease.OutBack))
            .Append(rectTransform.DOAnchorPosY(movementDirection, duration).SetEase(Ease.InOutSine))
            .Join(rectTransform.DOScale(1f, duration).SetEase(Ease.InOutSine))
            .Join(canvasGroup.DOFade(0, duration))
            .SetLoops(1, LoopType.Restart);
    }
}
