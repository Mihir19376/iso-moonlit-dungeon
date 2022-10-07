using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject pauseMenu;

    public GameManager gameManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        gameManager.isPlaying = false;
        Time.timeScale = 0f;
        gameUI.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void UnPauseGame()
    {
        gameManager.isPlaying = true;
        Time.timeScale = 1;
        gameUI.SetActive(true);
        pauseMenu.SetActive(false);
    }
}
