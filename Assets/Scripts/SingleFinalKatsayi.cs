using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFinalKatsayi : MonoBehaviour
{
    public float katSayi;
    public bool carpti;

    private void OnCollisionEnter(Collision collision)
    {
        carpti = true;
        collision.transform.GetComponent<Rigidbody>().isKinematic = true;
        collision.transform.position = transform.position + Vector3.up;
    }
}
