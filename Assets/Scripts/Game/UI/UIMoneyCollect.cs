using DG.Tweening;
using UnityEngine;

public class UIMoneyCollect : UITextEffect
{
    [Header("Settings")]
    [SerializeField] private float movementDirection = 150;
    [SerializeField] private float fadeInDuration = 1.25f;
    [SerializeField] private float fadeOutDuration = 0.5f;

    private int _curValue;

    public void AnimateCollect(int value)
    {
        _curValue += value;

        PrepareAnimate($"{(value > 0 ? "+ " : "")}{_curValue}");

        _tweener = DOTween.Sequence()
        .Append(rectTransform.DOScale(1.4f, 0.2f).SetEase(Ease.OutBack))
        .Append(rectTransform.DOAnchorPosY(movementDirection, fadeInDuration).SetEase(Ease.InOutSine))
        .Join(rectTransform.DOScale(InitialScale, fadeOutDuration).SetEase(Ease.InOutSine))
        .Join(canvasGroup.DOFade(0, fadeOutDuration))
        .OnComplete(() =>
        {
            _curValue = 0;
        });
    }
}
