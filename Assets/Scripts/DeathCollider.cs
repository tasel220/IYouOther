using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        var p = collision.gameObject.GetComponent<Player>();
        if (p != null)
            StartCoroutine(p.Death());
    }
}
