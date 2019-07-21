using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public static void StartGame()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public static void QuitGame()
    {
        SceneManager.LoadScene("Title");
    }

    public static void Result()
    {
        SceneManager.LoadScene("Result");
    }
}
