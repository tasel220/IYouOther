using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    private void Awake()
    {
        if (inst != null) Destroy(inst.gameObject);
        inst = this;
    }

    private void Assign()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
