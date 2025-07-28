using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public float fallTime = 1.0f;
    private float previousTime;

    private void Start()
    {
        previousTime = Time.time;
    }

    private void Update()
    {
        // 자동 하강 + ↓ 키로 빠른 하강
        if (Time.time - previousTime > fallTime || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector3.down);
            previousTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Move(Vector3.left);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            Move(Vector3.right);

        if (Input.GetKeyDown(KeyCode.Z))
            Rotate(-90);
        else if (Input.GetKeyDown(KeyCode.X))
            Rotate(90);

        if (Input.GetKeyDown(KeyCode.Space))
            HardDrop();

        if (Input.GetKeyDown(KeyCode.C))
            Hold(); // 추후 구현
    }

    void Move(Vector3 direction)
    {
        transform.position += direction;

        if (!GridManager.IsValidPosition(transform))
        {
            transform.position -= direction;

            if (direction == Vector3.down)
            {
                GridManager.AddToGrid(transform);
                GridManager.CheckForLines();
                this.enabled = false;
                FindObjectOfType<Spawner>().SpawnNext();
            }
        }

    }

    void Rotate(float angle)
    {
        transform.Rotate(0, 0, angle);

        if (!GridManager.IsValidPosition(transform))
            transform.Rotate(0, 0, -angle);
    }

    void HardDrop()
    {
        while (true)
        {
            transform.position += Vector3.down;
            if (!GridManager.IsValidPosition(transform))
            {
                transform.position += Vector3.up;
                GridManager.AddToGrid(transform);
                this.enabled = false;
                FindObjectOfType<Spawner>().SpawnNext();
                break;
            }
        }
    }

    void Hold()
    {
        // TODO: 나중에 Hold 시스템 구현
    }
}