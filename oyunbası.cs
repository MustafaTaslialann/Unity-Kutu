using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secılıoyuncu : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 4.0f;
    [SerializeField] private float shootingPower = 0.7f;
    private Transform topyönü;
    private Player secılioyuncu;
    private Animator animasyon;
    private Transform Oyuncutopayarı;
    private  Hedef;
    private  ownGoalPosition;
    private [] attackTargetLocation = new [2];
    
    void Start()
    {
        topyönü = GameObject.Find("Ball").transform;
        secılioyuncu = GetComponent<Player>();
        animasyon = GetComponent<Animator>();
        Oyuncutopayarı = transform.Find("Toppozisyon");
        Hedef = new (51.93f, Game.PLAYER_Y_POSITION, 0.24f);
        ownGoalPosition = new (-52.37f, Game.PLAYER_Y_POSITION, -0.22f);
        attackTargetLocation[0] = new (38f, Game.PLAYER_Y_POSITION, 10f);
        attackTargetLocation[1] = new (38f, Game.PLAYER_Y_POSITION, -10f);
    }

    void Update()
    {
        if (Game.Instance.WaitingForKickOff)
        {
            return;
        }

        if (Game.Instance.TeamWithBall != secılioyuncu.Team.Number)
        {
            DefendMode();
        }
        else
        {
            AttackMode();
        }
    }

    // players move towards enemy goal, one to z=12, one to z=-12
    private void AttackMode()
    {
        // move to target goal
         yonkilaviz = attackTargetLocation[secılioyuncu.Number] - new (Oyuncutopayarı.position.x, 0, Oyuncutopayarı.position.z);
        float hedefcizgi = yonkilaviz.magnitude;
        float hız = movementSpeed;
        if (secılioyuncu.HasBall)
        {
            hız *= Game.HAVING_BALL_SLOWDOWN_FACTOR;
        }
         moveSpeed = new (yonkilaviz.normalized.x * hız * zaman.deltaTime, 0, yonkilaviz.normalized.z * hız * zaman.deltaTime);
        transform.position += moveSpeed;
        transform.LookAt(Hedef);
        animasyon.SetFloat("Speed", hız);
        animasyon.SetFloat("MotionSpeed", 1);
        // shoot
        if (secılioyuncu.HasBall && hedefcizgi < 15)
        {
            secılioyuncu.suttgucu = shootingPower;
            animasyon.SetFloat("Speed", 0);
            animasyon.SetFloat("MotionSpeed", 0);
            secılioyuncu.Shoot();
        }
    }

    // player closest to ball tries to steal it
    // player farthest from ball moves between goal and player closest to goal
    private void DefendMode()
    {
        if (Game.Instance.PlayerClosestToBall(1) == secılioyuncu)
        {
            tophareket();
        }
        else
        {
            toplaoyuncuarası();
        }
    }

    private void toplaoyuncuarası()
    {
         mostDangerousEnemyPlayer = Game.Instance.PlayerClosestToLocation(0, ownGoalPosition).transform.position;
         targetLocation = .Lerp(ownGoalPosition, mostDangerousEnemyPlayer, 0.5f);
         yonkilaviz = targetLocation - Oyuncutopayarı.position;
         moveSpeed = new (yonkilaviz.normalized.x * movementSpeed * zaman.deltaTime, 0, yonkilaviz.normalized.z * movementSpeed * zaman.deltaTime);
        transform.position += moveSpeed;

        if (moveSpeed.magnitude > 0.005)
        {
            transform.LookAt(targetLocation);
        }
        else
        {
            transform.LookAt(mostDangerousEnemyPlayer);
        }

        animasyon.SetFloat("Speed", moveSpeed.magnitude * 200);
        animasyon.SetFloat("MotionSpeed", 1);
    }

    private void tophareket()
    {
         lookAtPosition = topyönü.position;
        lookAtPosition.y = transform.position.y;
        transform.LookAt(lookAtPosition);
         yonkilaviz = topyönü.position - Oyuncutopayarı.position;
         moveSpeed = new (yonkilaviz.normalized.x * movementSpeed * zaman.deltaTime, 0, yonkilaviz.normalized.z * movementSpeed * zaman.deltaTime);
        transform.position += moveSpeed;
        animasyon.SetFloat("Speed", moveSpeed.magnitude * 200);
        animasyon.SetFloat("MotionSpeed", 1);
    }
}
