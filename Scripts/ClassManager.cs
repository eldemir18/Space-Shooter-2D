using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    private bool isClassSelected = false;

    public bool IsClassSelected 
    {
        get {return isClassSelected;}
        set {isClassSelected = true;}
    }

    Player player;
    Shooter shooter;
    Health health;

    void Awake()
    {
        player = playerPrefab.GetComponent<Player>();
        shooter = playerPrefab.GetComponent<Shooter>();
        health = playerPrefab.GetComponent<Health>();
    }

    public void ApplyClassToPlayer(int classID)
    {
        // Set player sprite
        player.SelectSprite(classID);

        int[] upgrades = new int[4];
        
        for(int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i] = PlayerPrefs.GetInt("upgrades" + i.ToString(), 1);
        }
        
        int[] coefficents;

        if(classID == 0)
        {    
            coefficents = new int[4] {6,1,6,1};
            shooter.CurrentProjectileSize = 1.75f;
            health.HasHealthRegen = true;
            health.HealthRegenAmount = 0.01f;
        }
        else if(classID == 1)
        {
            coefficents = new int[4] {3,3,3,3};
            shooter.ShootingCount = 2;
            shooter.ShootingOffset = 0.35f; 

            health.HasHealthRegen = true;
            health.HealthRegenAmount = 0.005f;
        }
        else
        {
            coefficents = new int[4] {1,6,1,6};
            shooter.RapidShootingCount = 2;
            shooter.AutoShootingDelay = 0.1f;

            health.HasLifeSteal = true;
            health.LifeStealAmount = 0.05f;
        }

        shooter.CurrentDamage = shooter.CurrentDamage + (coefficents[0] * upgrades[0] * 5);
        shooter.CurrentFireRate = Mathf.Max(shooter.CurrentFireRate - (coefficents[1] * upgrades[1] * 0.045f), 0.1f);
        shooter.CurrentProjectileSpeed = shooter.CurrentProjectileSpeed + (coefficents[3] * upgrades[0] * 0.6f);
        health.CurrentHealth = health.CurrentHealth + (coefficents[2] * upgrades[2] * 20);
        health.MaxHealth = (int)(health.CurrentHealth);
        player.CurrentMoveSpeed = player.CurrentMoveSpeed + (coefficents[3] * upgrades[0] * 0.25f);

        /*
        Debug.Log("Damage:" + shooter.CurrentDamage.ToString());
        Debug.Log("Fire Rate:" + shooter.CurrentFireRate.ToString());
        Debug.Log("P Speed:" + shooter.CurrentProjectileSpeed.ToString());
        Debug.Log("Health:" + health.CurrentHealth.ToString());
        Debug.Log("M Speed:" + player.CurrentMoveSpeed.ToString());
        */
        
        Cursor.visible = false;
    }
}
