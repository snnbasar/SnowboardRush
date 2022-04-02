using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class EngelUi : MonoBehaviour
{
    private TextMeshPro text;

    [SerializeField] private Ease ease;
    [SerializeField] private float duration;
    private void Start()
    {
        text = transform.GetChild(0).GetComponent<TextMeshPro>();
        text.text = GameManager.instance.countOfFirlatilacakKardanAdam.ToString();

        float oldY = transform.localPosition.y;

        transform.DOLocalMoveY(oldY + 0.5f, duration).From(oldY).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
    }
}
