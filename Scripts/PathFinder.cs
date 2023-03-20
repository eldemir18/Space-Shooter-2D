using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] bool isLoop = false;
    [SerializeField] bool isSpeedSame = true;

    EnemySpawner enemySpawner;
    WaveConfigSO waveConfig;
    List<Transform> wayPoints;
    // New
    List<float> speeds;
    // New
    int wayPointIndex = 0;
    bool pathDirection;

    void Awake()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Start()
    {
        waveConfig = enemySpawner.GetCurrentWaves();

        wayPoints = waveConfig.GetWayPoints();
        speeds = waveConfig.GetMoveSpeeds();

        transform.position = wayPoints[wayPointIndex].position;
        wayPointIndex++;
        pathDirection = true;
    }

    void Update()
    {
        if(isLoop)
        {
            LoopPath();
        }
        else
        {
            FollowPath();
        }
    }

    void LoopPath()
    {
        if (wayPointIndex == wayPoints.Count || wayPointIndex == 0)
        {
            pathDirection ^= true;
            wayPointIndex += pathDirection ? 1 : -1;
        }

        Vector3 targetPosition = GetNextTargetPosition();

        if (transform.position == targetPosition)
        {
            wayPointIndex += pathDirection ? 1 : -1;
        }
    }

    void FollowPath()
    {
        if(wayPointIndex < wayPoints.Count)
        {
            Vector3 targetPosition = GetNextTargetPosition();
            if(transform.position == targetPosition)
            {
                wayPointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    Vector3 GetNextTargetPosition()
    {
        Vector3 targetPosition = wayPoints[wayPointIndex].position;
        
        float delta;
        if(isSpeedSame)
        {
            delta = speeds[0] * Time.deltaTime;
        }
        else
        {
            delta = speeds[wayPointIndex] * Time.deltaTime;
        }
        
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta);
        return targetPosition;
    }
}
