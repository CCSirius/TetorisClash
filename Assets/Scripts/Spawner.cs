using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] tetrominoPrefabs;
    private Queue<GameObject> queue = new Queue<GameObject>();

    [Header("Next Preview")]
    public NextSlotManager nextSlotManager;

    public Vector3 SpawnPosition => transform.position;

    private void Start()
    {
        FillBag();
        SpawnNext();
    }

    public void SpawnNext()
    {
        if (queue.Count < 4)
            FillBag();

        GameObject prefab = queue.Dequeue();
        GameObject instance = Instantiate(prefab, SpawnPosition, Quaternion.identity);

        Tetromino t = instance.GetComponent<Tetromino>();
        if (t != null)
        {
            t.createGhostOnStart = true;
            t.originalPrefab = prefab;
        }

        nextSlotManager?.UpdateSlots();
    }

    public GameObject SpawnTetromino(GameObject prefab, bool createGhost)
    {
        GameObject instance = Instantiate(prefab, SpawnPosition, Quaternion.identity);
        Tetromino t = instance.GetComponent<Tetromino>();
        if (t != null)
        {
            t.createGhostOnStart = createGhost;
            t.originalPrefab = prefab;
        }
        return instance;
    }

    private void FillBag()
    {
        List<GameObject> bag = new List<GameObject>(tetrominoPrefabs);
        for (int i = 0; i < bag.Count; i++)
        {
            int r = Random.Range(i, bag.Count);
            (bag[i], bag[r]) = (bag[r], bag[i]);
        }

        foreach (var block in bag)
            queue.Enqueue(block);

        nextSlotManager?.Initialize(queue);
    }
}