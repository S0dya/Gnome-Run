using DG.Tweening;
using TMPro;
using UnityEngine;

public class UITextEffect : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] protected private RectTransform rectTransform;
    [SerializeField] protected private TextMeshProUGUI text;
    [SerializeField] protected private CanvasGroup canvasGroup;

    [SerializeField] protected private Vector3 InitialScale = new(0.3f, 0.3f, 0.3f);

    protected private Tween _tweener;

    protected private Vector3 _initialPosition;

    protected virtual void Start()
    {
        canvasGroup.alpha = 0;
        _initialPosition = rectTransform.anchoredPosition;
    }

    protected virtual void PrepareAnimate(string valueText)
    {
        _tweener?.Kill();

        rectTransform.localScale = InitialScale;
        rectTransform.anchoredPosition = _initialPosition;

        text.text = valueText;
        canvasGroup.alpha = 1;
    }
}
