using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    // a reference to the Game Object of the player that is in the scene
    public GameObject player;
    // a reference to the player controller script
    PlayerController playerController;
    // a reference to the charaetr ocntroller script
    CharacterController characterController;
    // a reference to the game manager script
    GameManager gameManager;

    // the level changer (this is what chager the level: if its a + then the
    // level will go up on touching the door, else if its a - then thr level will go down)
    public int levelChanger;
    // a reference to the door open image which is at the bottom left of the screen 
    public GameObject doorOpenImage;
    // a reference to the door closed image which is at the bottom left of the screen 
    public GameObject doorClosedImage;
    // the required number of keys to this level (it is public so the
    // gamemaneger can access it a decide what this nuber shoudl be based on
    // how many enemies are in the scene)
    public int requiredKeys;
    // the audio source with the door lock sound attached to it
    public AudioSource doorSound;
    // the audio source with the win sound attached to it
    public AudioSource winSound;
    // Start is called before the first frame update
    void Start()
    {
        // set the required number of keys to 1, just as a fail safe (this
        // code doesnt actually come into play unless debugging)
        requiredKeys = 1;
        // make the leve changer positive at the start so that the levels go up
        levelChanger = 1;
        // find the gamamnager script compoenet on the game object in the
        // heirachy with the tag "GameManagerTag"
        gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
        // find the player object on the game object in the
        // heirachy with the tag "PlayerTag"
        player = GameObject.FindGameObjectWithTag("PlayerTag");
        // find the play controller script compoenet on the game object in the
        // heirachy with the tag "PlayerTag"
        playerController = GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<PlayerController>();
        // find the characetr controller component on the game object in the... you get it now
        characterController = GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<CharacterController>();
    }

    void Update()
    {
        // in the number of keys that the player has collected is above or equal
        // to the required number, then disable the door closed image and enable the opened one
        if (playerController.keysCollected >= requiredKeys)
        {
            doorClosedImage.SetActive(false);
            doorOpenImage.SetActive(true);
        }
        // and vice versa
        else
        {
            doorOpenImage.SetActive(false);
            doorClosedImage.SetActive(true);
        }
    }

    // this function only plays when this game objects collider has been hit
    // and stores the information of the other collider in a variable named "other"
    private void OnTriggerEnter(Collider other)
    {
        // if tag of the gameobject hit is "PlayerTag" then call the checkIfPlayerHasEnoughKeys function 
        if (other.gameObject.tag == "PlayerTag")
        {
            checkIfPlayerHasEnoughKeys();
        }
    }

    /// <summary>
    /// this will check if the player has enough keys and if so carries out the
    /// move to next level procedures. And if the level that the player has moved
    /// to is 0, then play the win sound
    /// </summary>
    public void checkIfPlayerHasEnoughKeys()
    {
        // if the number of keys (which is referenced by the keys collected
        // variable in the player controller script) is above or equal the
        // required number of keys then sewitch to the next level by carriying
        // out the follwoing procedures
        if (playerController.keysCollected >= requiredKeys)
        {
            doorSound.Play(); // play the door sound
            Debug.Log("Next Level!"); // print "Next Level!" into the log
            playerController.keysCollected = 0; // reset the number of keys collected in the next level to 0
            gameManager.level += levelChanger; // change the level by 1 in the order of the level changer
            gameManager.isGenerated = false; // tell the gamemanager that
                                             // the new enemies havent been generated yet
            characterController.enabled = false; // disable the charater controller so that - 
            playerController.transform.position = new Vector3(-6, 0, -6); // - the player can move back
                                                                          // to the origin point and - 
            characterController.enabled = true; // - now enable the charcaetr controller again
        }
        // if the level the player has proceeded to is 0. this means the player
        // has made it back through all the level and back and so - 
        if (gameManager.level == 0)
        {
            // - play the win sound!
            winSound.Play();
        }
    }
}
