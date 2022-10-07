using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    // a reference to the Game Object of the player that is in the scene
    public GameObject player;
    PlayerController playerController;
    CharacterController characterController;
    GameManager gameManager;

    public int levelChanger;

    public GameObject doorOpenImage;
    public GameObject doorClosedImage;

    public int requiredKeys;
    // Start is called before the first frame update
    void Start()
    {
        requiredKeys = 1;
        levelChanger = 1;
        gameManager = GameObject.FindGameObjectWithTag("GameManagerTag").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("PlayerTag");
        playerController = GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<PlayerController>();
        characterController = GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<CharacterController>();
    }

    void Update()
    {
        if (playerController.keysCollected >= requiredKeys)
        {
            doorClosedImage.SetActive(false);
            doorOpenImage.SetActive(true);
        }
        else
        {
            doorOpenImage.SetActive(false);
            doorClosedImage.SetActive(true);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerTag")
        {
            checkIfPlayerHasEnoughKeys();
        }
    }


    public void checkIfPlayerHasEnoughKeys()
    {
        if (playerController.keysCollected >= requiredKeys)
        {
            Debug.Log("Next Level!");
            playerController.keysCollected = 0;
            gameManager.level += levelChanger;
            gameManager.isGenerated = false;
            characterController.enabled = false;
            playerController.transform.position = new Vector3(-6, 0, -6);
            characterController.enabled = true;
        }
    }
}
