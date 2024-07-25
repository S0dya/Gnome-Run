using DG.Tweening;
using TMPro;
using UnityEngine;

public class UITextPopUp : MonoBehaviour
{

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private Vector3 InitialScale = new(0.3f, 0.3f, 0.3f);

    private Tween _tweener;

    private Vector3 _initialPosition;

    private void Start()
    {
        canvasGroup.alpha = 0;
        _initialPosition = rectTransform.anchoredPosition;
    }

    public void AnimateNewStatus(Color color, string statusName)
    {
        _tweener?.Kill();

        rectTransform.localScale = InitialScale;
        rectTransform.anchoredPosition = _initialPosition;

        text.color = color;
        text.text = statusName;

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
}
