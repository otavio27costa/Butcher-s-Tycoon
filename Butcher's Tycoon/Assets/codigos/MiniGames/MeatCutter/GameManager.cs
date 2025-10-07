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

    private int score;
    private float timeRemain;

    private void Awake()
    {
        Instance = this;
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
        if(timeRemain <= 0)
        {
            EndGame();
        }

        if(numbLife <= 0)
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
        Debug.Log($"O jogo acabou!! Sua pontuacao: {score}");
        Time.timeScale = 0;
    }

}
