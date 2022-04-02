using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItselfAfterSec : MonoBehaviour
{
    private void Start()
    {
        float sec = GameManager.instance.destroyFractalsAfterSeconds;
        Invoke("Destroy", sec);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
