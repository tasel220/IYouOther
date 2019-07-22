using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager inst;

    public Platform[] Platforms;

    public GameObject[] Item = new GameObject[2];
    public GameObject[] SpawnPoint = new GameObject[4];

    private void Awake()
    {
        inst = this;
        //AssignYou();
        //for (int i = 0; i < playerN; i++) Debug.Log(YouArray[i]);
        StartCoroutine(BattlePeriod());
        //Debug.Log(Input.GetJoystickNames()[0]);
    }

    private void Start()
    {
        
    }

    public static int playerN = 4;
    public int[] IArray = new int[playerN];
    bool[] used = new bool[playerN];
    private void AssignI()
    {
        for (int i = 0; i < playerN; i++) used[i] = false;
        int first = Random.Range(1, playerN);
        used[first] = true;
        IArray[0] = first;
        rec(0);
    }

    void rec(int ind)
    {
        for (int i = 0; i < playerN; i++)
        {
            if (!used[i] && i != ind)
            {
                used[i] = true;
                IArray[ind] = i;
                if (ind + 1 < playerN) rec(ind + 1);
                used[i] = false;
            }
        }
    }

    IEnumerator BattlePeriod()
    {
        Player.Initiate();
        yield return new WaitForSeconds(5f);

        for(int quarter = 1; quarter <= 4; quarter++)
        {
            yield return new WaitForSeconds(5f);
            SpawnItems(quarter < 4 ? 2 : 1);
            yield return new WaitForSeconds(5f);
            PlatformHazard(quarter != 4 ? quarter : 3);
            yield return new WaitForSeconds(5f);
        }
        Player.Finish();
        yield return new WaitForSeconds(0.5f);
        Finish();
    }

    void Finish()
    {
        Record.inst.RecordResults();
        StopAllCoroutines();
        SceneMgr.Result();
    }

    private void SpawnItems(int num)
    {
        if(num ==1)
        {
            for(int i = 0; ; i++)
            {
                if(!(Platforms[i].submerged))
                {
                    Instantiate(Item[Random.Range(0, 2)], SpawnPoint[i].transform.position, Quaternion.identity);
                    return;
                }
            }
        }

        List<Platform> remainingPlatforms = new List<Platform>();
        for (int i = 0;  i< 4; i++)
        {
            if (!(Platforms[i].submerged))
            {
                remainingPlatforms.Add(Platforms[i]);
            }
        }
        if (remainingPlatforms.Count == 3) remainingPlatforms.Remove(remainingPlatforms[Random.Range(0, 3)]);
        var pos1 = remainingPlatforms[0].transform.GetChild(1).position;
        var pos2 = remainingPlatforms[1].transform.GetChild(1).position;
        Instantiate(Item[1], pos1, Quaternion.identity);
        Instantiate(Item[0], pos2, Quaternion.identity);
    }

    private void PlatformHazard(int num)
    {
        var ints = SelectRandomNumbers(3, num);
        for(int i = 0; i < ints.Length; i++)
            StartCoroutine(Platforms[ints[i]].Hazard());
    }

    public static int[] SelectRandomNumbers(int max, int num)
    {
        int[] x = new int[num];
        int point = 0;
        x[0] = Random.Range(0, point);
        for (int i = 0; i < num; i++)
        {
            point = Random.Range(point, max + i + 2 - num);
            x[i] = point;
            point++;
        }
        return x;
    }

    public void KillPlayer(Player p)
    {
        StartCoroutine(p.Death());
    }
}
