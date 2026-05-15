using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SoundDestroy : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("Doom");
    }

    void Update()
    {
        
    }
    public IEnumerator Doom()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
