using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void RecordResults()
    {
        YouArray = BattleManager.inst.YouArray;
        for(int i = 0; i < 4; i++)
        {
            DeathCounts[i] = Player.players[i].deathCount;
        }
    }

    int[] YouArray;
    int[] DeathCounts = new int[4];

}
