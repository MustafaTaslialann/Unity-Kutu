using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetector : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Ball>() != null)
        {
            if (name.Equals("Golcizgi"))
            {
                Game.Instance.Hedefcızgı(1);
            }
            else
            {
                Game.Instance.Hedefcızgı(0);
            }
        }
    }
}