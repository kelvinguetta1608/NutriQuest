using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI winText;

    private string currentTargetCategory = "Ansiedad";
    private List<string> unlockedCategories = new List<string> { "Ansiedad" };
    private readonly string[] categoryOrder = { "Ansiedad", "Sueño", "ActividadFisica", "Estres" };
    private string unlockSavePath;

    private int score = 0;
    private int lives = 3;
    private int currentLevel = 1;
    private int[] scoreGoals = { 100, 200, 300 };
    private float[] spawnIntervals = { 1f, 0.7f, 0.5f };
    private float[] foodSpeeds = { 10f, 10f, 10f }; // Modificado: Diferentes velocidades

    [System.Serializable]
    private class UnlockData
    {
        public List<string> unlockedCategories;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            unlockSavePath = Application.persistentDataPath + "/unlockData.json";
            LoadUnlockData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GAMEF")
        {
            InitializeGame();
        }
    }

    private void InitializeGame()
    {
        InitializeUIReferences();
        ResetGameState();
        UpdateUI();

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void InitializeUIReferences()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            scoreText = canvas.transform.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
            livesText = canvas.transform.Find("LivesText")?.GetComponent<TextMeshProUGUI>();
            levelText = canvas.transform.Find("LevelText")?.GetComponent<TextMeshProUGUI>();
            gameOverPanel = canvas.transform.Find("GameOverPanel")?.gameObject;
            winPanel = canvas.transform.Find("WinPanel")?.gameObject;

            if (winPanel != null)
            {
                continueButton = winPanel.transform.Find("ContinueButton")?.GetComponent<Button>();
                winText = winPanel.transform.Find("WinText")?.GetComponent<TextMeshProUGUI>();
                Button restartWin = winPanel.transform.Find("RestartButton")?.GetComponent<Button>();
                Button menuWin = winPanel.transform.Find("MenuButton")?.GetComponent<Button>();
                if (continueButton != null) continueButton.onClick.AddListener(ContinueGame);
                if (restartWin != null) restartWin.onClick.AddListener(RestartGame);
                if (menuWin != null) menuWin.onClick.AddListener(GoToMenu);
            }

            if (gameOverPanel != null)
            {
                Button restartOver = gameOverPanel.transform.Find("RestartButton")?.GetComponent<Button>();
                Button menuOver = gameOverPanel.transform.Find("MenuButton")?.GetComponent<Button>();
                Button exitOver = gameOverPanel.transform.Find("ExitButton")?.GetComponent<Button>();
                if (restartOver != null) restartOver.onClick.AddListener(RestartGame);
                if (menuOver != null) menuOver.onClick.AddListener(GoToMenu);
                if (exitOver != null) exitOver.onClick.AddListener(QuitGame);
            }

            Button volverButton = canvas.transform.Find("VolverButton")?.GetComponent<Button>();
            if (volverButton != null)
            {
                volverButton.onClick.AddListener(GoToMenu);
            }
        }
    }

    private void ResetGameState()
    {
        score = 0;
        lives = 3;
        currentLevel = 1;
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Puntos: " + score;
        if (livesText != null)
            livesText.text = "Vidas: " + lives;
        if (levelText != null)
            levelText.text = "Nivel: " + (currentLevel == 1 ? "Fácil" : currentLevel == 2 ? "Medio" : "Difícil") + " (Tag: " + currentTargetCategory + ")";
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
        if (score >= scoreGoals[currentLevel - 1])
        {
            if (currentLevel < 3)
            {
                WinLevel();
            }
            else
            {
                GameWon();
            }
        }
    }

    public void LoseLife()
    {
        if (lives > 0)
        {
            lives--;
            UpdateUI();
            if (lives <= 0)
            {
                GameOver();
            }
        }
    }

    void WinLevel()
    {
        if (winText != null)
            winText.text = "¡Nivel Completado!";
        if (continueButton != null)
            continueButton.gameObject.SetActive(true);
        if (winPanel != null)
            winPanel.SetActive(true);
        Time.timeScale = 0;

        currentLevel++;
        lives = 3;
        UpdateUI();
    }

    void GameWon()
    {
        string nextCategory = GetNextCategory();
        if (nextCategory != null && !unlockedCategories.Contains(nextCategory))
        {
            unlockedCategories.Add(nextCategory);
            SaveUnlockData();
            if (winText != null)
            {
                // Modificado: Mostrar mensaje de recompensa
                string rewardMessage = GetRewardMessage(nextCategory);
                winText.text = $"¡Juego Completado!\nHas desbloqueado el minijuego de {nextCategory}!\n{rewardMessage}";
            }
        }
        else
        {
            if (winText != null)
                winText.text = "¡Juego Completado!";
        }

        if (continueButton != null)
            continueButton.gameObject.SetActive(false);
        if (winPanel != null)
            winPanel.SetActive(true);
        Time.timeScale = 0;
    }

    void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        if (winPanel != null)
            winPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        score = 0;
        lives = 3;
        currentLevel = 1;
        UpdateUI();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GAMEOBJ");
    }

    public float GetSpawnInterval()
    {
        return spawnIntervals[currentLevel - 1];
    }

    public float GetFoodSpeed()
    {
        return foodSpeeds[currentLevel - 1];
    }

    public string GetTargetCategory()
    {
        return currentTargetCategory;
    }

    public void SetTargetCategory(string category)
    {
        if (unlockedCategories.Contains(category))
        {
            currentTargetCategory = category;
        }
    }

    public bool IsCategoryUnlocked(string category)
    {
        return unlockedCategories.Contains(category);
    }

    private string GetNextCategory()
    {
        int currentIndex = Array.IndexOf(categoryOrder, currentTargetCategory);
        if (currentIndex >= 0 && currentIndex < categoryOrder.Length - 1)
        {
            return categoryOrder[currentIndex + 1];
        }
        return null;
    }

    private void SaveUnlockData()
    {
        UnlockData data = new UnlockData { unlockedCategories = unlockedCategories };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(unlockSavePath, json);
    }

    private void LoadUnlockData()
    {
        try
        {
            if (File.Exists(unlockSavePath))
            {
                string json = File.ReadAllText(unlockSavePath);
                UnlockData data = JsonUtility.FromJson<UnlockData>(json);
                if (data?.unlockedCategories != null)
                {
                    unlockedCategories = new List<string>(data.unlockedCategories);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al cargar datos de desbloqueo: {e.Message}");
        }
    }

    // Nuevo: Mensaje de recompensa por categoría desbloqueada
    private string GetRewardMessage(string category)
    {
        switch (category)
        {
            case "Sueño":
                return "¡Felicidades! Ahora puedes mejorar tu descanso con este nuevo desafío.";
            case "ActividadFisica":
                return "¡Gran trabajo! Activa tu cuerpo con este nuevo minijuego.";
            case "Estres":
                return "¡Excelente! Alivia tu mente con este nuevo reto desbloqueado.";
            default:
                return "";
        }
    }
}