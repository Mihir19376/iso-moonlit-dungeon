using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // a reference to the door controller script
    public DoorController doorController;
    // a reference to the keyBarController script so that we can update the key UI from here
    public KeyBarController keyBarController;
    // reference to the spider prefab so that this script can instantiate them
    public GameObject spider;
    // number of spider enemies left. This will be the number of spider enemies
    // spawned whenever the script needs to spawn them
    public int spiderEnemiesLeft;

    // the same deal as above is for the next 6 variables
    public GameObject wizard;
    public int wizardEnemiesLeft;

    public GameObject guard;
    public int guardEnemiesLeft;

    public GameObject boss;
    public int bossEnemiesLeft;

    // reference to what level it is (i.e. 0, 1, 2, 3, 4)
    public int level;
    // what stage it is "On the way to Boss" = 1, "Boss" = 2, "Back from Boss" = 3
    public int stage;
    // boolean telling the script if the required enemies has been spawned
    public bool isGenerated = false;
    // a boolean telling us if the boss has been defeated so we can make the
    // apporpriate changes to the game (i.e. the treause chest icon appearing,
    // changing the stage)
    public bool bossDefeated;
    // a reference to all the pillars in the scene. Just so I can deactivate and
    // activate them during and after the boss level. And yes, deactivating
    // them does re-calc the nav mesh
    public GameObject[] pillars;
    // the black panel that pops in and fades out when the level changes
    public GameObject transitionPanel;
    // and a reference to that panels image
    public Image panelImage;
    // a reference to the chest image gameobject in the game UI. so that this
    // script can make it appear when the player has attained it
    public GameObject chest;
    // a reference to the win screen canvas which appears when the player wins
    public GameObject winScreen;
    // a public boolean that states if the gaem is being played. If this is set
    // to false, then one of the menus must be up. this is cheked before any
    // input in the game so the player cant attack while in a menu
    public bool isPlaying;

    // the range in which the enemies are spawned on the floor
    private float spawningRange = 4;

    // get reference to the different enemy icons to use to let the player know
    // what level they are playing
    public GameObject spiderIcon;
    public GameObject wizardIcon;
    public GameObject guardIcon;
    public GameObject bossIcon;

    // Start is called before the first frame update
    void Start()
    {
        // set the number of spider enemies to be 4. So intially the player will face 4
        spiderEnemiesLeft = 4;
        // set the number of wizard enemies to be 4. So intially the player will face 4
        wizardEnemiesLeft = 4;
        // set the number of guard enemies to be 3. So intially the player will face 4
        guardEnemiesLeft = 3;
        // obviaously theres only one boss
        bossEnemiesLeft = 1;
        // the intial level is 1 - 
        level = 1;
        // - and so is the stage
        stage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // if the level is 3 (which means they have gotten past the guard
        // enemies and are on to the boss. so the stage is 2 now which is the boss stage)
        if (level > 3)
        {
            stage = 2; // set stage to be 2 (boss stage)
        }
        // if the boss is defeated, then the chest image is set to active
        // because defeating the boss means to get the chest
        if (bossDefeated)
        {
            chest.SetActive(true);
            doorController.levelChanger = -1; // also make the level changer
                                              // negative so that the player goes down elevls from now until they get to 0 meaning theyve won
            stage = 3; // set the stage to be 3, which tell this script its a return journey from the boss now
        }
        enemyGenrator(); // generate the enemies
    }

    public void enemyGenrator()
    {
        // if the stage is 1 (this means its the journey leading up to the boss) then:
        if (stage == 1)
        {
            // if the level is 1, and isGenerated = false (basically this stands for have the enemeis been not genrated yet?)
            if (level == 1 && !isGenerated)
            {
                // make the spider Icon appear to let the user know hat room they are in
                SpiderIconAppear();
                // make the required number of keys half the number of spider enemies in the room
                doorController.requiredKeys = (int)(spiderEnemiesLeft/2);
                // make the number of possible locks to get equal to the number of enemies in the arena
                keyBarController.locks = spiderEnemiesLeft;
                // Instantiate all the Spiders.
                for (int y = 0; y < spiderEnemiesLeft; ++y)
                {
                    // instantaite them at a random position on the arena
                    Instantiate(spider, new Vector3(generateRandomPointOnNavMesh(), 0, generateRandomPointOnNavMesh()), Quaternion.identity);
                }
                // set is egenrated to true so that these enemies wont generate
                // again until it is set to false (which is again set to false
                // once the player advances to the next level)
                isGenerated = true;
            }
            else if (level == 2 && !isGenerated)
            {
                // make the wizard Icon appear to let the user know hat room they are in
                WizardIconAppear();
                // make the required number of keys half the number of wizard enemies in the room
                doorController.requiredKeys = (int)(wizardEnemiesLeft/2);
                // make the number of possible locks to get equal to the number of enemies in the arena
                keyBarController.locks = wizardEnemiesLeft;
                // count and delete all the remaining spiders from the last level.
                spiderEnemiesLeft = GameObject.FindGameObjectsWithTag("SpiderTag").Length;
                deleteAllRemainingEnemiesWithThisTag("SpiderTag");
                // initiate the scene transition
                StartCoroutine(SceneTransition());
                // Instantiate all the Wizards.
                for (int y = 0; y < wizardEnemiesLeft; ++y)
                {
                    // spawn them at a random position on the arena
                    Instantiate(wizard, new Vector3(generateRandomPointOnNavMesh(), 0, generateRandomPointOnNavMesh()), Quaternion.identity);
                }
                // set is generated to true so that these enemies wont generate
                // again until it is set to false (which is again set to false
                // once the player advances to the next level)
                isGenerated = true;
            }
            else if (level == 3 && !isGenerated)
            {
                // make the guard Icon appear to let the user know hat room they are in
                GuardIconAppear();
                // make the required number of keys a third of the amount of guard enemies in the room
                doorController.requiredKeys = (int)(guardEnemiesLeft/3);
                // make the number of possible locks to get equal to the number of enemies in the arena
                keyBarController.locks = guardEnemiesLeft;
                // count and delete all the wizards.
                wizardEnemiesLeft = GameObject.FindGameObjectsWithTag("WizardTag").Length;
                deleteAllRemainingEnemiesWithThisTag("WizardTag");
                // initiate the scene transition
                StartCoroutine(SceneTransition());
                // Instantiate all the Guards
                for (int y = 0; y < guardEnemiesLeft; ++y)
                {
                    // spawn them at a random position on the arena
                    Instantiate(guard, new Vector3(generateRandomPointOnNavMesh(), 0, generateRandomPointOnNavMesh()), Quaternion.identity);
                }
                // set is generated to true so that these enemies wont generate
                // again until it is set to false (which is again set to false
                // once the player advances to the next level)
                isGenerated = true;
            }
        }
        // if the stage is 2 (this means its the boss level) then:
        else if (stage == 2 && !isGenerated)
        {
            // make the boss Icon appear to let the user know hat room they are in
            BossIconAppear();
            // make the required number of keys 1 (the number of bosses)
            doorController.requiredKeys = bossEnemiesLeft;
            // make the number of possible locks to get equal to the number of
            // enemies in the arena
            keyBarController.locks = bossEnemiesLeft;
            // count all the guards left and delete them
            guardEnemiesLeft = GameObject.FindGameObjectsWithTag("GuardTag").Length;
            deleteAllRemainingEnemiesWithThisTag("GuardTag");
            // initiate the scene transition
            StartCoroutine(SceneTransition());
            // Remove the pillars by iterating over this loop for how many
            // pillars there are in the pillar game object list
            for (int i = 0; i < pillars.Length; i++)
            {
                pillars[i].SetActive(false); // disable this pillar in the heriachy
            }
            // Instantiate Boss(es) there could be more than 1 which is why the
            // variables are named like this and there is a loop. there is only
            // one now but in the future that cound change and having this set
            // up now makes it alot easier to make that change
            for (int y = 0; y < bossEnemiesLeft; ++y)
            {
                // spawn the boss at the middle
                Instantiate(boss, new Vector3(0, 0, 0), Quaternion.identity);
            }
            // set is generated to true so that these enemies wont generate
            // again until it is set to false (which is again set to false
            // once the player advances to the next level)
            isGenerated = true;

        }
        // if the stage is 3 (this means its the return journey from the boss' defeat) then:
        else if (stage == 3)
        {
            // if the level is 3, and not genrated yet the:
            if (level == 3 && !isGenerated)
            {
                // make the guard Icon appear to let the user know hat room they are in
                GuardIconAppear();
                // bring back the pillars just like how I removed them
                for (int i = 0; i < pillars.Length; i++)
                {
                    pillars[i].SetActive(true);
                }
                // make the required number of keys equal the number of guards left
                doorController.requiredKeys = guardEnemiesLeft;
                // make the number of avaibale locks equal the  number of enemeis left
                keyBarController.locks = guardEnemiesLeft;
                // initiate the scene transition
                StartCoroutine(SceneTransition());
                /// Instantiate remaining Stone Giants at a random spot on the arena
                for (int y = 0; y < guardEnemiesLeft; ++y)
                {
                    Instantiate(guard, new Vector3(generateRandomPointOnNavMesh(), 0, generateRandomPointOnNavMesh()), Quaternion.identity);
                }
                // set the isGenrated to true
                isGenerated = true;
            }
            // if the level is 2, and not genrated yet then:
            else if (level == 2 && !isGenerated)
            {
                // make the wizard Icon appear to let the user know hat room they are in
                WizardIconAppear();
                // make the required number of keys equal the number of wizards left
                doorController.requiredKeys = wizardEnemiesLeft;
                // make the number of avaibale locks equal the  number of enemeis left
                keyBarController.locks = wizardEnemiesLeft;
                // initaite the scene transition
                StartCoroutine(SceneTransition());
                // Instantiate remaining Wizards.
                for (int y = 0; y < wizardEnemiesLeft; ++y)
                {
                    Instantiate(wizard, new Vector3(generateRandomPointOnNavMesh(), 0, generateRandomPointOnNavMesh()), Quaternion.identity);
                }
                isGenerated = true;
            }
            // if the level is 1, and not genrated yet then:
            else if (level == 1 && !isGenerated)
            {
                // make the spider Icon appear to let the user know hat room they are in
                SpiderIconAppear();
                // make the required number of keys equal the number of spiders left
                doorController.requiredKeys = spiderEnemiesLeft;
                // make the number of avaibale locks equal the  number of enemeis left
                keyBarController.locks = spiderEnemiesLeft;
                // Instantiate remaining Spiders.
                StartCoroutine(SceneTransition());
                // Instantiate 4 Spiders.
                for (int y = 0; y < spiderEnemiesLeft; ++y)
                {
                    Instantiate(spider, new Vector3(generateRandomPointOnNavMesh(), 0, generateRandomPointOnNavMesh()), Quaternion.identity);
                }
                isGenerated = true;
            }
            // if the level is 0, then this means the player has made through
            // all the levels and back and has won the game
            else if (level == 0)
            {
                // make rhe win screen appear
                winScreen.SetActive(true);
            }
        }
    }

    /// <summary>
    /// create a float variable named randomPoint to use as a x and z spawn point
    /// </summary>
    /// <returns>returns random point</returns>
    public float generateRandomPointOnNavMesh()
    {
        // generate a number between -spawningRange and +spawningRange
        float randomPoint = Random.Range(-spawningRange, spawningRange);
        // return this number
        return randomPoint;
    }

    /// <summary>
    /// delete all the remaining enemy with "this" tag
    /// </summary>
    /// <param name="enemyTagName">the name of the enemy tag to eradicate</param>
    public void deleteAllRemainingEnemiesWithThisTag(string enemyTagName)
    {
        // get a list of all the gameobjects (enemies) with the enetered enemy tag
        GameObject[] spiderLeftInScene = GameObject.FindGameObjectsWithTag(enemyTagName);
        // for each enemy in the game object list, destroy it
        foreach (GameObject theObject in spiderLeftInScene)
        {
            Destroy(theObject);
        }
    }

    /// <summary>
    /// transition to the next scene by making  the screen black and fading out of it
    /// </summary>
    /// <returns>doesnt return anything</returns>
    public IEnumerator SceneTransition()
    {
        // yes, I know this look odd. Buts its here just so that I don't have
        // to create an entriely new fucntion for this obscure peice of code.
        if (true)
        {
            // get the Image compoent on the transitionPanel object and make that equal to the panelImage
            panelImage = transitionPanel.GetComponent<Image>();
            // set the images colour to temperary colour. And its spelt "colour". I dont uderstand why the code speaks in american english
            var tempColor = panelImage.color;
            // set this colours slpha value (trasparecny) 1 (full) to fill the entrie screen black
            tempColor.a = 1;
            // set this new, opaque, colour back to the panel images colour
            panelImage.color = tempColor;
        }
        // fade out. Every iteration, the alpha value gets a little smaller hence making it more and more transparent
        for (int i = 0; i < 20; i++)
        {
            // get the Image compoent on the transitionPanel object and make that equal to the panelImage
            panelImage = transitionPanel.GetComponent<Image>();
            // set the images colour to temp colour.
            var tempColor = panelImage.color;
            // decrease this alpha value by 0.05 hence making it a bit more translucent
            tempColor.a -= 0.05f;
            // // set this new, slighty less opaque, colour back to the panel images colour
            panelImage.color = tempColor;
            // wait for .05 seconds till the next iteration
            yield return new WaitForSeconds(0.05f);
        }
    }

    void SpiderIconAppear()
    {
        spiderIcon.SetActive(true);
        guardIcon.SetActive(false);
        wizardIcon.SetActive(false);
        bossIcon.SetActive(false);
    }

    void WizardIconAppear()
    {
        spiderIcon.SetActive(false);
        guardIcon.SetActive(false);
        wizardIcon.SetActive(true);
        bossIcon.SetActive(false);
    }

    void GuardIconAppear()
    {
        spiderIcon.SetActive(false);
        guardIcon.SetActive(true);
        wizardIcon.SetActive(false);
        bossIcon.SetActive(false);
    }

    void BossIconAppear()
    {
        spiderIcon.SetActive(false);
        guardIcon.SetActive(false);
        wizardIcon.SetActive(false);
        bossIcon.SetActive(true);
    }
}
