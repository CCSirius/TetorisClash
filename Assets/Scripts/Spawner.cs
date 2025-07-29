using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] tetrominoPrefabs;
    private Queue<GameObject> queue = new Queue<GameObject>();

    private void Start() => SpawnNext();

    public void SpawnNext()
    {
        if (queue.Count == 0) FillBag();
        Instantiate(queue.Dequeue(), transform.position, Quaternion.identity);
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
    }
}
