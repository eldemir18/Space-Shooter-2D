using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFillGradient : MonoBehaviour
{
    [SerializeField] Slider slider;

    Image sliderImage;
 
    void Awake()
    {
        sliderImage = GetComponent<Image>();
    }
 
    void Update()
    {
        sliderImage.color = Color.Lerp(Color.cyan, Color.yellow, slider.value / slider.maxValue);
    }
}
