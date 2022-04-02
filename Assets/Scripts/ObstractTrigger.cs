using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstractTrigger : MonoBehaviour
{
    private Transform obstract;
    private bool triggered;
    private int firlatilacakKardanAdam;
    private void Start()
    {
        obstract = this.transform.parent;
        firlatilacakKardanAdam = GameManager.instance.countOfFirlatilacakKardanAdam;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!triggered && other.CompareTag("Player") && KardanAdamManager.instance.kardanAdamlar.Count >= firlatilacakKardanAdam)
        {
            Debug.Log("player triggera girdi");
            KardanAdamManager.instance.KardanAdamFirlat(obstract);
            triggered = true;
        }
    }
}
