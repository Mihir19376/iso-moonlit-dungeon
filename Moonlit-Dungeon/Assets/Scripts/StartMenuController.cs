using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// this script controls the start menu and all the attributes associated to it (such as button, etc.)
/// </summary>
public class StartMenuController : MonoBehaviour
{
    // a referenc to the startMenu canvas game object (this reference can be
    // used to turn it off or on)
    public GameObject startMenu;
    // same as above just for the game UI canvas
    public GameObject gameUI;
    // same as above just for the instructions menu canvas
    public GameObject instructionsMenu;
    // a public reference to the gamanager script (this will create a spot in
    // the inspector to drag the script into)
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // set the isPLaying bool to false at the start of the game.
        gameManager.isPlaying = false;
        // set the time scale to 0 so that most updates dont happen
        // Just so that no input is being taken during the menu
        Time.timeScale = 0f;
    }

    /// <summary>
    /// this is the play game function which is used when the play game button is pressed
    /// it basicallly activates the game
    /// </summary>
    public void PlayGame()
    {
        // set the isPlayaing bool in the game object to true
        gameManager.isPlaying = true;
        // set the timescale to 1 to allow far all usual updates
        Time.timeScale = 1f;
        // switch off (de-activate) the start menu canvas
        startMenu.SetActive(false);
        // and switch on (activate) the gameplay UI
        gameUI.SetActive(true);
    }

    /// <summary>
    /// Quits the game when the quit button is pressed
    /// </summary>
    public void QuitGame()
    {
        // stops and exits the game (app)
        // but this wont work in the inspection. only once the game is built
        Application.Quit();
    }

    /// <summary>
    /// this function is called when the instructions button is pressed.
    /// It will open up the instructions panel
    /// </summary>
    public void GoToInsructions()
    {
        // activate the instructions menu canvas
        instructionsMenu.SetActive(true);
        // and switch off the start menu canvas
        startMenu.SetActive(false);
    }

    /// <summary>
    /// this reverses that of the function right above (GoToInstructions)
    /// </summary>
    public void GoBack()
    {
        // switches off the instructions menu
        instructionsMenu.SetActive(false);
        // switches on the start menu
        startMenu.SetActive(true);
    }

    /// <summary>
    /// this function (when the give-up button is pressed) will start the entire
    /// game over again 
    /// </summary>
    public void RestartGame()
    {
        // this is reload the current scene. Thus restarting the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
