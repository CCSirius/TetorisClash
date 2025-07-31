using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NextSlotManager : MonoBehaviour
{
    public Image[] nextSlots; // Image_NextSlot1~3 연결
    public Sprite[] blockSprites; // Tetromino 프리팹과 Sprite 순서 동일

    private Queue<GameObject> blockQueue;

    public void Initialize(Queue<GameObject> queue)
    {
        blockQueue = queue;
        UpdateSlots();
    }

    public void UpdateSlots()
    {
        if (blockQueue == null) return;

        GameObject[] preview = blockQueue.ToArray();
        for (int i = 0; i < nextSlots.Length; i++)
        {
            if (i < preview.Length)
            {
                var prefab = preview[i];
                string name = prefab.name.Replace("(Clone)", "").Trim();
                Sprite sprite = FindSpriteByName(name);
                nextSlots[i].sprite = sprite;
                nextSlots[i].color = Color.white;
            }
            else
            {
                nextSlots[i].sprite = null;
                nextSlots[i].color = Color.clear;
            }
        }
    }

    private Sprite FindSpriteByName(string name)
    {
        foreach (Sprite s in blockSprites)
        {
            if (s.name == name) return s;
        }
        return null;
    }
}