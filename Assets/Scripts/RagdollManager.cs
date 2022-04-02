using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    private Collider[] colliders;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        colliders = GetComponentsInChildren<Collider>();
    }

    public void DoRagdoll(bool status)
    {
        anim.enabled = !status;
        foreach (Collider col in colliders)
        {

            col.enabled = status;
            col.GetComponent<Rigidbody>().isKinematic = !status;
            col.GetComponent<Rigidbody>().AddForce(-Vector3.forward * 5, ForceMode.Impulse);
        }
    }
}
