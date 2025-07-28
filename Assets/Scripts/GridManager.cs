using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int width = 10;
    public static int height = 20;
    public static Transform[,] grid = new Transform[width, height];

    public static Vector2 Round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    public static bool InsideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0);
    }

    public static void AddToGrid(Transform tetromino)
    {
        foreach (Transform child in tetromino)
        {
            Vector2 pos = Round(child.position);
            grid[(int)pos.x, (int)pos.y] = child;
        }
    }

    public static bool IsValidPosition(Transform tetromino)
    {
        foreach (Transform child in tetromino)
        {
            Vector2 pos = Round(child.position);

            if (!InsideBorder(pos))
                return false;

            if (grid[(int)pos.x, (int)pos.y] != null)
                return false;
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
                y--; // 같은 줄을 다시 검사 (중첩 제거)
            }
        }
    }

    private static bool IsFullLine(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (grid[x, y] == null)
                return false;
        }
        return true;
    }

    private static void DeleteLine(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            GameObject.Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    private static void MoveAllRowsDown(int startY)
    {
        for (int y = startY; y < height; ++y)
        {
            MoveRowDown(y);
        }
    }

    private static void MoveRowDown(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += Vector3.down;
            }
        }
    }

}