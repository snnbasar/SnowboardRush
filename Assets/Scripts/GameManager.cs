using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Karakter Settings")]
    public float slowTimeEffectDuration;
    public float limitKarakterVelocity;
    public bool gameMod;

    [Header("Kardan Adam Settings")]
    public int countOfFirlatilacakKardanAdam = 2;
    public float kardanAdamFirlatmaGucu = 100f;
    public float kardanAdamSnakeEffectSpeed;
    public float kardanAdamWeight;
    public float kardanAdamRiseValue;
    public float kardanAdamTargetSpread;

    [Header("Engel Settings")]
    public int obstractExplosionForce;
    public int obstractExplosionRadius;
    public float forceAfterBreakingWalls;
    public float destroyFractalsAfterSeconds;
    [Header("Camera Settings")]
    public float amplitudeGainOnFly;
    public float frequencyGainOnFly;

    [Header("Yildiz Ayarý")]
    public int star1;
    public int star2;
    public int star3;

    [Header("Menü Status")]
    public bool ses;
    public bool play;

    [Header("Eventler")]
    public UnityEvent startGame;
    public UnityEvent stopGame;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
#if UNITY_EDITOR
        Application.targetFrameRate = 300;
#else
        Application.targetFrameRate = 60;
        Screen.SetResolution(720, 1280, true);
#endif

        startGame.AddListener(() => { KarakterKontrol.instance.CanMove(true); });
        stopGame.AddListener(() => { KarakterKontrol.instance.CanMove(false); });
    }

    public async void TimeScaleSlow(float howmuch)
    {
        while (Time.timeScale > howmuch)
        {
            Time.timeScale -= howmuch * slowTimeEffectDuration;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            Debug.Log("slowing");
            await Task.Yield();
        }
        TimeScaleNormal(howmuch);
    }

    public async void TimeScaleNormal(float howmuch)
    {
        while (Time.timeScale < 1)
        {
            Time.timeScale += howmuch * slowTimeEffectDuration * 2;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            Debug.Log("reverting");
            await Task.Yield();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangePlayMode()
    {
        if (!play)
            StartGame();
        if (play)
            StopGame();
        play = !play;
    }
    public void StartGame()
    {
        startGame?.Invoke();
        Time.timeScale = 1;
        UIManager.instance.playButton.SetActive(true);
    }

    public void StopGame()
    {
        stopGame?.Invoke();
        Time.timeScale = 0;
        UIManager.instance.playButton.SetActive(true);
    }

    public void OnLevelComplete()
    {
        UIManager.instance.OnLevelComplete(true);
    }

    public void OnGameOver()
    {
        UIManager.instance.OnGameOver(true);
    }

    public void NextLevel()
    {
        print("Next Level Requested");
        RestartGame();
    }

    public void ChangeSound()
    {
        if (ses)
            AudioListener.pause = false;
        else
            AudioListener.pause = true;
        ses = !ses;
    }
}
