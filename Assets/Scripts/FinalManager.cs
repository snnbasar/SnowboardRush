using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalManager : MonoBehaviour
{
    public static FinalManager instance;
    public List<SingleFinalKatsayi> finalCarpanlar = new List<SingleFinalKatsayi>();

    public int kardanAdamSayisi;
    public float finalGoDuration;

    public float puanCarpani;
    private bool finalCalled;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        foreach (SingleFinalKatsayi i in GetComponentsInChildren<SingleFinalKatsayi>())
        {
            finalCarpanlar.Add(i);
        }
    }
    public void DoFinal()
    {
        UIManager.instance.restartButton.SetActive(false);
        UIManager.instance.playButton.SetActive(false);
        KarakterKontrol.instance.CanMove(false);
        kardanAdamSayisi = KardanAdamManager.instance.kardanAdamlar.Count;
        int lastAdam = 0;
        for (int i = 0; i < kardanAdamSayisi; i++)
        {
            if (i == kardanAdamSayisi - 1)
                lastAdam = i;
            if(i >= 50)
                KardanAdamManager.instance.FinalKardanAdamFirlat(i, finalCarpanlar[50].transform.position + GetRandomPos(1f));
            else
                KardanAdamManager.instance.FinalKardanAdamFirlat(i, finalCarpanlar[i].transform.position);
        }
        if (lastAdam == 0)
            return;
        KarakterKontrol.instance.cam.Follow = KardanAdamManager.instance.kardanAdamlar[lastAdam].transform;
        KarakterKontrol.instance.cam.LookAt = KardanAdamManager.instance.kardanAdamlar[lastAdam].transform;
    }

    public void OnFinalComplete()
    {
        float puan = 0;

        if(kardanAdamSayisi >= 51)
        {
            puan = 51 + 9;
            puan /= 10;
        }
        else
        {
            puan = kardanAdamSayisi + 9;
            puan /= 10;
        }
        puanCarpani = puan;

        print(puan);
        int newGems = (int)(GemCounterManager.instance.gems * puanCarpani);
        for (int i = 0; i < newGems; i++)
        {
            GemCounterManager.instance.CounterArttir(KarakterKontrol.instance.transform.position);
        }
    }

    public void Completed()
    {
        if (!finalCalled)
        {
            OnFinalComplete();
            print("Completed");
            finalCalled = true;
            GameManager.instance.OnLevelComplete();
        }
    }
    
    private Vector3 GetRandomPos(float spread)
    {
        float x = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);

        return new Vector3(x, 0, z);
    }
}
