using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    // get a reference to the gameUI gameobject canvas in the hierarchy
    public GameObject gameUI;
    // do the same for the puase meanu canvas gameobject 
    public GameObject pauseMenu;
    // and the same for the gameManager script
    public GameManager gameManager;

    /// <summary>
    /// This is the pause game function that the pause button in the hierarchy
    /// will use on press
    /// </summary>
    public void PauseGame()
    {
        // set the isPlaying boolean in the game manager to false
        gameManager.isPlaying = false;
        // set the time scale to 0, this will render all the things that use a
        // change in time to stop (such as animation)
        Time.timeScale = 0f;
        // set the gameUI canvas to not show up on screen by de-activating it
        gameUI.SetActive(false);
        // set the pauseMenu canvas to show up on screen by activating it
        pauseMenu.SetActive(true);
    }

    /// <summary>
    /// this is the un pause game function which un pauses the game when the
    /// button that uses it is pressed
    /// All it does is the literally the opposite of the function above
    /// </summary>
    public void UnPauseGame()
    {
        gameManager.isPlaying = true;
        Time.timeScale = 1;
        gameUI.SetActive(true);
        pauseMenu.SetActive(false);
    }
}
