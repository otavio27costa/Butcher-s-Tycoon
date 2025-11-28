using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGamePortal : MonoBehaviour
{
    public bool inside = false;
    public string miniGameName;
    public TMP_Text textPanel;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inside)
        {
            SceneController.instance.OpenMinigame(miniGameName);
        }
    }

    public void Leave()
    {
        Time.timeScale = 1;
        //lootPanel.SetActive(false);
        SceneController.instance.CloseMinigame(miniGameName);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inside = true;
        }
    }
}
