using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickStart()
    {
        SceneManager.LoadScene("GAMESCENE");
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("level", 1);
    }
}
