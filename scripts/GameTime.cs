using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameTime : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;

    ClassManager classManager;

    float timer;
    public float Timer
    {
        get{return timer;}
    }
    
    bool isTimerRunning = true;
    public bool IsTimerRunning
    {
        get{return isTimerRunning;}
        set{isTimerRunning = value;}
    }

    void Awake()
    {
        classManager = FindObjectOfType<ClassManager>();
    }

    void Update()
    {
        if(!classManager.IsClassSelected) return;

        if(isTimerRunning)
        {
            timer += Time.deltaTime;
        } 

        DisplayTime();
    }

    void DisplayTime()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        timeText.text = niceTime;
    }

    public void SaveTime()
    {
        PlayerPrefs.SetInt("playTime", Mathf.FloorToInt(PlayerPrefs.GetInt("playTime", 0) + timer));
    }
}
