using DG.Tweening;
using UnityEngine;

public class UITextPopUp : UITextEffect
{
    public void AnimateNewStatus(Color color, string statusName)
    {
        PrepareAnimate(color, statusName);

        _tweener = DOTween.Sequence()
            .Append(canvasGroup.DOFade(1, 0.1f))
            .Append(rectTransform.DOScale(3f, 0.2f).SetEase(Ease.OutBack))
            .Append(rectTransform.DOScale(1.75f, 0.4f).SetEase(Ease.InOutSine))
            .Join(canvasGroup.DOFade(0, 0.5f).SetDelay(1))
            .OnComplete(() => 
            {
                rectTransform.localScale = InitialScale;
                canvasGroup.alpha = 0;
            });
    }

    private void PrepareAnimate(Color color, string valueText)
    {
        PrepareAnimate(valueText);

        text.color = color;
    }
}
