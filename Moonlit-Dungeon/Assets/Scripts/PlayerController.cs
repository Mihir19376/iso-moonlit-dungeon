using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator playerAnimator;
    private float turnSpeed = 100f;
    private float moveSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            
            playerAnimator.SetBool("IsRunning", true);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            playerAnimator.SetBool("IsRunning", true);
            transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
        }

        else
        {
            playerAnimator.SetBool("IsRunning", false);
        }

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
        }

    }
}
