using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ClassInfo : MonoBehaviour
{
    [SerializeField] int[] coefficents = new int[4]; 
    [SerializeField] Slider[] sliders = new Slider[4];

    void Start()
    {
        AssingSliders();
    }

    void AssingSliders()
    {
        int defaultValue = 1;
        for(int i = 0; i < sliders.Length; i++)
        {
            sliders[i].maxValue = 18;
            sliders[i].value = coefficents[i] * (defaultValue + PlayerPrefs.GetInt("upgrades" + i.ToString(), 0));
        }
    }
}
