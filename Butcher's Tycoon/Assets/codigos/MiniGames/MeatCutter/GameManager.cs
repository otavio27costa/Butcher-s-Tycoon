using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI numLife;
    [SerializeField] private float gameTime;
    [SerializeField] private int numbLife;
    [SerializeField] private int coinForPoint;
    [SerializeField] private int earnedMoney;
    [SerializeField] private TextMeshProUGUI earnedMoneyText;

    private bool gameEnded = false;
    private int score;
    private float timeRemain;
    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timeRemain = gameTime;
        score = 0;
        UpdateUI();
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if(gameEnded) return;

        timeRemain -= Time.deltaTime;
        if(timeRemain <= 0 || numbLife <= 0)
        {
            timeRemain = 0;
            EndGame();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = "Pontos: " + score;
        timerText.text = "Tempo: " + Mathf.CeilToInt(timeRemain);
        numLife.text = "Vidas:" + numbLife;
    }

    public void AddScore(int value)
    {
        score += value;
    }

    public void HitRotten()
    {
        score -= 5;
        numbLife --;
    }

    public void EndGame()
    {
        if(gameEnded) return;
        gameEnded = true;

        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        timeRemain = 0;
        Debug.Log("jogo acabou");
        earnedMoney = score * coinForPoint;

        earnedMoneyText.text = $"Voce ganhou {earnedMoney} moedas!!";
        PlayerData.instance.AddMoney(earnedMoney);
    }

}
