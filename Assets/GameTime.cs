using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    public bool play;
    public float timer;
    public int minutes, second;
    public Text TimeUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (play)
        {
            timer += Time.deltaTime;
            minutes = (int)timer / 60;
            second = (int)timer % 60;
            TimeUI.text = minutes.ToString() + ":" + second.ToString("00");
        }


    }
    public void GameStart()
    {
        play = true;
    }

    public void GameClear()
    {
        play = false;
    }
}
