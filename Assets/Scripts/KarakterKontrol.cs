using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;
using MoreMountains.Feedbacks;
using DitzeGames.MobileJoystick;

public class KarakterKontrol : MonoBehaviour
{
    public static KarakterKontrol instance;

    private Rigidbody rg;
    public float speed = 5f;
    public float sensitivity;
    public float limitHorizontalMovement;
    public float breakForceMultiplier;

    public float fallMultiplayer;
    [SerializeField] private float velo;
    public GameObject particle;
    public bool canMove;

    public MMFeedbacks DropFeedback;

    [SerializeField] private float rampForce;
    [SerializeField] private float startForceMultiplier;

    private float limitVelocity;

    private Animator anim;
    private CinemachineImpulseSource impulse;
    public CinemachineVirtualCamera cam;
    public Joystick joystick;
    public RagdollManager ragdollManager;

    private Vector2 startPos;
    //float distToGround;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rg = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        impulse = GetComponent<CinemachineImpulseSource>();
        particle.SetActive(false);

        ragdollManager.DoRagdoll(false);
        CanMove(false);
        AddForce(rampForce * startForceMultiplier);

        limitVelocity = GameManager.instance.limitKarakterVelocity;
    }


    void FixedUpdate()
    {

        if (canMove)
            Move();
        velo = rg.velocity.magnitude;
        if(velo > limitVelocity)
        {
            float brakeSpeed = velo - limitVelocity;  // calculate the speed decrease

            Vector3 normalisedVelocity = rg.velocity.normalized;
            Vector3 brakeVelocity = normalisedVelocity * brakeSpeed * breakForceMultiplier;  // make the brake Vector3 value

            rg.AddForce(-brakeVelocity);  // apply opposing brake force
        }
    }

    
    private void Move()
    {
        float hor = Input.GetAxis("Horizontal") * speed + joystick.AxisNormalized.x * sensitivity;
        float ver = Input.GetAxis("Vertical") + 1;

        /*if (Input.GetMouseButtonDown(0))
        {
            joystick.transform.position = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            //joystick.transform.localPosition = Vector2.zero;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if(touch.phase == TouchPhase.Began)
            {
                //startPos = touch.position;
                joystick.transform.position = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                //joystick.transform.localPosition = Vector2.zero;
            }*/
            /*Vector2 direction = touch.position - startPos;
            
            if(direction.x >= -100 && direction.x <= 100)
                hor = direction.normalized.x / 3;
            else
                hor = direction.normalized.x;

            hor *= sensitivity;

            if (hor < -limitHorizontalMovement)
                hor = -limitHorizontalMovement;
            else if (hor > limitHorizontalMovement)
                hor = limitHorizontalMovement;
            print(hor);*/
        //}

        Vector3 movement = new Vector3(hor, 0, ver * speed);
        movement = transform.TransformDirection(movement);

        rg.MovePosition(transform.position + movement * Time.deltaTime);

        if (rg.velocity.y < 0)
            rg.velocity += Vector3.up * Physics.gravity.y * (fallMultiplayer - 1) * Time.deltaTime;

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("ramp"))
        {
            transform.rotation = Quaternion.Euler(-20, 0, 0);
            anim.SetBool("jumpAnim", false);
            particle.SetActive(false);
            KardanAdamManager.instance.ChangeKardanAdamAnimationToJump(false);
        }
        if (collision.transform.CompareTag("plane"))
        {
            transform.rotation = collision.transform.rotation;
            anim.SetBool("jumpAnim", false);
            //impulse.GenerateImpulse();
            ChangeNoise(0, 0);
            SoundManager.instance.PlayMusic(Soundlar.Drop);
            DropFeedback.PlayFeedbacks();
            Debug.Log("deprem");
            particle.SetActive(true);
            KardanAdamManager.instance.ChangeKardanAdamAnimationToJump(false);

            EmojiManager.instance.DoVeloBasedCompliment(velo);
            print(velo);
        }

    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("ramptrigger"))
        {
            AddForce(rampForce);
            anim.SetBool("jumpAnim", true);
            int rndm = GetRandomInt(0, 4);
            Debug.Log(rndm);
            anim.SetInteger("randomAnimInt", rndm);
            SoundManager.instance.PlayMusic(Soundlar.Jump);
            WaitASecForAnimToGetIdle();
            ChangeNoise(GameManager.instance.amplitudeGainOnFly, GameManager.instance.frequencyGainOnFly);
            KardanAdamManager.instance.ChangeKardanAdamAnimationToJump(true);
        }
    }

    private async void WaitASecForAnimToGetIdle()
    {
        await Task.Delay(500);
        anim.SetBool("jumpAnim", false);
    }

    private void ChangeNoise(float amp, float freq)
    {
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amp;
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = freq;
    }

    private int GetRandomInt(int min, int max)
    {
        return Random.Range(min, max);
    }

    public void AddForce(float force)
    {
        rg.AddForce(Vector3.forward * force, ForceMode.Impulse);
    }

    public void CanMove(bool moveable)
    {
        canMove = moveable;
        rg.isKinematic = !moveable;
        if(moveable)
            AddForce(rampForce * startForceMultiplier);
    }

    public void KillMe()
    {
        CanMove(false);
        ragdollManager.DoRagdoll(true);
        Transform ragdolledchar = transform.GetChild(0);
        cam.LookAt = ragdolledchar.GetChild(0);
        //cam.Follow = ragdolledchar;
        KardanAdamManager.instance.OnPlayerDied();
        GameManager.instance.OnGameOver();
    }


}
