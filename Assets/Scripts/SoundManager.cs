using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Soundlar
{
    PickUp,
    Jump,
    AdamFirlatma,
    Drop,
    WallToIce,
    WallOnDontBreak,
    DestroyWall,
    PickUpGem,
    AdamLose

}
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<AudioSource> sounds = new List<AudioSource>();
    public List<AudioSource> musics = new List<AudioSource>();
    public bool playBeratPlaylist;
    public List<AudioSource> beratPlaylist = new List<AudioSource>();
    private Soundlar soundlar;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            sounds.Add(transform.GetChild(i).GetComponent<AudioSource>());
        }

        Transform musicObject = transform.GetChild(0).GetChild(0);
        for (int i = 0; i < musicObject.childCount; i++)
        {
            musics.Add(musicObject.GetChild(i).GetComponent<AudioSource>());
        }
        PlayRandomMusic();
    }

    private void PlayRandomMusic()
    {
        int rndmMusic;
        if (playBeratPlaylist)
        {
            rndmMusic = Random.Range(0, beratPlaylist.Count);
            beratPlaylist[rndmMusic].Play();
        }
        else
        {
            rndmMusic = Random.Range(0, musics.Count);
            musics[rndmMusic].Play();
        }
            
    }

   

    public void PlayMusic(Soundlar soundfx)
    {
        soundlar = soundfx;
        SoundsIndexes();
    }

    private void SoundsIndexes()
    {
        switch (soundlar)
        {
            case Soundlar.AdamFirlatma:
                sounds[1].Play();
                break;
            case Soundlar.Drop:
                sounds[2].Play();
                break;
            case Soundlar.Jump:
                sounds[3].Play();
                break;
            case Soundlar.PickUp:
                sounds[4].Play();
                break;
            case Soundlar.WallOnDontBreak:
                sounds[5].Play();
                break;
            case Soundlar.WallToIce:
                sounds[6].Play();
                break;
            case Soundlar.DestroyWall:
                sounds[7].Play();
                break;
            case Soundlar.PickUpGem:
                sounds[8].Play();
                break;
            case Soundlar.AdamLose:
                sounds[9].Play();
                break;
            default:
                break;
        }
    }

}
