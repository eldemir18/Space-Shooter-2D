using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int baseHealth = 50;
    
    private float currentHealth;
    public float CurrentHealth
    {
        get{return currentHealth;}
        set{currentHealth = value;}
    }

    private int maxHealth;
    public int MaxHealth
    {
        get{return maxHealth;}
        set{maxHealth = value;}
    }

    private bool hasHealthRegen = false;
    public bool HasHealthRegen
    {
        get{return hasHealthRegen;}
        set{hasHealthRegen = value;}
    }

    private float healthRegenAmount;
    public float HealthRegenAmount
    {
        get{return healthRegenAmount;}
        set{healthRegenAmount = value;}
    }

    private bool hasLifeSteal = false;
    public bool HasLifeSteal
    {
        get{return hasLifeSteal;}
        set{hasLifeSteal = value;}
    }

    private float lifeStealAmount;
    public float LifeStealAmount
    {
        get{return lifeStealAmount;}
        set{lifeStealAmount = value;}
    }

    [Header("Effects")]
    [SerializeField] public ParticleSystem hitEffect;
    [SerializeField] public ParticleSystem explosionEffect;

    [HideInInspector]
    public AudioPlayer audioPlayer;

    [Header("Camera Shake")]
    [SerializeField] bool isCameraShake = false;
    CameraShake cameraShake;

    [Header("Score")]
    [SerializeField] int enemyScore = 50;
    Score playerScore;

    LevelManager levelManager;
    

    void Awake()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        playerScore = FindObjectOfType<Score>();
        levelManager = FindObjectOfType<LevelManager>();

        currentHealth = baseHealth;
        maxHealth = baseHealth;
    }

    void Update()
    {
        HealthRegen();
    }

    private void HealthRegen()
    {
        if (hasHealthRegen)
        {
            currentHealth = MathF.Min(currentHealth + (currentHealth * healthRegenAmount * Time.deltaTime), maxHealth);
        }
    }

    public void LifeSteal()
    {
        if(hasLifeSteal)
        {
            currentHealth = MathF.Min(currentHealth + (currentHealth * lifeStealAmount), maxHealth);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();

        if(damageDealer != null)
        {
            TakeDamage(damageDealer.BaseDamage);
            ShakeCamera();
            damageDealer.Hit();
        }
    }

    void ShakeCamera()
    {
        if(isCameraShake)
        {
            cameraShake.Play();
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            PlayEffect(explosionEffect);
            audioPlayer.PlayExplosionClip();

            // If it is enemy
            if(gameObject.CompareTag("Enemy"))
            {
                playerScore.PlayerScore += enemyScore;
                PlayerPrefs.SetInt("kill", PlayerPrefs.GetInt("kill", 0) + 1);


            }
            // If it is player
            else if(gameObject.CompareTag("Player"))
            {
                FindObjectOfType<GameTime>().SaveTime();
                levelManager.LoadGameOver(false);
            }

            Destroy(gameObject);
        }
        else
        {
            PlayEffect(hitEffect);
        }
    }

    public void PlayEffect(ParticleSystem effect)
    {
        Instantiate(effect, transform.position, Quaternion.identity);
    }
}
