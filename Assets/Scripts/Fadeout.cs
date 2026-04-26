using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Fadeout : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("Death");
    }
    public IEnumerator Death()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject, 0.15f);
    }
}
