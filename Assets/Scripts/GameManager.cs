using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;

    public int comboCount = 0;
    public int score = 0;

    private GameObject currentTetromino;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetCurrentTetromino(GameObject tetro)
    {
        currentTetromino = tetro;
    }

    public GameObject GetCurrentTetromino()
    {
        return currentTetromino;
    }

    public void DestroyCurrentTetromino()
    {
        if (currentTetromino != null)
        {
            Destroy(currentTetromino);
            currentTetromino = null;
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0;
    }

    public void OnLineCleared(int linesCleared)
    {
        int baseScore = 0;

        switch (linesCleared)
        {
            case 1: baseScore = 100; break;
            case 2: baseScore = 300; break;
            case 3: baseScore = 500; break;
            case 4: baseScore = 800; break;
        }

        int comboBonus = comboCount * 50;
        score += baseScore + comboBonus;

        comboCount++;
        Debug.Log($"Score: {score}, Combo x{comboCount}");
        UpdateUI();
    }

    public void OnLineClearBreak()
    {
        if (comboCount > 0)
            Debug.Log($"Combo ended at x{comboCount}");

        comboCount = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";

        if (comboText != null)
            comboText.text = comboCount > 1 ? $"Combo: x{comboCount}" : "";
    }
}