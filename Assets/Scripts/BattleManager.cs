using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager inst;

    public Platform[] Platforms;

    public GameObject[] Item = new GameObject[2];
    public GameObject[] SpawnPoint = new GameObject[4];

    public int numOfPlayers;

    private void Awake()
    {
        playerN = numOfPlayers;
        inst = this;
        IArray = new int[playerN];
        AssignI();
        StartCoroutine(BattlePeriod());
    }

    public static int playerN = 4;
    public int[] IArray;

    private void AssignI()
    {
        bool[] filled = new bool[playerN];
        for (int i = 0; i < playerN; i++) filled[i] = false;

        for (int value = 0; value < playerN - 1; value++)
        {
            int remainingN = playerN - 1;
            for (int i = 0; i < playerN; i++) if (i == value || filled[i]) remainingN--;
            int x = Random.Range(0, remainingN);
            for (int i = 0; i < remainingN; i++) if (i == value || filled[i]) x++;
            IArray[x] = value;
            filled[x] = true;
        }
        if(!filled[playerN - 1])
        {
            int index = Random.Range(0, playerN - 1);
            IArray[playerN - 1] = IArray[index];
            IArray[index] = playerN - 1;
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
        for (int i = 0;  i< playerN; i++)
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
        int lastPoint = 0;
        x[0] = Random.Range(0, lastPoint);
        for (int i = 0; i < num; i++)
        {
            lastPoint = Random.Range(lastPoint, max + i + 2 - num);
            x[i] = lastPoint;
            lastPoint++;
        }
        return x;
    }

    public Vector3 RandomSafeSpawnPoint()
    {
        List<Platform> remainingPlatforms = new List<Platform>();
        for (int i = 0; i < 4; i++)
        {
            if (!(Platforms[i].submerged))
            {
                remainingPlatforms.Add(Platforms[i]);
            }
        }
        return remainingPlatforms[Random.Range(0, remainingPlatforms.Count)].SpawnPoint;
    }

    public void KillPlayer(Player p)
    {
        StartCoroutine(p.Death());
    }
}
