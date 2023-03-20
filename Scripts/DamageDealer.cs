using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] bool isEnemy = false;
    [SerializeField] bool isBoss = false;
    [SerializeField] bool isPlayerProjectile = false;
    [SerializeField] int baseDamage = 10;

    Health health;

    public int BaseDamage
    {
        get{return baseDamage;}
        set{baseDamage = value;}
    }

    void Awake()
    {
        if(isEnemy)
        {
            health = GetComponent<Health>();
        }
    }

    public void Hit()
    {
        if(isEnemy)
        {
            health.PlayEffect(health.explosionEffect);
            health.audioPlayer.PlayExplosionClip();
        }
        else if(isPlayerProjectile)
        {
            int damage = PlayerPrefs.GetInt("damage", 0);
            PlayerPrefs.SetInt("damage", damage + baseDamage);

            GameObject.Find("Player").GetComponent<Health>().LifeSteal();
        }
        else if(isBoss) return;
        Destroy(gameObject);
    }
}
