using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void onClickMainMenu()
    {
        SceneManager.LoadScene("STARTGAME");
    }
    public void onClickRestart()
    {
        SceneManager.LoadScene("GAMESCENE");
    }
}
