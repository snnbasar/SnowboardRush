using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class KardanAdamCounterManager : MonoBehaviour
{
    public static KardanAdamCounterManager instance;

    private int _collectedAdams;
    public int collectedAdams { get { return _collectedAdams; } set
        {
            _collectedAdams = value;
            collectText.text = _collectedAdams.ToString();
        } }

    public float duration;
    public Ease ease;
    public GameObject prefab;
    public Camera cam;
    public Transform target;
    public Transform canvas;

    private TextMeshProUGUI collectText;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        collectText = target.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void CounterArttir(Vector3 pos)
    {
        
        DoArttirEffect(pos);
    }

    public void CounterAzalt()
    {
        
        DoAzaltEffect();
    }
    public void DoArttirEffect(Vector3 pos)
    {
        Vector3 fromPos = cam.WorldToViewportPoint(pos);
        GameObject x = Instantiate(prefab);
        x.transform.SetParent(canvas);

        x.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+1";

        x.transform.DOScale(1f, duration).From(0.5f);
        x.transform.DOLocalMove(target.localPosition, duration).From(fromPos + RandomPosition(50, false)).SetEase(ease)
            .OnComplete(() => { collectedAdams++; Destroy(x); });
    }

    private void DoAzaltEffect()
    {
        GameObject x = Instantiate(prefab);
        x.transform.SetParent(canvas);

        x.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-1";

        x.transform.DOScale(0.5f, duration).From(1f);
        x.transform.DOLocalMove(target.localPosition + RandomPosition(50, true), duration / 2).From(target.localPosition + RandomPosition(50, true)).SetEase(ease).
            OnComplete(() => { collectedAdams--; Destroy(x); });
    }

    private Vector3 RandomPosition(float a, bool withY)
    {
        float x = Random.Range(-1 * a, 1 * a);
        if (withY)
        {
            float y = Random.Range(-1 * a, 1 * a);
            return new Vector3(x, y, 0);
        }
        else
            return new Vector3(x, 0, 0);
    }
}
