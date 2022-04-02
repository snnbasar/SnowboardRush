using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpKardanAdam : MonoBehaviour
{
    public bool picked;

    private Adam adam;

    private void Start()
    {
        adam = GetComponent<Adam>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !picked)
        {
            KardanAdamManager.instance.kardanAdamlar.Add(this.gameObject);
            SoundManager.instance.PlayMusic(Soundlar.PickUp);
            KardanAdamManager.instance.UpdateKardanAdams();
            KardanAdamCounterManager.instance.CounterArttir(this.transform.position);
            picked = true;
            adam.AdamPicked();
            
        }
    }
}
