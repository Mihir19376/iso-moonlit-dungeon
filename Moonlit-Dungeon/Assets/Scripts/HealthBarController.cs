using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    // get a script variable reference to the playercontroller
    PlayerController playerController;
    // get a public refrence (so there is a slot in the inspector to add it) to
    // the fill colour image
    public Image fillImage;
    // get a refrerence to the slider object (the health bar)
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        // the player script is equal to the PlayerController Script conponent
        // on the object in the heriachy with the PlayerTag
        playerController = GameObject.FindGameObjectWithTag("PlayerTag").GetComponent<PlayerController>();
        // the slider is equal to the Slider Componenet on the object this
        // script is on
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        // if the current value of the slider is less than or equal to the min
        // value of the slider object, then keep the slider unabled in the heirachy
        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }
        // but if the sliders values is above the min value of the slider object
        // and isn't already disabled, then keep it enabled
        if(slider.value > slider.minValue && !fillImage.enabled)
        {
            fillImage.enabled = true;
        }

        /* create a float variable called fillvalue and set it equal to the
           player health variabel on the playerocntroller script divided by the
           max health float on the player controller */
        float fillValue = playerController.playerHealth/playerController.playerMaxHealth;
        // print out this float value in the log
        Debug.Log(fillValue);
        // and make the slider value equal to that
        slider.value = fillValue;
    }
}