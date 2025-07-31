using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int comboCount = 0;
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
        // TODO: 게임오버 UI, 재시작 버튼 등 활성화
        Time.timeScale = 0; // 일시정지
    }

    public void OnLineCleared()
    {
        comboCount++;
        Debug.Log($"Combo! x{comboCount}");
    }

    public void OnLineClearBreak()
    {
        if (comboCount > 0)
        {
            Debug.Log($"Combo ended at x{comboCount}");
            comboCount = 0;
        }
    }
}