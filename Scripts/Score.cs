using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    int playerScore;

    public int PlayerScore 
    {
        get{return playerScore;}
        set{playerScore = value;}
    }

    static Score instance;

    void Awake()
    {
        ManageSingleton();
    }

    void ManageSingleton()
    {
        if(instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            playerScore = 0;
        }
    }

    public void ResetPlayerScore()
    {
        playerScore = 0;
    }

    public bool IsHighScore()
    {
        for(int i = 0; i < 5; i++)
        {
            if(playerScore > PlayerPrefs.GetInt("highScore" + i.ToString(), 0))
            {
                int j = 4;
                while(j >= i)
                {
                    PlayerPrefs.SetString("name"   + (j+1).ToString(), PlayerPrefs.GetString("name" + j.ToString(), "-"));
                    PlayerPrefs.SetInt("highScore" + (j+1).ToString(), PlayerPrefs.GetInt("highScore" + j.ToString(), 0));
                    j--;
                }
                PlayerPrefs.SetInt("highScore" + i.ToString(), playerScore);
                PlayerPrefs.SetInt("currentRank", i);
                return true;
            }
        }    
        return false; 
    }
}
