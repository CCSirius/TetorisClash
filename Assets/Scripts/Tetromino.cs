using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public float fallTime = 0.5f;
    public float softDropMultiplier = 0.1f;
    private float previousTime;
    private GameObject ghost;

    public float repeatDelay = 0.2f;
    public float repeatRate = 0.05f;
    private float horizontalPressTime;
    private int lastDirection;

    private void Start()
    {
        if (!GridManager.IsValidPosition(transform))
        {
            Debug.Log("Game Over: Invalid starting position");
            enabled = false;
            return;
        }

        previousTime = Time.time;
        Invoke(nameof(CreateGhost), 0.01f);
    }

    private void Update()
    {
        float interval = fallTime;
        if (Input.GetKey(KeyCode.DownArrow)) interval *= softDropMultiplier;

        if (Time.time - previousTime > interval)
        {
            Move(Vector3.down);
            previousTime = Time.time;
        }

        HandleHorizontalInput();

        if (Input.GetKeyDown(KeyCode.Z)) Rotate(-90);
        else if (Input.GetKeyDown(KeyCode.X)) Rotate(90);

        if (Input.GetKeyDown(KeyCode.Space)) HardDrop();
        if (Input.GetKeyDown(KeyCode.C)) Hold();

        UpdateGhost();
    }

    void HandleHorizontalInput()
    {
        int direction = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) direction = -1;
        else if (Input.GetKey(KeyCode.RightArrow)) direction = 1;

        if (direction != 0)
        {
            if (direction != lastDirection)
            {
                horizontalPressTime = Time.time;
                Move(Vector3.right * direction);
                lastDirection = direction;
            }
            else if (Time.time - horizontalPressTime > repeatDelay &&
                     (Time.time - horizontalPressTime - repeatDelay) % repeatRate < Time.deltaTime)
            {
                Move(Vector3.right * direction);
            }
        }
        else
        {
            lastDirection = 0;
        }
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
                Destroy(ghost);
                enabled = false;
                FindObjectOfType<Spawner>().SpawnNext();
            }
        }
    }

    void Rotate(float angle)
    {
        transform.Rotate(0, 0, angle);
        foreach (Vector3 offset in GridManager.wallKickOffsets)
        {
            transform.position += offset;
            if (GridManager.IsValidPosition(transform)) return;
            transform.position -= offset;
        }
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
                break;
            }
        }

        GridManager.AddToGrid(transform);
        GridManager.CheckForLines();
        Destroy(ghost);
        enabled = false;
        FindObjectOfType<Spawner>().SpawnNext();
    }

    void Hold() { }

    void CreateGhost()
    {
        ghost = Instantiate(gameObject, transform.position, transform.rotation);
        DestroyImmediate(ghost.GetComponent<Tetromino>());

        foreach (Transform child in ghost.transform)
        {
            var sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color c = sr.color;
                c.a = 0.3f;
                sr.color = c;
            }
        }

        UpdateGhost();
    }

    void UpdateGhost()
    {
        if (!ghost) return;

        ghost.transform.position = transform.position;
        ghost.transform.rotation = transform.rotation;

        while (true)
        {
            ghost.transform.position += Vector3.down;
            if (!GridManager.IsValidPosition(ghost.transform, ignoreSelf: true))
            {
                ghost.transform.position += Vector3.up;
                break;
            }
        }
    }
}