using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NewRecord : MonoBehaviour
{
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject newRecordCanvas;
    [SerializeField] GameObject winScreenCanvas;
    [SerializeField] TMP_InputField textBox;
    [SerializeField] Button submitButton;

    void Start()
    {   
        
        if(FindObjectOfType<Score>().IsHighScore())
        {
            SetNewRecordActive();
        }
        else if(FindObjectOfType<LevelManager>().GetIsGameWon())
        {
            SetWinScreenActive();
        }
        else
        {
            SetGameOverActive();
        }
    }

    void Update()
    {
        if(textBox.text.Length < 3)
        {
            submitButton.interactable = false;
        }
        else
        {
            submitButton.interactable = true;
        }
    }

    public void SubmitHighScore()
    {
        PlayerPrefs.SetString("name" + PlayerPrefs.GetInt("currentRank").ToString(), textBox.text);
        if(FindObjectOfType<LevelManager>().GetIsGameWon())
        {
            SetWinScreenActive();
        }
        else
        {
            SetGameOverActive();
        }
    }

    void SetNewRecordActive()
    {
        newRecordCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);
        winScreenCanvas.SetActive(false);
    }

    void SetGameOverActive()
    {
        newRecordCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        winScreenCanvas.SetActive(false);
    }

    void SetWinScreenActive()
    {
        newRecordCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        winScreenCanvas.SetActive(true);
    }
}
