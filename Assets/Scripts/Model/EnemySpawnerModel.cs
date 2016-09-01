using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class EnemySpawnerModel : GameElement
{
    public int spawnCoolDown;
    private WaitForSeconds waitTillSpawnNextEnemy;

    private Coroutine spawnEnemyCoroutine;

    public GameObject SpawnLimit1, SpawnLimit2;
    private const float groundLevel = 0.05f;

    

    void Awake()
    {
         waitTillSpawnNextEnemy = new WaitForSeconds(spawnCoolDown);
    }

    void Start()
    {

        this.RegisterListener(EventID.OnGameStart, (sender, param) => StartSpawningEnemies());
        this.RegisterListener(EventID.OnPlayerDie, (sender, param) => StopSpawningEnemies());
    }

    public void StartSpawningEnemies()
    {
        if (GameManager.IsInStory())
        {
            Debug.Log("story mode spawning");
            spawnEnemyCoroutine = StartCoroutine(SpawnEnemy());
        }
        else
            spawnEnemyCoroutine = StartCoroutine(SpawnEnemyInifinite());            
    }

    public void StopSpawningEnemies()
    {
        StopCoroutine(spawnEnemyCoroutine);
    }

    IEnumerator SpawnEnemy()
    {
        while (GameManager.isPlaying() && GameManager.model.GetEnemyCount() < GameManager.model.GetEnemyCurrentCap())
        {
            yield return waitTillSpawnNextEnemy;

            Vector3 spawnLocation = CalculateSpawnLocation();

            this.PostEvent(EventID.OnSpawnEnemy, spawnLocation);            
        }
    }

    IEnumerator SpawnEnemyInifinite()
    {        
        while (GameManager.isPlaying())
        {
            yield return waitTillSpawnNextEnemy;

            Vector3 spawnLocation = CalculateSpawnLocation();

            this.PostEvent(EventID.OnSpawnEnemy, spawnLocation);            
        }        
    }

    private Vector3 CalculateSpawnLocation()
    {
        float x = Random.Range(SpawnLimit1.transform.position.x, SpawnLimit2.transform.position.x);
        float z = Random.Range(SpawnLimit1.transform.position.z, SpawnLimit2.transform.position.z);

        return new Vector3(x, groundLevel, z);
    }
}
