using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManager : MonoBehaviour
{
    public Transform finalTrigger;
    public Image progressBarMask;
    private Transform plane;
    private Transform karakter;
    private float total;
    public float yuzde;
    private float offset;
    private float offsetOfPlaneTotal;
    private void Start()
    {
        karakter = KarakterKontrol.instance.transform;
        offsetOfPlaneTotal = finalTrigger.transform.lossyScale.z * 3;
        total = CalculatePlaneScale();
        offset = karakter.position.z - 0;
    }

    private void Update()
    {
        if(yuzde <= 1f)
        {
            progressBarMask.fillAmount = yuzde;
            yuzde = YuzdeHesapla();
        }
    }

    private float YuzdeHesapla()
    {
        float charPos = karakter.position.z - offset;
        return charPos / total;
    }

    private float CalculatePlaneScale()
    {
        plane = GameObject.FindGameObjectWithTag("plane").transform;
        float planeScale = plane.lossyScale.z;
        return planeScale * 10 - offsetOfPlaneTotal + offset;
    }
}
