using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] names;
    [SerializeField] TextMeshProUGUI[] scores;

    void Start()
    {
        DisplayHighScore();
    }

    void DisplayHighScore()
    {
        for(int i = 0; i < 5; i++)
        {
            names[i].text = PlayerPrefs.GetString("name" + i.ToString(), "-");
            scores[i].text = PlayerPrefs.GetInt("highScore" + i.ToString(), 0).ToString();
        }
    }

}
