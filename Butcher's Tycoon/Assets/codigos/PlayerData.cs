using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;
    public int money = 0;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);       

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Update()
    {
        UpdateMoneyUI();
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public void UpdateMoneyUI()
    {
        if(moneyText != null)
        {
            moneyText.text = "Moedas: " + money;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Tycoon")
        {
            moneyText.gameObject.SetActive(false);
        }
        else
        {
            moneyText.gameObject.SetActive(true);
        }
    }
}
