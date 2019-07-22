using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    //타이틀 화면에서 시작 버튼
    public void StartGame()
    {
        SceneManager.LoadScene("BattleScene");
    }

    //게임 도중에 종료버튼 눌렀을 때
    public void QuitGame()
    {
        StopAllCoroutines();
        SceneManager.LoadScene("Title");
    }

    //게임 끝까지 하고 나서 결과 화면 로드
    public static void Result()
    {
        SceneManager.LoadScene("Result");
    }
}
