using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this script controls the key/locks bar and all the attributes associated to it
/// </summary>
public class KeyBarController : MonoBehaviour
{
    // get a script variable reference to the playercontroller
    PlayerController playerController;

    // the number of keys gathered
    public int keys;
    // and the number of locks to unlock
    public int locks;
    // a list of empty RawImages
    public RawImage[] keysAndLocks;
    // a texture (its like an image reference for the raw image) of a Key
    public Texture aKey;
    // a texture of a lock
    public Texture aLock;

    // Start is called before the first frame update
    void Start()
    {
        // the player script is equal to the PlayerController Script conponent
        // on the object in the heriachy with the PlayerTag
        playerController = GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<PlayerController>();
        // a back up peice of code (does nothing, its just for debugging)
        locks = 4;
    }

    // Update is called once per frame
    void Update()
    {
        // set the number of keys to the number of keys collected (which is
        // represented by the keysCollected variabel in the gamemanager)
        keys = playerController.keysCollected;
        // if the number of keys ecxeeds the number of locks, rest it back to
        // the total num of locks
        if (keys > locks)
        {
            keys = locks;
        }

        // for every empty raw image
        for (int i = 0; i < keysAndLocks.Length; i++)
        {
            // if this iteration is below the number of keys, set this
            // iterations image to be a key. And if not, a lock
            if (i < keys)
            {
                keysAndLocks[i].texture = aKey;
            }
            else
            {
                keysAndLocks[i].texture = aLock;
            }
            // if this iteration is below the number of locks, then enable this
            // image, if not then disable it 
            if (i < locks)
            {
                keysAndLocks[i].enabled = true;
            }
            else
            {
                keysAndLocks[i].enabled = false;
            }
        }
    }
}
