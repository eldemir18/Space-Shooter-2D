using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectSlider;

    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("Music Volume", 0.5f);
        effectSlider.value = PlayerPrefs.GetFloat("Effect Volume", 0.5f);
    }
}
