using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record : MonoBehaviour
{
    public static Record inst;
    private void Awake()
    {
        inst = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RecordResults()
    {
        IArray = BattleManager.inst.IArray;
        for(int i = 0; i < 4; i++)
        {
            if(Player.players[i] != null)
            DeathCounts[i] = Player.players[i].deathCount;
        }
    }

    public int[] IArray;
    public int[] DeathCounts = new int[4];

}
