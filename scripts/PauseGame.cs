using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    bool GameIsPaused = false;
    [SerializeField] GameObject pauseMenuCanvas;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseOrResume();
        }
    }

    public void PauseOrResume()
    {
        GameIsPaused = !GameIsPaused;
        pauseMenuCanvas.SetActive(GameIsPaused);
        Time.timeScale = GameIsPaused ? 0f : 1f;
        Cursor.visible = GameIsPaused;
    }
}
