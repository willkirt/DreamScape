using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPCManager : MonoBehaviour
{
    [Header("Lists of the various NPC's in the game and spawn locations.")]
    [SerializeField] Enemy[] enemyPrefab;
    [SerializeField] Friendly[] friendlyPrefab;
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    [SerializeField] List<GameObject> friendlies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy(string name, int spawnPoint)
    {
        Enemy e = Array.Find(enemyPrefab, newEnemy => newEnemy.enemyName == name);

        if (e == null)
        {
            Debug.Log("Enemy prefab " + name + " not found.");
        }
        else
        {
            try
            {
                enemies.Add(Instantiate(e.prefab, spawnPoints[spawnPoint].transform.position, spawnPoints[spawnPoint].transform.rotation));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public void UpdateTarget(GameObject newTarget)
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<EnemyAIScript1>().AggroState == EnemyAIScript1.EnemyStates.Chase)
            {
                enemy.GetComponent<EnemyAIScript1>().StartChasing(newTarget);
            }
        }
    }

    public void StopChasing()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<EnemyAIScript1>().AggroState == EnemyAIScript1.EnemyStates.Chase)
            {
                enemy.GetComponent<EnemyAIScript1>().StartIdle();
            }
        }
    }
}
