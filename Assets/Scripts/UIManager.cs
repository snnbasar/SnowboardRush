using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    public GameObject playButton;
    public GameObject restartButton;

    public GameObject levelComplete;
    public GameObject gameOver;
    public Image star1;
    public Image star2;
    public Image star3;
    public Image kupa;
    public TextMeshProUGUI levelCompleteText;

    public float duration;
    public Ease ease;

    private void Awake()
    {
        instance = this;
        OnLevelComplete(false);
        OnGameOver(false);
        playButton.SetActive(false);
        restartButton.SetActive(true);
    }

    public void OnLevelComplete(bool stat)
    {
        playButton.SetActive(false);
        restartButton.SetActive(false);
        levelComplete.SetActive(stat);
        Sequence seq = DOTween.Sequence();
        seq.Append(levelComplete.transform.DOScale(Vector3.one, duration).From(new Vector3(0.5f, 0.5f, 0.5f)).SetEase(ease));



        seq.OnComplete(() => { YildizHesapla(); });
    }

    public void OnGameOver(bool stat)
    {
        playButton.SetActive(false);
        restartButton.SetActive(false);
        gameOver.SetActive(stat);
        Sequence seqGameOver = DOTween.Sequence();
        seqGameOver.Append(gameOver.transform.DOScale(Vector3.one, duration).From(new Vector3(0.5f, 0.5f, 0.5f)).SetEase(ease));
    }

    private void YildizHesapla()
    {
        int yildiz1 = GameManager.instance.star1;
        int yildiz2 = GameManager.instance.star2;
        int yildiz3 = GameManager.instance.star3;

        int totalGems = GemCounterManager.instance.gems;
        print("totalGems: " + totalGems);
        if(totalGems >= yildiz1 && totalGems < yildiz2)
        {
            star1.DOFade(1f, duration).From(0f).SetEase(ease);
        }
        else if (totalGems >= yildiz2 && totalGems < yildiz3)
        {
            Sequence seqStar2 = DOTween.Sequence();
            seqStar2.Append(star1.DOFade(1f, duration).From(0f).SetEase(ease)).
                Append(star2.DOFade(1f, duration).From(0f).SetEase(ease));
        }
        else if (totalGems >= yildiz3)
        {
            Sequence seqStar3 = DOTween.Sequence();
            seqStar3.Append(star1.DOFade(1f, duration).From(0f).SetEase(ease)).
                Append(star2.DOFade(1f, duration).From(0f).SetEase(ease)).
                Append(star3.DOFade(1f, duration).From(0f).SetEase(ease));
            
        }
    }
}
