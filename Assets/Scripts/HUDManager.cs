using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;

    void Update()
    {
        // Reloading the HUD every frame isn't very efficient,
        // but as it's a small PC game I want to avoid over-optimization
        scoreText.text  = "Score: " + GameController.I.playerStats.score;
        livesText.text  = "Lives: " + GameController.I.playerStats.lives;
    }
}