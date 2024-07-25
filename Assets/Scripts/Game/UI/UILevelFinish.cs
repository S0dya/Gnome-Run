using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UILevelFinish : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    private void Start()
    {
        Close();
    }

    public void Open()
    {
        canvasGroup.DOFade(1, 0.25f).OnComplete(() =>
        {
            canvasGroup.blocksRaycasts = true;
        });
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
