using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject gameUI;
    public GameObject instructionsMenu;

    void Start()
    {
        Time.timeScale = 0f;
    }
    public void PlayGame()
    {
        Time.timeScale = 1f;
        startMenu.SetActive(false);
        gameUI.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToInsructions()
    {
        instructionsMenu.SetActive(true);
        startMenu.SetActive(false);
    }

    public void GoBack()
    {
        instructionsMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
