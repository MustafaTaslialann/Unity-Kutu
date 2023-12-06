using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAKIM
{
    int SAYI;
    int score;
    List<Player> oyuncular = new List<Player>();
    bool isİnsan;
    Player playerClosestToBall;

    public TAKIM(int SAYI, bool isİnsan)
    {
        this.SAYI = SAYI;
        this.isİnsan = iİnsan;
    }

    public int SAYI { get => SAYI; set => SAYI = value; }
    public bool IsHuman { get => isİnsan; set => isİnsan = value; }
    public List<Player> Players { get => oyuncular; set => oyuncular = value; }
    public Player PlayerClosestToBall { get => playerClosestToBall; set => playerClosestToBall = value; }
    public int Score { get => score; set => score = value; }