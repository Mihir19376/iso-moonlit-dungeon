using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        gameUI.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        gameUI.SetActive(true);
        pauseMenu.SetActive(false);
    }
}
