using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public int moveSpeed = 5;
    private GridManager grid;

    private void Start()
    {
        grid = FindObjectOfType<GridManager>();
    }
    public void Update(){
        if (grid == null) return;

        Vector2 movement = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );

        Vector2 newPos = (Vector2)transform.position + movement * moveSpeed * Time.deltaTime;

        float minX = 0f;
        float maxX = grid.width;
        float minY = 0f;
        float maxY = grid.height;

        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        rb.MovePosition(newPos);
    }
}
