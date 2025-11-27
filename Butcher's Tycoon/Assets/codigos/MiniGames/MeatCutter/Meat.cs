using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : MonoBehaviour
{
    public bool isRotten;
    public int scoreMeat = 10;
    public GameManager gameManager;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Blade"))
        {
            if (isRotten)
            {
                GameManager.Instance.HitRotten();
            }
            else
            {
                GameManager.Instance.AddScore(scoreMeat);
            }

            Destroy(gameObject);
        }
    }
}
