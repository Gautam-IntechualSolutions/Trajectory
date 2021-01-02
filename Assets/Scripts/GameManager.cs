using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score,activeObjCount=0;
    public bool isGameOver = false,isCollide=false;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("level") == 0)
        {
            PlayerPrefs.SetInt("level", 1);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
