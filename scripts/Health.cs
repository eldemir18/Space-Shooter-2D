using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int baseHealth = 50;
    
    int currentHealth;
    public int CurrentHealth
    {
        get{return currentHealth;}
        set{currentHealth = value;}
    }

    [Header("Effects")]
    [SerializeField] public ParticleSystem hitEffect;
    [SerializeField] public ParticleSystem explosionEffect;
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
