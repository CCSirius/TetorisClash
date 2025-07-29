using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int width = 10;
    public static int height = 20;
    public static Transform[,] grid = new Transform[width, height];

    public static readonly Vector3[] wallKickOffsets = {
        Vector3.zero, Vector3.right, Vector3.left,
        Vector3.right * 2, Vector3.left * 2
    };

    public static Vector2Int Round(Vector2 pos) => Vector2Int.RoundToInt(pos);

    public static bool InsideBorder(Vector2Int pos) =>
        pos.x >= 0 && pos.x < width && pos.y >= 0;

    public static void AddToGrid(Transform tetromino)
    {
        foreach (Transform child in tetromino)
        {
            Vector2Int pos = Round(child.position);
            if (pos.y < height && InsideBorder(pos))
                grid[pos.x, pos.y] = child;
        }
    }

    public static bool IsValidPosition(Transform tetromino, bool ignoreSelf = false)
    {
        foreach (Transform child in tetromino)
        {
            Vector2Int pos = Round(child.position);
            if (!InsideBorder(pos)) return false;

            if (pos.y < height)
            {
                var cell = grid[pos.x, pos.y];
                if (cell != null && (!ignoreSelf || cell.parent != tetromino))
                    return false;
            }
        }
        return true;
    }

    public static void CheckForLines()
    {
        for (int y = 0; y < height; ++y)
        {
            if (IsFullLine(y))
            {
                DeleteLine(y);
                MoveAllRowsDown(y + 1);
                y--;
            }
        }
    }

    private static bool IsFullLine(int y)
    {
        for (int x = 0; x < width; ++x)
            if (grid[x, y] == null) return false;
        return true;
    }

    private static void DeleteLine(int y)
    {
        HashSet<Transform> parents = new HashSet<Transform>();
        for (int x = 0; x < width; ++x)
        {
            if (grid[x, y] != null)
            {
                parents.Add(grid[x, y].parent);
                GameObject.Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }
        GameManager.Instance.StartCoroutine(CleanupEmptyParents(parents));
    }

    private static void MoveAllRowsDown(int startY)
    {
        for (int y = startY; y < height; y++)
            MoveRowDown(y);
    }

    private static void MoveRowDown(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += Vector3.down;
            }
        }
    }

    private static IEnumerator CleanupEmptyParents(HashSet<Transform> parents)
    {
        yield return null;
        foreach (var p in parents)
            if (p != null && p.childCount == 0)
                GameObject.Destroy(p.gameObject);
    }
}