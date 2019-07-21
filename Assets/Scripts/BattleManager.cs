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
        Debug.Log(Input.GetJoystickNames()[0]);
    }

    private void Update()
    {
        //Debug.Log(Input.GetAxis("Horizontal_0" + ", " + "Vertical_0"));
    }

    public static int playerN = 4;
    public int[] YouArray = new int[playerN];
    bool[] used = new bool[playerN];
    private void AssignYou()
    {
        for (int i = 0; i < playerN; i++) used[i] = false;
        rec(0);
    }

    void rec(int ind)
    {
        for (int i = 0; i < playerN; i++)
        {
            if (!used[i] && i != ind)
            {
                used[i] = true;
                YouArray[ind] = i;
                if (ind + 1 < playerN) rec(ind + 1);
                used[i] = false;
            }
        }
    }
   

    IEnumerator BattlePeriod()
    {
        yield return new WaitForSeconds(5f);
        Player.Initiate();
        yield return new WaitForSeconds(5f);
        yield return new WaitForSeconds(5f);
        yield return new WaitForSeconds(5f);
        yield return new WaitForSeconds(5f);
        yield return new WaitForSeconds(5f);
        yield return new WaitForSeconds(5f);
        yield return new WaitForSeconds(5f);
        yield return new WaitForSeconds(5f);
        yield return new WaitForSeconds(5f);
        yield return new WaitForSeconds(5f);
        yield return new WaitForSeconds(5f);
        yield return new WaitForSeconds(5f);
    }

    private void SpawnItem()
    {
        var pos = SpawnPoint[Random.Range(0, 3)].transform.position;
        Instantiate(Item[Random.Range(0, 1)], pos, Quaternion.identity);
    }

    private void PlatformHazard(int num)
    {
        StartCoroutine(Platforms[Random.Range(0, Platforms.Length - 1)].Hazard());
    }
}
