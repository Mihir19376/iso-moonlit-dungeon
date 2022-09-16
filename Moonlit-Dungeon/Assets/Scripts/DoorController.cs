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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerTag")
        {
            checkIfPlayerHasEnoughKeys();
        }

        Debug.Log(collision.gameObject.tag);
    }

    public void checkIfPlayerHasEnoughKeys()
    {
        if (playerController.keysCollected >= 4)
        {
            player.transform.position = new Vector3(0, 0, 0);
            Debug.Log("Next Level!");
        }
    }
}
