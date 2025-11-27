using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGamePortal : MonoBehaviour
{
    private bool inside = false;
    public string miniGameName;
    public TMP_Text textPanel;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inside)
        {
            SceneController.instance.OpenMinigame(miniGameName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inside = true;
            textPanel.text = "Pressione 'E' para entrar no " + miniGameName;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIInteractMessage.Instance.HideMessage();
            textPanel.text = "";
        }
    }

    public void Leave()
    {
        Time.timeScale = 1;
        //lootPanel.SetActive(false);
        SceneController.instance.CloseMinigame(miniGameName);
        
    }
}
