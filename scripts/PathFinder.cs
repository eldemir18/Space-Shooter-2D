using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] bool isLoop = false;

    EnemySpawner enemySpawner;
    WaveConfigSO waveConfig;
    List<Transform> wayPoints;
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
        float delta = waveConfig.GetMoveSpeed() * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta);
        return targetPosition;
    }
}
