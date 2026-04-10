using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerSprite : MonoBehaviour
{
    public SpriteRenderer [] Plush;
    public Sprite [] PlushStates;

        public void DamageTaken(int Damage)
    {
        if (Damage <= 4 && Damage >= 1)
        {
            Debug.Log("1-4 Damage Taken");
        }

        if (Damage <= 10 && Damage >= 5)
        {
            Debug.Log("5-10 Damage Taken");
        }

        if (Damage >= 11)
        {
            Debug.Log("11+ Damage Taken");
        }
        else
        {
            Debug.Log("0 Damage Taken");
        }
    }
}
