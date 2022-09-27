using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject spider;
    public int spiderEnemiesLeft;

    public GameObject wizard;
    public int wizardEnemiesLeft;

    // level 1, 2, 3, and 4
    public int level;
    // "On the way to Boss" = 1, "Boss" = 2, "Back from Boss" = 3
    public int stage;
    public bool isGenerated = false;

    // Start is called before the first frame update
    void Start()
    {
        spiderEnemiesLeft = 4;
        level = 1;
        stage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (level > 4)
        {
            stage = 2;
        }
        enemyGenrator();
    }

    public void enemyGenrator()
    {
        if (stage == 1)
        {
            if (level == 1 && !isGenerated)
            {
                // Instantiate 4 Spiders.
                for (int y = 0; y < spiderEnemiesLeft; ++y)
                {
                    Instantiate(spider, new Vector3(0, 0, 0), Quaternion.identity);
                }
                isGenerated = true;
            }
            else if (level == 2 && !isGenerated)
            {
                // Instantiate 4 Wizards.
                for (int y = 0; y < wizardEnemiesLeft; ++y)
                {
                    Instantiate(wizard, new Vector3(0, 0, 0), Quaternion.identity);
                }
                isGenerated = true;
            }
            else if (level == 3)
            {
                // Instantiate 4 ...
            }
            else if (level == 4)
            {
                // Instantiate 4 ...
            }
        }
        else if (stage == 2)
        {
            // Instantiate Boss
        }
        else if (stage == 3)
        {
            if (level == 1)
            {
                // Instantiate remaining Spiders.
            }
            else if (level == 2)
            {
                // Instantiate remaining Wizards.
            }
            else if (level == 3)
            {
                // Instantiate remaining ...
            }
            else if (level == 4)
            {
                // Instantiate remaining ...
            }
        }
    }

    public void generateRandomPointOnNavMesh()
    {
        // generateRandomPointOnNavMesh
    }
}
