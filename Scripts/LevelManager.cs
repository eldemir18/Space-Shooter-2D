using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    Score playerScore;
    static bool isGameWon;

    void Awake()
    {
        playerScore = FindObjectOfType<Score>();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGame()
    {
        playerScore.ResetPlayerScore();
        SceneManager.LoadScene("Game");
    }

    public void LoadGameOver(bool state)
    {
        isGameWon = state;
        StartCoroutine(WaitAndLoad("GameOver", levelLoadDelay));
    }

    IEnumerator WaitAndLoad(string name, float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(name);

        Cursor.visible = true;
    }

    public bool GetIsGameWon()
    {
        return isGameWon;
    }
}
