using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInteractMessage : MonoBehaviour
{
    public static UIInteractMessage Instance;
    public GameObject panel;
    public Text messageText;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void ShowMessage(string msg)
    {
        messageText.text = msg;
        panel.SetActive(true);
    }

    public void HideMessage()
    {
        panel.SetActive(false);
    }
}
