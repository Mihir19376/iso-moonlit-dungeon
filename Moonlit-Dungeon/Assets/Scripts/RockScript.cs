using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    // this is the furthest distance that the rock can go before getting destroyed
    private int destoryPosition = 7;
    // a grey explosion prefab to be instantiate when this rock hits the player
    public GameObject greyExplosionFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if the z position is less than the negative destroyPosition or
        // larger than the position destroy position then destroy this object
        if (transform.position.z <= -destoryPosition || transform.position.z >= destoryPosition)
        {
            Destroy(gameObject);
        }
        // if the x position is less than the negative destroyPosition or
        // larger than the position destroy position then destroy this object
        if (transform.position.x <= -destoryPosition || transform.position.x >= destoryPosition)
        {
            Destroy(gameObject);
        }
    }

    /* this function will only play when this objects boxcollider gets triggered
       (when it touches something) and stores the information of the collider it
       touched in a vaariable called other */
    private void OnTriggerEnter(Collider other)
    {
        // if the variables' gameObjects tag is "PlayerTag" then destroy this
        // object
        if (other.gameObject.tag == "PlayerTag")
        {
            // make a grey explosion on the area of hit
            Instantiate(greyExplosionFX, transform.position, Quaternion.identity);
            // then destroy itself 
            Destroy(gameObject);
        }
    }
}
