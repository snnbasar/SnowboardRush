using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalManagerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FinalManager.instance.DoFinal();
        }
    }
}
