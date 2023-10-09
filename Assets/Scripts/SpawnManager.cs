using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject SpawnHole;
    private Transform playerPos;
    private float minRandomArea = 50;
    private float spawnRadious = 50;
    private float maxBorderX = 110;
    private float maxBorderY = 72;
    private int enemyCount;
    private int minAmount;
    private int maxAmount;
    private float spawnWaveTime;
    private int mode;
    private int waveNumber;
    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Instance != null)
        {
            mode = DataManager.Instance.GetMode();
        }
        else
        {
            Debug.LogError("DataManager not found an instance!");
        }
        InitData();
    }

    void InitData()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        waveNumber = GetComponent<GameController>().currentWave;
        // spawn enemy wave after each spawnWaveTime
        switch (mode)
        {
            case 0:
                minAmount = 2;
                maxAmount = 6;
                spawnWaveTime = 8f;
                break;
            case 1:
                minAmount = 3;
                maxAmount = 7;
                spawnWaveTime = 5f;
                break;
            case 2:
                minAmount = 4;
                maxAmount = 8;
                spawnWaveTime = 2f;
                break;
            default:
                Debug.LogError("Invalid mode = " + mode);
                break;
        }
        InvokeRepeating("StartSpawn", 0.5f, spawnWaveTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClearSpawn()
    {
        GameObject[] enemyLists = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyLists)
        {
            if (enemy != null)
            {
                DestroyImmediate(enemy);
            }
        }
    }

    public void AddMoreSpawn()
    {
        // add 0 or 1 on each amount
        minAmount += Random.Range(0, 2);
        maxAmount += Random.Range(0, 2);
    }

    void StartSpawn()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount < minAmount)
        {
            int enemiesToSpawn = Random.Range(minAmount, maxAmount - enemyCount);
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                StartCoroutine(CreateOneEnemy(GenerateSpawnPosition()));
            }
        }
    }

    IEnumerator CreateOneEnemy(Vector3 spawnPos)
    {
        GameObject enemyHole = Instantiate(SpawnHole, spawnPos, Quaternion.identity);
        yield return new WaitForSeconds(1);
        Destroy(enemyHole);
        int randomEnemy = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate(enemyPrefabs[randomEnemy], spawnPos, Quaternion.identity);
        int enemyMaxHP = enemy.GetComponent<EnemyStatus>().maxHP;
        enemy.GetComponent<EnemyStatus>().maxHP = (int)(enemyMaxHP * (1 + 0.1 * waveNumber)); // increase enemy HP 10% each wave
    }

    Vector3 GenerateSpawnPosition()
    {
        while (true)
        {
            Vector3 minRange = minRandomArea * new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
            Vector3 spawnPos = Random.insideUnitCircle * spawnRadious;
            Vector3 randomPos = playerPos.position + minRange + spawnPos;
            if (randomPos.x < maxBorderX && randomPos.x > -maxBorderX && randomPos.y < maxBorderY && randomPos.y > -maxBorderY)
            {
                return randomPos;
            }
        }
    }

}
