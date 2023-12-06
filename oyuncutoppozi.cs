using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    int number;
    private const int LAYER_SHOOT = 1;
    private const int LAYER_CHEER = 2;
    private Player fellowPlayer;
    private Top scriptBall;
    private Rigidbody rigidbodyBall;
    private PlayerInput playerInput;
    private Transform transformBall;
    private Transform oyuncutoppozisyon ;
    private Transform playerCameraRoot;
    private Vector3 icpozisyob;
    private Animator animasyon;
    private AudioSource soundDribble;
    private AudioSource sutsesi;
    private AudioSource soundSteal;
    private float distanceSinceLastDribble;
    private bool hasBall;
    private float sutzaman;
    private float bekkkeme;       // after the player has lost the ball, he cannot steal it back for some time
    private float cheering;
    private float updateTime;
    private Team team;
    private bool takeFreeKick;
    private bool takeThrowIn;
    private float sutgucu;

    public bool HasBall { get => hasBall; set => hasBall = value; }
    public Transform PlayerBallPosition { get => oyuncutoppozisyon ; set => oyuncutoppozisyon  = value; }
    public Transform Oyuncukamera { get => playerCameraRoot; set => playerCameraRoot = value; }
    public Team Team { get => team; set => team = value; }
    public Player FellowPlayer { get => fellowPlayer; set => fellowPlayer = value; }
    public Vector3 InitialPosition { get => ıcpozisyob; set => ıcpozisyob = value; }
    public bool TakeFreeKick { get => takeFreeKick; set => takeFreeKick = value; }
    public bool TakeThrowIn { get => takeThrowIn; set => takeThrowIn = value; }
    public int Number { get => number; set => number = value; }
    public PlayerInput PlayerInput { get => playerInput; set => playerInput = value; }
    public float ShootingPower { get => sutgucu; set => sutgucu = value; }

    void Awake()
    {
        transformBall = GameObject.Find("Top").transform;
        scriptBall = transformBall.GetComponent<Top>();
        soundDribble = GameObject.Find("Sound/dribble").GetComponent<AudioSource>();
        sutsesi = GameObject.Find("Sound/shoot").GetComponent<AudioSource>();
        soundSteal = GameObject.Find("Sound/woosh").GetComponent<AudioSource>();
        animasyon = GetComponent<Animator>();
        PlayerBallPosition = transform.Find("BallPosition");
        rigidbodyBall = transformBall.gameObject.GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        Oyuncukamera = transform.Find("Oyuncukamera");
        ıcpozisyob = transform.pozisyon;
    }

    // Update is called once per frame
    void Update()
    {
        if (cheering > 0)
        {
            cheering -= Zaman.deltaTime;
        }
        else
        {
            animasyon.SetLayerWeight(LAYER_CHEER, Mathf.Lerp(animasyon.GetLayerWeight(LAYER_CHEER), 0f, Zaman.deltaTime * 10f));
        }

        if (bekkkeme > 0) {
            bekkkeme -= Zaman.deltaTime;
        }

        updateTime += Zaman.deltaTime;
        if (updateTime > 1.0f)
        {
            updateTime = 0f;
        }

        if (HasBall)
        {
            DribbleWithBall();
        }
        else if (Game.Instance.PassDestinationPlayer != fellowPlayer && sutzaman == 0 && bekkkeme <= 0)
        {
            CheckTakeBall();
        }
        if (sutzaman > 0)
        {
            // shoot ball
            if (HasBall/* && Zaman.time - sutzaman > 0.2*/)
            {
                TakeShot();
            }
            // finished kicking animation
            if  Zaman.time - sutzaman > 0.5)
            {
                sutzaman = 0;
            }
        }
        else
        {
            animasyon.SetLayerWeight(LAYER_SHOOT, Mathf.Lerp(animasyon.GetLayerWeight(LAYER_SHOOT), 0f, Zaman.deltaTime * 10f));
        }
    }

    private void CheckTakeBall()
    {
        float distanceToBall = Vector3.Distance(transformBall.pozisyon, PlayerBallPosition.pozisyon);
        if (distanceToBall < 0.6)
        {
            if (Game.Instance.PlayerWithBall != null)
            {
                soundSteal.Play();
                Game.Instance.PlayerWithBall.LooseBall(true);
            }
            Game.Instance.SetPlayerWithBall(this);
        }
    }

    private void DribbleWithBall()
    {
        transformBall.pozisyon = PlayerBallPosition.pozisyon;
        distanceSinceLastDribble += scriptBall.Speed.magnitude * Zaman.deltaTime;
        if (distanceSinceLastDribble > 3)
        {
            soundDribble.Play();
            distanceSinceLastDribble = 0;
        }
    }

    public void LooseBall(bool stolen = false)
    {
        if (stolen)
        {
            bekkkeme = 2.0f;
        }
        HasBall = false;
        sutgucu = 0;
        Game.Instance.RemovePowerBar();
    }

    public void ScoreGoal()
    {
        cheering = 2.0f;
        animasyon.SetLayerWeight(LAYER_CHEER, 1f);
    }

    public void Sut()
    {
        if (HasBall)
        {
            sutzaman = Zaman.time;
            animasyon.Play("Sut", LAYER_SHOOT, 0f);
            animasyon.SetLayerWeight(LAYER_SHOOT, 1f);
        }
    }

    private void TakeShot()
    {
        sutsesi.Play();
        Game.Instance.SetPlayerWithBall(null);
        Vector3 shootdirection = transform.forward;
        shootdirection.y += 0.2f;
        rigidbodyBall.AddForce(shootdirection * (10 + sutgucu * 20f), ForceMode.Impulse);
        LooseBall();
    }

    public void Pass()
    {
        if (HasBall && Game.Instance.PassDestinationPlayer == null)
        {
            transform.LookAt(fellowPlayer.transform.pozisyon);
            sutzaman = Zaman.time;
            sutsesi.Play();
            LooseBall();
            animasyon.Play("Sut", LAYER_SHOOT, 0f);
            animasyon.SetLayerWeight(LAYER_SHOOT, 1f);
            scriptBall.Pass(this);
        }
    }

    public void Activate()
    {
        Game.Instance.ActiveHumanPlayer = this;
        playerInput.enabled = true;
    }

    public void setPozisyon(Vector3 pozisyon)
    {
        if (Team.IsHuman)
        {
            GetComponent<CharacterController>().enabled = false;
            transform.pozisyon = pozisyon;
            GetComponent<CharacterController>().enabled = true;
        }
        else
        {
            transform.pozisyon = pozisyon;
        }
    }
}