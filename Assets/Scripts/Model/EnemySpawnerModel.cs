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

    int enemyCount = 0;

    bool[] fast;

    void Awake()
    {
        waitTillSpawnNextEnemy = new WaitForSeconds(spawnCoolDown);
    }

    void Start()
    {
        fast = new bool[70];

        this.RegisterListener(EventID.OnGameStart, (sender, param) => StartCoroutine(StartSpawningEnemies()));
        this.RegisterListener(EventID.OnPlayerDie, (sender, param) => StopSpawningEnemies());
    }

    IEnumerator StartSpawningEnemies()
    {
        WaitForSeconds wait = new WaitForSeconds(0);

        while (!GameManager.model.Set())
        {
            yield return wait;
        }

        AssignFastEnemies(); // might be called later than cap set which is a problem

        if (GameManager.IsInStory())
        {
            Debug.Log("story mode spawning");
            spawnEnemyCoroutine = StartCoroutine(SpawnEnemy());
        }
        else
            spawnEnemyCoroutine = StartCoroutine(SpawnEnemyInifinite());
    }

    private void AssignFastEnemies()
    {
        Debug.Log("assigning");

        for (int i = 0; i < GameManager.model.GetEnemyCurrentCap(); i++)
        {
            float roll = Random.value;

            fast[i] = roll < GameManager.model.GetCurrentFast();
        }
    }

    public bool IsFast()
    {
        return fast[enemyCount];
    }

    public void StopSpawningEnemies()
    {
        enemyCount = 0;

        StopCoroutine(spawnEnemyCoroutine);
    }

    IEnumerator SpawnEnemy()
    {
        while (GameManager.isPlaying() && enemyCount < GameManager.model.GetEnemyCurrentCap())
        {
            yield return waitTillSpawnNextEnemy;

            Vector3 spawnLocation = CalculateSpawnLocation();

            this.PostEvent(EventID.OnSpawnEnemy, spawnLocation);
            enemyCount++;            
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
