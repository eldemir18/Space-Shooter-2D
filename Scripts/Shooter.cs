using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float baseProjectileSpeed = 10f;
    [SerializeField] float projectileLifeTime = 3.5f;
    [SerializeField] float baseFireRate = 1f;

    [Header("AI")]
    [SerializeField] float fireRateVariance = 0f;
    [SerializeField] float minFireRate = 0.1f;

    [Header("Shooting")]
    [SerializeField] private int rapidShootingCount = 1;
    public int RapidShootingCount
    {
        get{return rapidShootingCount;}
        set{rapidShootingCount = value;}
    }

    [SerializeField] private int shootingCount = 1;
    public int ShootingCount
    {
        get{return shootingCount;}
        set{shootingCount = value;}
    }

    [SerializeField] private float shootingOffset = 1.0f;
    public float ShootingOffset
    {
        get{return shootingCount;}
        set{shootingOffset = value;}
    }

    [SerializeField] private float autoShootingDelay = 0.1f;
    public float AutoShootingDelay
    {
        get{return autoShootingDelay;}
        set{autoShootingDelay = value;}
    }

    private AudioPlayer audioPlayer;
    private DamageDealer damageDealer;

    private float currentProjectileSpeed;
    private float currentFireRate;
    private float currentProjectileSize;
    private int currentDamage;

    public int CurrentDamage
    {
        get{return currentDamage;}
        set{currentDamage = value;} 
    }

    public float CurrentFireRate
    {
        get{return currentFireRate;}
        set{currentFireRate = value;} 
    }

    public float CurrentProjectileSpeed
    {
        get{return currentProjectileSpeed;}
        set{currentProjectileSpeed = value;}
    }

    public float CurrentProjectileSize
    {
        get{return currentProjectileSize;}
        set{currentProjectileSize = value;}
    }

    void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
        damageDealer = projectilePrefab.GetComponent<DamageDealer>(); 
        
        currentFireRate = baseFireRate;
        currentProjectileSpeed = baseProjectileSpeed;
        currentDamage = damageDealer.BaseDamage;
        currentProjectileSize = 1f;
    }

    void Start()
    {
        Fire();
    }

    void Fire()
    {
        StartCoroutine(FireContinuously());
    }
    
    IEnumerator FireContinuously()
    {
        while(true)
        {
            for(int i = 0; i < rapidShootingCount; i++)
            {
                float[] offsetX = new float[shootingCount];
                float[] offsetY = new float[shootingCount]; 
                
                if (shootingCount == 1)
                {
                    offsetX[0] = 0.0f;
                    
                    offsetY[0] = 0.0f;
                }
                else if (shootingCount == 2)
                {
                    offsetX[0] = shootingOffset; offsetX[1] = -shootingOffset;
                    
                    offsetY[0] = 0.0f; offsetY[1] = 0.0f;
                }
                else if(shootingCount == 3)
                {
                    offsetX[0] = shootingOffset; offsetX[1] = 0.0f; offsetX[2] = -shootingOffset;

                    offsetY[0] = 0.0f; offsetY[1] = 0.0f; offsetY[2] = 0.0f;
                }
                else if(shootingCount == 4)
                {
                    offsetX[0] = shootingOffset; offsetX[1] = -shootingOffset; 
                    offsetX[2] = -3*shootingOffset; offsetX[3] = 3*shootingOffset;

                    offsetY[0] = 0.0f; offsetY[1] = 0.0f; 
                    offsetY[2] = -1.5f; offsetY[3] = -1.5f;
                }
                
                Shooting(offsetX, offsetY);
                
                yield return new WaitForSeconds(autoShootingDelay);
            }

            yield return new WaitForSeconds(GetTimeToNextProjectile());
        }
    }

    void Shooting(float[] positionX, float[] positionY)
    {
        for(int i = 0; i < shootingCount; i++)
        {
            Vector3 position = new Vector3(transform.position.x + positionX[i], transform.position.y + positionY[i]);
            GameObject instance = Instantiate(projectilePrefab, position, Quaternion.identity);
            
            DamageDealer damageDealer = instance.GetComponent<DamageDealer>();
            damageDealer.BaseDamage = currentDamage;
            damageDealer.transform.localScale = new Vector3
            (
                damageDealer.transform.localScale.x * currentProjectileSize, 
                damageDealer.transform.localScale.y * currentProjectileSize, 
                1
            );


            Rigidbody2D rb2D = instance.GetComponent<Rigidbody2D>();
            rb2D.velocity = transform.up * currentProjectileSpeed; 

            projectileLifeTime = 18f / Mathf.Abs(currentProjectileSpeed);
            Destroy(instance, projectileLifeTime);       
        }

        audioPlayer.PlayShootingClip();
    }

    float GetTimeToNextProjectile()
    {
        float timeToNextProjectile = Random.Range(currentFireRate - fireRateVariance, currentFireRate + fireRateVariance);
        timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, minFireRate, float.MaxValue);
        return timeToNextProjectile;
    }
}
