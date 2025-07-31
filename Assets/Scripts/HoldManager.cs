using UnityEngine;
using UnityEngine.UI;

public class HoldManager : MonoBehaviour
{
    public Image holdSlot;
    private GameObject heldTetrominoPrefab;
    private bool usedThisTurn = false;

    public void Hold(GameObject current)
    {
        if (usedThisTurn) return;

        // 모든 고스트 제거
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (var g in ghosts)
            Destroy(g);

        Tetromino tetro = current.GetComponent<Tetromino>();
        if (tetro == null) return;

        GameObject prefabToHold = tetro.originalPrefab;

        if (heldTetrominoPrefab == null)
        {
            heldTetrominoPrefab = prefabToHold;
            UpdateHoldUI(tetro.iconSprite);
            FindObjectOfType<Spawner>().SpawnNext();
        }
        else
        {
            GameObject temp = heldTetrominoPrefab;
            heldTetrominoPrefab = prefabToHold;
            UpdateHoldUI(tetro.iconSprite);

            FindObjectOfType<Spawner>().SpawnTetromino(temp, true);
        }

        usedThisTurn = true;
        Destroy(current);
    }

    public void ResetHoldTurn()
    {
        usedThisTurn = false;
    }

    private void UpdateHoldUI(Sprite icon)
    {
        holdSlot.sprite = icon;
        holdSlot.color = Color.white;
        holdSlot.preserveAspect = true;
    }
}