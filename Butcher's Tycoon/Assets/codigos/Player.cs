using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public int moveSpeed = 5;
    private GridManager grid;
    public Animator animator;
    public TextMeshProUGUI textMeshProUGUI;


    private void Start()
    {
        grid = FindObjectOfType<GridManager>();
    }
    public void Update()
    {
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

        newPos.x = Mathf.Clamp(newPos.x, minX, maxX - 1);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        rb.MovePosition(newPos);

        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("IsUp", true);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("IsUp", false);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("IsDown", true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("IsDown", false);
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            animator.SetBool("IsMove", true);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            animator.SetBool("IsMove", false);
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            animator.SetBool("IsMove2", true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            animator.SetBool("IsMove2", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Maquina"))
        {
            textMeshProUGUI.text = ("Aperte 'E' para minigame");
        }
    }
}
