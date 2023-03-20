using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIDisplay : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] Slider healthSlider;
    [SerializeField] Health playerHealth;
    private float maxHealth;
    
    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    Score playerScore;

    void Awake()
    {
        playerScore = FindObjectOfType<Score>();
    }

    public void SetHealthBar()
    {
        maxHealth = playerHealth.CurrentHealth;
        healthSlider.maxValue = maxHealth;
    }

    void Update()
    {
        healthSlider.value = playerHealth.CurrentHealth;
        scoreText.text = playerScore.PlayerScore.ToString("000000000");
    }
}
