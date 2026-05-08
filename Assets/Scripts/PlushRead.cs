using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlushRead : MonoBehaviour
{
    public SpriteRenderer [] Plush;
    public Sprite [] PlushState;
    public int PlushFront;

    void Start()
    {
        Plush[PlushFront].sprite = PlushState[0];
    }

    void Update()
    {
        Plush[PlushFront].sprite = PlushState[0];
    }
}
