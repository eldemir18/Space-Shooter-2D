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
    [SerializeField] float timeToWaitBoss = 5f;
    

    int gameStage;
    bool coroutineStartFlag;
    bool bossStartFlag;
    bool isBoss;

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
    }

    void Update()
    {           
        CheckBossTimer();

        if(coroutineStartFlag || bossStartFlag) return;

        if(isBoss)
        {
            StartCoroutine(SpawnBoss(bosses[gameStage]));
        }
        else
        {
            StartCoroutine(SpawnEnemyWaves(waveConfigs[gameStage]));
        }
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

                Instantiate(currentWave.GetEnemyPrefab(j), 
                            currentWave.GetStartingWayPoint().position, 
                            Quaternion.identity, transform);

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

        yield return new WaitForSeconds(timeToWaitBoss);

        GameObject boss = Instantiate(currentWave.GetEnemyPrefab(0), 
                                      currentWave.GetStartingWayPoint().position, 
                                      Quaternion.identity, transform);
        
        yield return new WaitUntil(() => boss == null);
        
        gameTime.IsTimerRunning = true;
        isBoss = false;
        coroutineStartFlag = false;
        gameStage++;
        FindObjectOfType<AudioPlayer>().ChangeMusic(gameStage);

        if(gameStage == 2)
        {
            FindObjectOfType<LevelManager>().LoadGameOver(true);
        }

        yield return new WaitForSeconds(timeToWaitBoss);

        bossStartFlag = false;
    }

    public WaveConfigSO GetCurrentWaves()
    {
        return currentWave;
    }
}
