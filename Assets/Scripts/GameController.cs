using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController I;
    public CoverManager coverManager;
    public EnemySpawner enemySpawner;
    public MountedMachinegun machinegun;

    public int scorePerKill = 1;
    
    // Player Stats
    [System.Serializable] public class PlayerStats{
        public int lives = 5;       // Remaining Lives
        public int score = 0;       // Earned score (kill count)
        public int highscore = 0;
    }

    public PlayerStats playerStats;

    // Game screens to enable/disable
    [System.Serializable]
    private class ScreenData{
        public GameObject hud;
        public GameObject gameOver;
        public TextMeshProUGUI gameOverScoreText;
        public TextMeshProUGUI gameOverHighscoreText;
        public GameObject exitGamePopup;
    }

    [SerializeField] private ScreenData screenData;

    void Awake()
    {
        GameController.I = this;
        playerStats.highscore = PlayerPrefs.GetInt("highscore", 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool popupOpen = this.screenData.exitGamePopup.activeSelf;
            this.screenData.exitGamePopup.SetActive(!popupOpen);

            if (!popupOpen) { PauseGame(); }
            else { ResumeGame(); }
        }
    }

    void Start()
    {
        screenData.hud.SetActive(true);
        screenData.gameOver.SetActive(false);
        screenData.exitGamePopup.SetActive(false);
        LockCursor();
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void DamagePlayer(int damage)
    {
        playerStats.lives -= damage;
        if (playerStats.lives < 0)
        {
            GameOver();
        }
    }

    public void IncreaseScore(int amount)
    {
        playerStats.score += amount;
    }

    public void GameOver()
    {
        screenData.hud.SetActive(false);
        screenData.gameOver.SetActive(true);

        PauseGame();

        if (playerStats.score > playerStats.highscore)
        {
            playerStats.highscore = playerStats.score;
            PlayerPrefs.SetInt("highscore", playerStats.score);
            PlayerPrefs.Save();
        }
        screenData.gameOverScoreText.text = "Score: " + playerStats.score.ToString();
        screenData.gameOverHighscoreText.text = "Highscore: " + playerStats.highscore.ToString();
    }

    public void RestartGame()
    {
        //Reloading the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        enemySpawner.StopSpawner();
        machinegun.SetFiringEnabled(false);
        Cursor.lockState = CursorLockMode.None;
        UnlockCursor();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        enemySpawner.StartSpawner();
        machinegun.SetFiringEnabled(true);
        LockCursor();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
