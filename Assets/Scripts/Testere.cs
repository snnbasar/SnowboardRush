using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Testere : MonoBehaviour
{
    public float rotateDuration;
    public Ease rotationEase;
    public float moveXDuration;
    public Ease moveXEase;
    public float fromPosX;
    public float toPosX;

    private void Start()
    {
        transform.DOLocalRotate(Vector3.forward * 360, rotateDuration, RotateMode.FastBeyond360).From(Vector3.zero).SetEase(rotationEase).SetLoops(-1, LoopType.Restart);
        transform.DOLocalMoveX(toPosX, moveXDuration).From(fromPosX).SetEase(moveXEase).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("kardanadam") && !GameManager.instance.gameMod)
        {
            collision.transform.GetComponent<Adam>().DestroyMe();
        }
        if (collision.transform.CompareTag("Player") && !GameManager.instance.gameMod)
        {
            KarakterKontrol.instance.KillMe();
            SoundManager.instance.PlayMusic(Soundlar.WallOnDontBreak);
        }
    }
}
