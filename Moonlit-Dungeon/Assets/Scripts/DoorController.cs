using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    // a reference to the Game Object of the player that is in the scene
    public GameObject player;
    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerTag");
        playerController = GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<PlayerController>();
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
        if (playerController.keysCollected >= 4)
        {
            Debug.Log("Next Level!");
        }
    }
}
