using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    public int phaseCount;
    public int currentPhase;
    private float phaseHealth;
    private float startingHealth;

    public BulletSpawner[] phase1Spawners;
    public BulletSpawner[] phase2Spawners;
    public BulletSpawner[] phase3Spawners;
    public BulletSpawner[] phase4Spawners;

    public Image healthBar;


    void Start()
    {
        foreach (BulletSpawner spawner in phase1Spawners)
        {
            spawner.enabled = true;
        }
        foreach (BulletSpawner spawner in phase2Spawners)
        {
            spawner.enabled = false;
        }
        foreach (BulletSpawner spawner in phase3Spawners)
        {
            spawner.enabled = false;
        }
        foreach (BulletSpawner spawner in phase4Spawners)
        {
            spawner.enabled = false;
        }

        phaseHealth = GetComponent<EnemyScript>().health / phaseCount;
        currentPhase = 1;
        startingHealth = GetComponent<EnemyScript>().health;
    }


    void Update()
    {
        if (GetComponent<EnemyScript>().health <= startingHealth - (phaseHealth * currentPhase))
        {
            UnityEngine.Debug.Log("y");
            currentPhase++;
            PhasePush();
        }

        healthBar.fillAmount = GetComponent<EnemyScript>().health / 100f;
        //UnityEngine.Debug.Log(GetComponent<EnemyScript>().health);
        //UnityEngine.Debug.Log(GetComponent<EnemyScript>().health - (phaseHealth * currentPhase));


    }

    void PhasePush()
    {
        if (currentPhase == 1)
        {
            foreach (BulletSpawner spawner in phase1Spawners)
            {
                spawner.enabled = true;
            }
        }
        if (currentPhase == 2)
        {
            foreach (BulletSpawner spawner in phase1Spawners)
            {
                spawner.enabled = false;
            }
            foreach (BulletSpawner spawner in phase2Spawners)
            {
                spawner.enabled = true;
            }
        }
        if (currentPhase == 3)
        {
            foreach (BulletSpawner spawner in phase2Spawners)
            {
                spawner.enabled = false;
            }
            foreach (BulletSpawner spawner in phase3Spawners)
            {
                spawner.enabled = true;
            }
        }
        if (currentPhase == 4)
        {
            foreach (BulletSpawner spawner in phase3Spawners)
            {
                spawner.enabled = false;
            }
            foreach (BulletSpawner spawner in phase4Spawners)
            {
                spawner.enabled = true;
            }
        }
    }
    void OnDestroy()
    {
        foreach (BulletSpawner spawner in phase1Spawners)
        {
            spawner.enabled = false;
        }
        foreach (BulletSpawner spawner in phase2Spawners)
        {
            spawner.enabled = false;
        }
        foreach (BulletSpawner spawner in phase3Spawners)
        {
            spawner.enabled = false;
        }
        foreach (BulletSpawner spawner in phase4Spawners)
        {
            spawner.enabled = false;
        }
    }
}