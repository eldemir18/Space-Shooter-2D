using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ListOfWaveConfigSO
{
    public List<WaveConfigSO> waveConfigSOList;

    public WaveConfigSO this[int index]
    {
        get{ return waveConfigSOList[index];}
        set{ waveConfigSOList[index] = value;}
    }

    public int Count()
    {
        return waveConfigSOList.Count;
    }
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Waves")]
    [SerializeField] List<ListOfWaveConfigSO> waveConfigs;

    [Header("Bosses")]
    [SerializeField] ListOfWaveConfigSO bosses;
    [SerializeField] int[] bossTimers;

    
    [Header("Times")]
    [SerializeField] float timeBetweenWaves = 0f;
    [SerializeField] float timeWaitAfterBoss = 2f;

    int gameStage;
    bool coroutineStartFlag;
    bool bossStartFlag;
    bool isBoss;

    List<GameObject> enemieList;
    WaveConfigSO currentWave;
    GameTime gameTime;
    ClassManager classManager;

    void Awake()
    {
        gameTime = FindObjectOfType<GameTime>();
        classManager = FindObjectOfType<ClassManager>();
    }

    void Start()
    {
        gameStage = 0;
        coroutineStartFlag = false;
        isBoss = false;
        enemieList = new List<GameObject>();
    }

    void Update()
    {           
        CheckBossTimer();
        RemoveDeadEnemies();

        if(coroutineStartFlag || bossStartFlag) return;

        if(isBoss && enemieList.Count == 0)
        {
            StartCoroutine(SpawnBoss(bosses[gameStage]));
        }
        else if(!isBoss)
        {
            StartCoroutine(SpawnEnemyWaves(waveConfigs[gameStage]));
        }
    }

    private void RemoveDeadEnemies()
    {
        enemieList.RemoveAll(item => item == null);
    }

    void CheckBossTimer()
    {
        if(Mathf.FloorToInt(gameTime.Timer) == bossTimers[gameStage] && !bossStartFlag)
        {
            gameTime.IsTimerRunning = false;
            isBoss = true;
        }  
    }

    IEnumerator SpawnEnemyWaves(ListOfWaveConfigSO currentWaveConfig)
    {
        coroutineStartFlag = true;
   
        var random = new System.Random();

        int start = 0;
        int end = currentWaveConfig.Count();

        int[] indexes = Enumerable.Range(start, end).OrderBy(x => random.Next()).ToArray();
        
        // Loop through waves
        for(int i = start; i < end; i++)
        {
            if(isBoss) break;

            currentWave = currentWaveConfig[indexes[i]];
            
            // Loop through enemies
            for(int j = 0; j < currentWave.GetEnemyCount(); j++)
            {
                if(isBoss) break;

                GameObject newEnemie = Instantiate(currentWave.GetEnemyPrefab(j), 
                                                   currentWave.GetStartingWayPoint().position, 
                                                   Quaternion.identity, transform);
                enemieList.Add(newEnemie);

                yield return new WaitForSeconds(currentWave.GetRandomSpawnTime());
            }

            yield return new WaitForSeconds(timeBetweenWaves);
        }
        
        coroutineStartFlag = false;
    }


    IEnumerator SpawnBoss(WaveConfigSO currentBoss)
    {
        coroutineStartFlag = true;
        bossStartFlag = true;

        currentWave = currentBoss;

        GameObject boss = Instantiate(currentWave.GetEnemyPrefab(0), 
                                      currentWave.GetStartingWayPoint().position, 
                                      Quaternion.identity, transform);
        
        yield return new WaitUntil(() => boss == null);
        
        gameTime.IsTimerRunning = true;
        isBoss = false;
        coroutineStartFlag = false;
        gameStage++;
        FindObjectOfType<AudioPlayer>().ChangeMusic(gameStage);

        if(gameStage == 3)
        {
            FindObjectOfType<LevelManager>().LoadGameOver(true);
        }

        yield return new WaitForSeconds(timeWaitAfterBoss);

        bossStartFlag = false;
    }

    public WaveConfigSO GetCurrentWaves()
    {
        return currentWave;
    }
}
