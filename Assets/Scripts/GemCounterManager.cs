using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GemCounterManager : MonoBehaviour
{
    public static GemCounterManager instance;

    private int _gems;
    public int gems { get { return _gems; } set
        {
            _gems = value;
            collectText.text = _gems.ToString();

        } }
    [Header("Emojide Alýnacak Gemler")]
    public int perfectGem;
    public int awesomeGem;
    public int greatGem;
    public int goodGem;
    [Header("Effect Ayarlarý")]
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
            .OnComplete(() => { gems++; Destroy(x); });
    }

    private void DoAzaltEffect()
    {
        GameObject x = Instantiate(prefab);
        x.transform.SetParent(canvas);

        x.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-1";

        x.transform.DOScale(0.5f, duration).From(1f);
        x.transform.DOLocalMove(target.localPosition + RandomPosition(50, true), duration / 2).From(target.localPosition + RandomPosition(50, true)).SetEase(ease).
            OnComplete(() => { gems--; Destroy(x); });
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

    public void GiveComplimentGem(Compliment compliment) //calls on emoji manager
    {
        int howMany = 0;
        switch (compliment)
        {
            case Compliment.Great:
                howMany = greatGem;
                break;
            case Compliment.Awesome:
                howMany = awesomeGem;
                break;
            case Compliment.Perfect:
                howMany = perfectGem;
                break;
            case Compliment.Good:
                howMany = goodGem;
                break;
            default:
                break;
        }

        for (int i = 0; i < howMany; i++)
        {
            CounterArttir(KarakterKontrol.instance.transform.position);
        }
    }
}
