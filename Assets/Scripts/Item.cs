using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { none, ice, spike }
    public ItemType type;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Player>().CurrentItem = type;
        Destroy(gameObject);
    }
}
