using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Upgrades : MonoBehaviour
{
    [SerializeField] Button[] upgradeButtons;
    [SerializeField] GameObject[] upgradeInfos;
    [SerializeField] Slider[] levels;

    int[] upgrades;

    void Start()
    {
        SetUpgrades();
        DisplayLevels();
    }

    public void SetUpgrades()
    {
        upgrades = new int[4];
        
        for(int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i] = PlayerPrefs.GetInt("upgrades" + i.ToString(),0);
            if(upgrades[i] == 5)
            {
                upgradeButtons[i].interactable = false;
            }
            levels[i].maxValue = 5;
        }
    }

    public void DisplayLevels()
    {
        for(int i = 0; i < levels.Length; i++)
        {
            levels[i].value = upgrades[i];
        }
    }


    public void UpgradeLevel(int index)
    {   
        if(upgrades[index] < 5 && upgradeInfos[index].GetComponent<UpgradesInfo>().GetButtonActivate())
        {
            upgrades[index]++;
            
            PlayerPrefs.SetInt("upgrades" + index.ToString(), upgrades[index]);
            
            DisplayLevels();
            
            if(upgrades[index] == 5)
            {
                upgradeButtons[index].interactable = false;
            }

            upgradeInfos[index].GetComponent<UpgradesInfo>().AssingInfos();
        }
    }


}
