using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Cinemachine;

public enum Emoji
{
    Gozluklu,
    Gulen,
    Mutlu,
    Bayildim
}

public class Adam : MonoBehaviour
{
    private Rigidbody rg;
    private CapsuleCollider col;
    public GameObject trailParticle;
    public float startForce;
    private float weight;
    public bool firlat;

    public ParticleSystem aa;
    public ParticleSystem ab;

    private Animator anim;

    private float adamSpeed;

    public Transform toFollow;

    public TextMeshPro text;
    private Emoji emoji;
    private Animator textAnim;

    [SerializeField] private float colliderRadius;
    [SerializeField] private float colliderHeight;

    

    void Start()
    {
        rg = GetComponent<Rigidbody>();
        rg.isKinematic = true;
        col = GetComponent<CapsuleCollider>();
        
        col.isTrigger = true;

        weight = GameManager.instance.kardanAdamWeight;
        adamSpeed = GameManager.instance.kardanAdamSnakeEffectSpeed;
        anim = GetComponentInChildren<Animator>();
        aa.Play();
        trailParticle.SetActive(false);

        textAnim = text.GetComponent<Animator>();
        //text.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!rg.isKinematic)
        {
            if (!firlat)
            {
                /*rg.velocity += Vector3.up * Physics.gravity.y * (weight - 1) * Time.deltaTime;
                if (transform.localPosition.z > 0f)
                    KardanAdamManager.instance.UpdateKardanAdams();*/

                if (toFollow != null)
                {
                    Vector3 dir = new Vector3(toFollow.position.x - transform.position.x, 0, 0);

                    rg.MovePosition(transform.position + dir * Time.deltaTime * adamSpeed);
                }
            }
                
        }
    }
    /*void asdas()
    {
        rg.AddForce(Vector3.forward * startForce, ForceMode.Impulse);
    }*/
    public void Firlat(Vector3 engel, float force)
    {
        firlat = true;
        float rise = GameManager.instance.kardanAdamRiseValue;
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
        float currentY = transform.localPosition.y;
        this.transform.DOLocalMoveY(currentY + rise, 1).From(currentY).SetEase(Ease.InOutQuart).OnComplete(() => {
            //rg.isKinematic = false;
            //col.isTrigger = false;
            LockConstraintsForAll(false);
            trailParticle.SetActive(true);
            this.transform.SetParent(null);
            Vector3 direction = engel - this.transform.position;
            rg.velocity = Vector3.zero;
            rg.AddForce(direction * force, ForceMode.Force);
            SoundManager.instance.PlayMusic(Soundlar.AdamFirlatma);
        });

    }

    public async void FinalMove(Vector3 engelF)
    {
        firlat = true;
        float riseF = GameManager.instance.kardanAdamRiseValue;
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
        this.transform.SetParent(null);
        float currentYF = transform.position.y;
        float duration = FinalManager.instance.finalGoDuration;
        Sequence seq = DOTween.Sequence();

        trailParticle.SetActive(true);

        seq.Append(this.transform.DOMoveY(currentYF + riseF, 1).From(currentYF).SetEase(Ease.InOutQuart));
        seq.Append(this.transform.DOMove(engelF + Vector3.up, duration).SetEase(Ease.InOutQuart)).OnComplete(() => 
            {
                FinalManager.instance.Completed();
            });
        await Task.Delay(1800);
        SoundManager.instance.PlayMusic(Soundlar.AdamFirlatma);
        var transposer = KarakterKontrol.instance.cam.GetCinemachineComponent<CinemachineTransposer>();
        DOTween.To(() => transposer.m_FollowOffset, x => transposer.m_FollowOffset = x, new Vector3(0, 15, -15), duration);
    }


    public void AdamPicked()
    {
        rg.isKinematic = false;
        col.isTrigger = false;
        col.radius = colliderRadius;
        col.height = colliderHeight;
        //trailParticle.SetActive(true);
        aa.Stop();
        if (toFollow != null)
            LockConstraintsForMoveable(true);
        else
            LockConstraintsForAll(true);

        ab.transform.DOScale(1.1f, 0.1f).From(1f).SetDelay(0.4f).OnComplete(() => { ab.Play(); });
    }

    private void LockConstraintsForAll(bool locked)
    {
        if (locked)
            rg.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        if (!locked)
            rg.constraints = RigidbodyConstraints.None;
    }

    private void LockConstraintsForMoveable(bool locked)
    {
        if (locked)
            rg.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY
                | RigidbodyConstraints.FreezePositionZ;
        if (!locked)
            rg.constraints = RigidbodyConstraints.None;
    }

    public void DestroyMe()
    {
        KardanAdamManager.instance.kardanAdamlar.Remove(this.gameObject);
        KardanAdamCounterManager.instance.CounterAzalt();
        SoundManager.instance.PlayMusic(Soundlar.AdamLose);
        Destroy(this.gameObject);
    }

    public void ChangeEmoji(Emoji emoji)
    {
        this.emoji = emoji;
        //text.gameObject.SetActive(true);
        switch (this.emoji)
        {
            case Emoji.Gozluklu:
                text.text = "<sprite=3>";
                break;
            case Emoji.Gulen:
                text.text = "<sprite=4>";
                break;
            case Emoji.Mutlu:
                text.text = "<sprite=0>";
                break;
            case Emoji.Bayildim:
                text.text = "<sprite=2>";
                break;
            default:
                break;
        }

        textAnim.Play("EmojiAnim");

        print("animPlayed");
        //text.DOFade(1f, 0.5f).From(0f).SetLoops(4, LoopType.Yoyo).OnComplete(() => text.gameObject.SetActive(false));

    }

    public void ResetAnim()
    {
        textAnim.Play("None");
    }

    public void HangAround()
    {
        toFollow = null;
        LockConstraintsForAll(false);
        this.transform.SetParent(null);
        rg.isKinematic = false;
        col.isTrigger = false;
        anim.enabled = false;
        rg.AddExplosionForce(5, KarakterKontrol.instance.transform.position, 5);
    }


}
