using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { none, ice, spike }
    public ItemType type;
    private void Awake()
    {
        if (type == ItemType.ice)
            iceModel.SetActive(true);
        else
            spikeModel.SetActive(true);
    }
    public GameObject iceModel;
    public GameObject spikeModel;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Player>().CurrentItem = type;
        Destroy(gameObject);
    }
}
