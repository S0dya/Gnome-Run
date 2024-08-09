using UnityEngine;
using DG.Tweening;

public class UILevelFinish : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private void Start()
    {
        Close();
    }

    public virtual void Open()
    {
        canvasGroup.DOFade(1, 0.25f).OnComplete(() => canvasGroup.blocksRaycasts = true);
    }

    public void Close()
    {
        canvasGroup.alpha = 0; canvasGroup.blocksRaycasts = false;
    }
}
