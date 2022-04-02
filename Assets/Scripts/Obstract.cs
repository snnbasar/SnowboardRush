using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Obstract : MonoBehaviour
{
    public GameObject fracturedObstract;
    public bool kirilmayaHazir; // sets on kardanadammanager

    public MMFeedbacks KirilmaEfecti;

    public ParticleSystem spikeParticle;
    public Material icedMat;
    public GameObject ui;
    private int maxHealth;
    public int currentHit { get { return _currentHit; } 
        set { 
            _currentHit = value;
            if (_currentHit >= maxHealth)
            {
                this.GetComponentInChildren<Renderer>().material = icedMat;
                kirilmayaHazir = true;
                ui.SetActive(false);
            }
        } }
    public int _currentHit;

    private void Start()
    {
        maxHealth = GameManager.instance.countOfFirlatilacakKardanAdam;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (kirilmayaHazir && collision.transform.CompareTag("Player"))
        {
            SoundManager.instance.PlayMusic(Soundlar.DestroyWall);
            Destruction(KarakterKontrol.instance.transform);
        }
        if (collision.transform.CompareTag("kardanadam"))
        {
            currentHit++;
            spikeParticle.Play();
            SoundManager.instance.PlayMusic(Soundlar.WallToIce);
            collision.transform.GetComponent<Adam>().DestroyMe();
        }
        if (!kirilmayaHazir && collision.transform.CompareTag("Player") && !GameManager.instance.gameMod) 
        {
            SoundManager.instance.PlayMusic(Soundlar.WallOnDontBreak);
            KarakterKontrol.instance.KillMe();
        }
            
    }



    private void Destruction(Transform player)
    {
        GameObject obj = Instantiate(fracturedObstract, transform.position, transform.rotation);
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            obj.transform.GetChild(i).GetComponent<Rigidbody>().AddExplosionForce(GameManager.instance.obstractExplosionForce,
                player.position, GameManager.instance.obstractExplosionForce);
        }
        KirilmaEfecti.transform.SetParent(null);
        KirilmaEfecti.PlayFeedbacks();
        KarakterKontrol.instance.AddForce(GameManager.instance.forceAfterBreakingWalls);
        this.gameObject.SetActive(false);
    }

}
