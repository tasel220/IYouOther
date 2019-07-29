using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Vector3 originalPos;
    Animator animator;
    [HideInInspector]
    public Vector3 SpawnPoint;
    public Material mat;
    public bool submerged = false;

    private void Awake()
    {
        mat.color = Color.white;
        originalPos = transform.position;
        SpawnPoint = transform.GetChild(1).transform.position;
        animator = GetComponent<Animator>();
    }

    public IEnumerator Hazard()
    {
        StartCoroutine(Warn());
        yield return new WaitForSeconds(5f);
        StartCoroutine(Drown());
    }

    IEnumerator Drown()
    {
        animator.SetTrigger("Fall");
        submerged = true;
        yield return new WaitForSeconds(5f);
        submerged = false;
    }
    
    private IEnumerator Warn()
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(1f);
        mat.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        mat.color = Color.red;
        yield return new WaitForSeconds(1f);
        mat.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        //3
        mat.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        mat.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        mat.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        mat.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        mat.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        mat.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        mat.color = Color.white;
        Debug.Log("경고 끝");
    }
}
