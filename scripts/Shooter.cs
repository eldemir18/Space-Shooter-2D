using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float baseProjectileSpeed = 10f;
    [SerializeField] float projectileLifeTime = 5f;
    [SerializeField] float baseFireRate = 1f;

    [Header("AI")]
    [SerializeField] float fireRateVariance = 0f;
    [SerializeField] float minFireRate = 0.1f;

    [Header("Shooting")]
    [SerializeField] int rapidShootingCount = 1;
    [SerializeField] int shootingCount = 1;
    [SerializeField] float autoShottingDelay = 0.1f;

    AudioPlayer audioPlayer;
    DamageDealer damageDealer;

    float currentProjectileSpeed;
    float currentFireRate;
    int currentDamage;

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

    void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
        damageDealer = projectilePrefab.GetComponent<DamageDealer>(); 
        
        currentFireRate = baseFireRate;
        currentProjectileSpeed = baseProjectileSpeed;
        currentDamage = damageDealer.BaseDamage;
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
                float[] offsets = new float[shootingCount];
                
                if (shootingCount == 1)
                {
                    offsets[0] = 0.0f;
                }
                else if (shootingCount == 2)
                {
                    offsets[0] = 1.0f; offsets[1] = -1.0f;
                }
                else
                {
                    offsets[0] = 1.0f; offsets[1] = 0.0f; offsets[2] = -1.0f;
                }
                
                Shooting(offsets);
                
                yield return new WaitForSeconds(autoShottingDelay);
            }
            yield return new WaitForSeconds(GetTimeToNextProjectile());
        }
    }

    void Shooting(float[] positions)
    {
        for(int i = 0; i < positions.Length; i++)
        {
            Vector3 position = new Vector3(transform.position.x + positions[i], transform.position.y);
            GameObject instance = Instantiate(projectilePrefab, position, Quaternion.identity);
            instance.GetComponent<DamageDealer>().BaseDamage = currentDamage;
            Rigidbody2D rb2D = instance.GetComponent<Rigidbody2D>();
            rb2D.velocity = transform.up * currentProjectileSpeed; 

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
