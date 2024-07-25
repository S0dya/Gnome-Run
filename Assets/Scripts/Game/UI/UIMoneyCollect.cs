using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIMoneyCollect : MonoBehaviour
{
    [SerializeField] private float movementDirection = 150;
    [SerializeField] private float duration = 1.5f;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private Vector3 InitialScale = new(0.6f, 0.6f, 0.6f);

    private Tween _tweener;

    private Vector3 _initialPosition;

    private void Start()
    {
        canvasGroup.alpha = 0;
        _initialPosition = rectTransform.anchoredPosition;
    }

    public void AnimateCollect(string valueText)
    {
        _tweener.Kill();

        rectTransform.localScale = InitialScale;
        rectTransform.anchoredPosition = _initialPosition;

        text.text = valueText;
        canvasGroup.alpha = 1;

        _tweener = DOTween.Sequence()
            .Append(rectTransform.DOScale(1.3f, 0.2f).SetEase(Ease.OutBack))
            .Append(rectTransform.DOAnchorPosY(movementDirection, duration).SetEase(Ease.InOutSine))
            .Join(rectTransform.DOScale(1f, duration).SetEase(Ease.InOutSine))
            .Join(canvasGroup.DOFade(0, duration))
            .SetLoops(1, LoopType.Restart);
    }
}
