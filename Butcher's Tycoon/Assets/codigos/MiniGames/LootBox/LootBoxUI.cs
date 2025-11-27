using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LootBoxUI : MonoBehaviour
{
    public static LootBoxUI instance;
    public Sprite rewardIcon;
    public TextMeshProUGUI rewardNameText;
    [SerializeField] private Animator animator;
    public float timer = 0;

    public GameObject panel;

    private LootReward reward;
    public string miniGameName;

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void Awake()
    {
        instance = this;
        panel.SetActive(false);
    }

    public void AnimateTrigger()
    {
        panel.SetActive(true);
        animator.SetBool("GameEnd", true);
    }


    public void ShowReward(LootReward r)
    {
        
        reward = r;
        rewardNameText.text = r.rewardName;
        if (r.moneyAmount > 0)
           PlayerData.instance.AddMoney(r.moneyAmount);
    }

    public void Close()
    {
        SceneController.instance.CloseMinigame(miniGameName);
        panel.SetActive(false);
    }
}
