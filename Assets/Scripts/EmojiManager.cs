using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public enum Compliment
{
    Great,
    Awesome,
    Perfect,
    Good
}
public class EmojiManager : MonoBehaviour
{
    public static EmojiManager instance;

    private Compliment compliment;
    private Emoji emoji;

    public Transform[] compliments;

    public float scale;
    public float duration;

    [SerializeField] private float forGood, forGreat, forAwesome, forPerfect;

    private Vector3 curScale;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance.gameObject);
    }

    private void Start()
    {
        foreach  (Transform comp in compliments)
        {
            comp.gameObject.SetActive(false);
            curScale = comp.localScale;
        }
    }

    public void DoRandomCompliment()
    {
        int rndm = Random.Range(0, 3);

        compliment = (Compliment)rndm;
        DoCompliment();
    }

    public void DoVeloBasedCompliment(float velo)
    {
        if (velo >= forGood && velo < forGreat) 
        { 
            compliment = Compliment.Good; 
        }
        else if(velo >= forGreat && velo < forAwesome)
        {
            compliment = Compliment.Great;
        }
        else if (velo >= forAwesome && velo < forPerfect)
        {
            compliment = Compliment.Awesome;
        }
        else if (velo >= forPerfect)
        {
            compliment = Compliment.Perfect;
        }
        DoCompliment();
        GemCounterManager.instance.GiveComplimentGem(compliment);
    }

    public void DoCompliment()
    {

        Transform curComplement = null;

        switch (compliment)
        {
            case Compliment.Great:
                curComplement = compliments[0];
                emoji = Emoji.Gulen;
                break;
            case Compliment.Awesome:
                curComplement = compliments[1];
                emoji = Emoji.Gozluklu;
                break;
            case Compliment.Perfect:
                curComplement = compliments[2];
                emoji = Emoji.Bayildim;
                break;
            case Compliment.Good:
                curComplement = compliments[3];
                emoji = Emoji.Mutlu;
                break;
            default:
                break;
        }

        KardanAdamManager.instance.PlayEmoji(emoji);


        curComplement.gameObject.SetActive(true);

        TextMeshProUGUI tmp = curComplement.GetComponent<TextMeshProUGUI>();
        Sequence seq = DOTween.Sequence();
        seq.Append(curComplement.DOScale(new Vector3(scale, scale, scale), duration / 2).From(new Vector3(0.4f, 0.4f, 0.4f))).
            Append(tmp.DOFade(0f, duration / 2).From(1f));
        seq.OnComplete(() => { curComplement.gameObject.SetActive(false); });

    }

    private void ResetApperiance()
    {
        for (int i = 0; i < compliments.Length; i++)
        {
            compliments[i].localScale = curScale;
            TextMeshProUGUI tmp = compliments[i].GetComponent<TextMeshProUGUI>();
            Color color = tmp.color;
            color.a = 1f;
            tmp.color = color;
            compliments[i].gameObject.SetActive(false);
        }
    }

}
