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
        scoreText.text  = "Score: " + GameController.I.playerStats.score;
        livesText.text  = "Lives: " + GameController.I.playerStats.lives;
    }
}