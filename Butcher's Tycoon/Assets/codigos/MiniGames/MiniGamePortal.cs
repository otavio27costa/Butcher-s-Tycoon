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

    public void Leave()
    {
        Time.timeScale = 1;
        //lootPanel.SetActive(false);
        SceneController.instance.CloseMinigame(miniGameName);
        
    }
}
