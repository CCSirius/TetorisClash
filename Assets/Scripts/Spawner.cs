using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] tetrominoPrefabs;

    public void SpawnNext()
    {
        int index = Random.Range(0, tetrominoPrefabs.Length);
        Instantiate(tetrominoPrefabs[index], transform.position, Quaternion.identity);
    }

    private void Start()
    {
        SpawnNext(); // 첫 블록 생성
    }
}