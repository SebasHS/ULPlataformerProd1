using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausaJuego : MonoBehaviour
{

    private bool isPaused = false;
    public GameObject pausePanel;

    void Start()
    {
        pausePanel.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; 
        pausePanel.SetActive(true); 
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; 
        pausePanel.SetActive(false); 
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartLevel();
        ResumeGame();
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
