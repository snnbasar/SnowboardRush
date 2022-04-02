using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gem : MonoBehaviour
{
    private BoxCollider col;
    public Ease ease;
    public float duration;
    public float height;
    public GameObject pickupParticle;
    void Start()
    {
        col = GetComponent<BoxCollider>();
        float curY = transform.localPosition.y;

        transform.DOLocalMoveY(curY + height, duration).From(curY).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GemCounterManager.instance.CounterArttir(this.transform.position);
            SoundManager.instance.PlayMusic(Soundlar.PickUpGem);
            Instantiate(pickupParticle, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
