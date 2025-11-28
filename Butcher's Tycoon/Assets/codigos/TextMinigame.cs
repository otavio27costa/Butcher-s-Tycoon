using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMinigame : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Maquina"))
        {
            textMeshProUGUI.text = ("Aperte 'E' para minigame");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Maquina"))
        {
            textMeshProUGUI.text = ("");
        }
    }

}
