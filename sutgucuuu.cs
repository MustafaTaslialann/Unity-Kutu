using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oyuncu : MonoBehaviour
{
    Player scriptPlayer;
    private StarterAssetsInputs starterAssetsInputs;

    void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        scriptPlayer = GetComponent<Player>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (starterAssetsInputs.pass)
        {
            starterAssetsInputs.pass = false;
            scriptPlayer.Pass();
        }

        if (starterAssetsInputs.shoot)
        {
            if (scriptPlayer.HasBall)
            {
                scriptPlayer.sutgucu += 1.5f * Time.deltaTime;
                Game.Instance.SetPowerBar(scriptPlayer.sutgucu);
                if (scriptPlayer.sutgucu > 1)
                {
                    scriptPlayer.sutgucu = 1;
                }
            }
        }
        else if (scriptPlayer.sutgucu>0)
        {
            scriptPlayer.sutt();
        }
    }
}