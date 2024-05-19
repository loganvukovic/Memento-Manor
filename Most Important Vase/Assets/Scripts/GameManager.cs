using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerMovement playerScript;
    public GameObject gameOverScreen;
    public bool gameOver = false;
    public EnemyScript[] bosses;
    public BossScript[] bossScripts;
    public GameObject risingFloor;
    public GameObject fakeWizard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        if (Time.timeScale != 0 && !gameOver)
        {
            gameOver = true;
            Time.timeScale = 0f;
            gameOverScreen.SetActive(true);
        }
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        playerScript.currentHealth = 100;
        UnityEngine.Debug.Log(Time.timeScale);
        gameOverScreen.SetActive(false);
        gameOver = false;
        playerScript.Respawn();
        foreach (EnemyScript boss in bosses)
        {
            if (boss != null)
            {
                boss.health = 100;
            }
        }
        foreach (BossScript boss in bossScripts)
        {
            if (boss != null)
            {
                boss.currentPhase = 1;
                if(boss.activeBoss)
                {
                    boss.PhasePush();
                }
            }
        }
        risingFloor.transform.position = new Vector3(574.71f, -9.1f, -0.2674881f);
        risingFloor.GetComponent<RisingDeath>().timeSinceSpawn = 0;
        fakeWizard.transform.position = new Vector3(590.71f, 15.24f, -0.1353276f);
        fakeWizard.GetComponent<TeleportAndShoot>().StopAllCoroutines();
        fakeWizard.GetComponent<TeleportAndShoot>().isTeleporting = false;
    }
}