using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z <= -7 || transform.position.z >= 7)
        {
            Destroy(gameObject);
        }
        if (transform.position.x <= -7 || transform.position.x >= 7)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerTag")
        {
            Destroy(gameObject);
        }
    }
}
