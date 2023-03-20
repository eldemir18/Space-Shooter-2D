using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UpgradesInfo : MonoBehaviour
{
    [SerializeField] int upgradeIndex;

    
    int[] damageCost = new int[5]{3000, 10000, 15000, 20000, 30000};
    int[] fireRateCost = new int[5]{50, 100, 200, 400, 1000};
    int[] healthCost = new int[5]{1000, 2000, 4000, 7500, 10000};
    int[] speedCost = new int[5]{100, 250, 500, 750, 1000};
    

    TextMeshProUGUI info;

    bool buttonActivate;

    void Awake()
    {
        info = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        AssingInfos();
    }

    public void AssingInfos()
    {
        if(upgradeIndex == 0) // Damage
        {   
            UpgradeLevels(damageCost, "damage", " damage given");
        }
        else if(upgradeIndex == 1) // Fire Rate
        { 
            UpgradeLevels(fireRateCost, "kill", " enemy killed");
        }
        else if(upgradeIndex == 2) // Health
        {
            UpgradeLevels(healthCost, "highScore0", " high score"); 
        }
        else if(upgradeIndex == 3) // Speed
        {
            UpgradeLevels(speedCost, "playTime", "(s) play time");
        }
    }

    void UpgradeLevels(int[] costs, string prefName, string infoText)
    {
        int index = PlayerPrefs.GetInt("upgrades" + upgradeIndex.ToString(), 0);
        if(index == 5)
        {
            info.text = "Max Level";
        }
        else
        {
            int progress = Convert.ToInt32((100 * PlayerPrefs.GetInt(prefName, 0)) / costs[index]);

            if(progress >= 100)
            {
                progress = 100;
                buttonActivate = true;
            }
            else
            {
                buttonActivate = false;
            }

            info.text = costs[index].ToString() + infoText + "\nprogress: %" + progress.ToString();
        }
    }

    public bool GetButtonActivate()
    {
        return buttonActivate;
    }
}
