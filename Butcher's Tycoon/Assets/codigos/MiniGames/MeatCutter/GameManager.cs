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

    public bool gameEnded = false;
    private int score;
    private float timeRemain;
    public Lootbox lootbox;
    public LootBoxUI lootBoxUI;
    public float timeAnimation = 3;
    public int animeCount = 0;
    public bool endScene;
    public int rewardNumb = 0;

    private void Awake()
    {
        Instance = this;
        rewardNumb = 0;
        
    }

    private void Start()
    {
        timeRemain = gameTime;
        score = 0;
        UpdateUI();
    }

    private void Update()
    {
        timeRemain -= Time.deltaTime;
        if(timeRemain <= 0 || numbLife <= 0)
        {
            timeRemain = 0;
            EndGame();
        }

        if(endScene == true)
        {
            GameObject meat = GameObject.FindWithTag("Meat");
            Destroy(meat.gameObject);
            Debug.Log("Carne excluida");
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
        gameEnded = true;
        timeRemain = 0;

        if(animeCount < 1)
        {
            LootBoxUI.instance.AnimateTrigger();
            animeCount++;
            Debug.Log("animacao iniciada");
        }

        if(lootBoxUI.timer >= timeAnimation && rewardNumb < 1)
        {
            LootReward reward = LootBoxSystem.GetRewardByScore(lootbox, score);
            lootBoxUI.ShowReward(reward);
            endScene = true;
            Time.timeScale = 0;
            rewardNumb++;
        }
       
    }

}
