using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBarController : MonoBehaviour
{
    PlayerController playerController;

    public int keys;
    public int locks;
    public RawImage[] keysAndLocks;
    public Texture aKey;
    public Texture aLock;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<PlayerController>();
        locks = 4;
    }

    // Update is called once per frame
    void Update()
    {
        keys = playerController.keysCollected;
        if (keys > locks)
        {
            keys = locks;
        }

        for (int i = 0; i < keysAndLocks.Length; i++)
        {
            if (i < keys)
            {
                keysAndLocks[i].texture = aKey;
            }
            else
            {
                keysAndLocks[i].texture = aLock;
            }

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
