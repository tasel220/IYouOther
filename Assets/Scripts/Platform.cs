using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Vector3 originalPos;

    private void Awake()
    {
        originalPos = transform.position;
    }

    public IEnumerator Hazard()
    {
        Warn();
        yield return new WaitForSeconds(5f);
        StartCoroutine(Drown());
    }

    IEnumerator Drown()
    {
        transform.position = originalPos + Vector3.down * 100;
        yield return new WaitForSeconds(5f);
        transform.position = originalPos;
    }
    
    private void Warn()
    {

    }
}
