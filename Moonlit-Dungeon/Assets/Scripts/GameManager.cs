using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public DoorController doorController;
    public KeyBarController keyBarController;

    public GameObject spider;
    public int spiderEnemiesLeft;

    public GameObject wizard;
    public int wizardEnemiesLeft;

    public GameObject guard;
    public int guardEnemiesLeft;

    public GameObject boss;
    public int bossEnemiesLeft;

    // level 1, 2, 3
    public int level;
    // "On the way to Boss" = 1, "Boss" = 2, "Back from Boss" = 3
    public int stage;
    public bool isGenerated = false;

    public bool bossDefeated;

    public GameObject[] pillars;

    public GameObject transitionPanel;
    public Image panelImage;

    // Start is called before the first frame update
    void Start()
    {
        spiderEnemiesLeft = 4;
        wizardEnemiesLeft = 4;
        guardEnemiesLeft = 3;
        bossEnemiesLeft = 1;
        level = 1;
        stage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (level > 3)
        {
            stage = 2;
        }
        if (bossDefeated)
        {
            doorController.levelChanger = -1;
            stage = 3;
        }
        enemyGenrator();
    }

    public void enemyGenrator()
    {
        if (stage == 1)
        {
            if (level == 1 && !isGenerated)
            {
                // make the required number of keys 2
                doorController.requiredKeys = 2;
                keyBarController.locks = spiderEnemiesLeft;
                // Instantiate 4 Spiders.
                for (int y = 0; y < spiderEnemiesLeft; ++y)
                {
                    Instantiate(spider, new Vector3(generateRandomPointOnNavMesh(), 0, generateRandomPointOnNavMesh()), Quaternion.identity);
                }
                isGenerated = true;
            }
            else if (level == 2 && !isGenerated)
            {
                // make the required number of keys 2
                doorController.requiredKeys = 2;
                keyBarController.locks = wizardEnemiesLeft;
                // count and delete all the spiders.
                spiderEnemiesLeft = GameObject.FindGameObjectsWithTag("SpiderTag").Length;
                deleteAllRemainingEnemiesWithThisTag("SpiderTag");
                StartCoroutine(SceneTransition());
                // Instantiate 4 Wizards.
                for (int y = 0; y < wizardEnemiesLeft; ++y)
                {
                    Instantiate(wizard, new Vector3(generateRandomPointOnNavMesh(), 0, generateRandomPointOnNavMesh()), Quaternion.identity);
                }
                isGenerated = true;
            }
            else if (level == 3 && !isGenerated)
            {
                // make the required number of keys 1
                doorController.requiredKeys = 1;
                keyBarController.locks = guardEnemiesLeft;
                // count and delete all the wizards.
                wizardEnemiesLeft = GameObject.FindGameObjectsWithTag("WizardTag").Length;
                deleteAllRemainingEnemiesWithThisTag("WizardTag");
                StartCoroutine(SceneTransition());
                // Instantiate 4 Guards
                for (int y = 0; y < guardEnemiesLeft; ++y)
                {
                    Instantiate(guard, new Vector3(generateRandomPointOnNavMesh(), 0, generateRandomPointOnNavMesh()), Quaternion.identity);
                }
                isGenerated = true;
            }
        }
        else if (stage == 2 && !isGenerated)
        {
            // make the required number of keys 1
            doorController.requiredKeys = 1;
            keyBarController.locks = bossEnemiesLeft;
            // count all the guards left and delete them
            guardEnemiesLeft = GameObject.FindGameObjectsWithTag("GuardTag").Length;
            deleteAllRemainingEnemiesWithThisTag("GuardTag");
            StartCoroutine(SceneTransition());
            // Remove the pillars
            for (int i = 0; i < pillars.Length; i++)
            {
                pillars[i].SetActive(false);
            }
            // Instantiate Boss
            for (int y = 0; y < bossEnemiesLeft; ++y)
            {
                Instantiate(boss, new Vector3(-5, 0, -5), Quaternion.identity);
            }
            isGenerated = true;

        }
        else if (stage == 3)
        {
            if (level == 3 && !isGenerated)
            {
                // make the required number of keys equal the number of guards left
                doorController.requiredKeys = guardEnemiesLeft;
                keyBarController.locks = guardEnemiesLeft;
                // Instantiate remaining Stone Giants
                StartCoroutine(SceneTransition());
                // Instantiate 4 Guards
                for (int y = 0; y < guardEnemiesLeft; ++y)
                {
                    Instantiate(guard, new Vector3(generateRandomPointOnNavMesh(), 0, generateRandomPointOnNavMesh()), Quaternion.identity);
                }
                isGenerated = true;
            }
            else if (level == 2 && !isGenerated)
            {
                // make the required number of keys equal the number of wizards left
                doorController.requiredKeys = wizardEnemiesLeft;
                keyBarController.locks = wizardEnemiesLeft;
                // Instantiate remaining Wizards.
                StartCoroutine(SceneTransition());
                // Instantiate 4 Wizards.
                for (int y = 0; y < wizardEnemiesLeft; ++y)
                {
                    Instantiate(wizard, new Vector3(generateRandomPointOnNavMesh(), 0, generateRandomPointOnNavMesh()), Quaternion.identity);
                }
                isGenerated = true;
            }
            else if (level == 1 && !isGenerated)
            {
                // make the required number of keys equal the number of spiders left
                doorController.requiredKeys = spiderEnemiesLeft;
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
            else if (level == 0)
            {
                //win statment
            }
        }
    }

    public float generateRandomPointOnNavMesh()
    {
        float randomPoint = Random.Range(-4, 4);
        return randomPoint;
    }

    public void deleteAllRemainingEnemiesWithThisTag(string enemyTagName)
    {
        GameObject[] spiderLeftInScene = GameObject.FindGameObjectsWithTag(enemyTagName);
        foreach (GameObject theObject in spiderLeftInScene)
        {
            Destroy(theObject);
        }
    }

    public IEnumerator SceneTransition()
    {
        if (true)
        {
            // incrase the alpha
            panelImage = transitionPanel.GetComponent<Image>();
            var tempColor = panelImage.color;
            tempColor.a = 1;
            panelImage.color = tempColor;
        }
        
        for (int i = 0; i < 20; i++)
        {
            // incrase the alpha
            panelImage = transitionPanel.GetComponent<Image>();
            var tempColor = panelImage.color;
            tempColor.a -= 0.05f;
            panelImage.color = tempColor;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
