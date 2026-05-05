using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Fadeout : MonoBehaviour
{
    public SpriteRenderer sr;
    public float AlphaRange;
    public float Speed;
    public Color sc;
    public bool Fading;

    void Start()
    {
        Speed = 0.000085f;
        StartCoroutine("Fadein");
        StartCoroutine("Death");
    }
    void Update()
    {
        if (Fading == false) 
        {
            sc = sr.color;
            sc.a += Time.deltaTime + Speed;
            sr.color = sc;
        }
        if (Fading == true)
        {
            sc = sr.color;
            sc.a -= Time.deltaTime + Speed;
            sr.color = sc;
        }
    }
    public IEnumerator Death()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject, 0.15f);
    }
    public IEnumerator Fadein()
    {
        Fading = false;
        yield return new WaitForSeconds(1.5f);
        Fading = true;
    }
}
