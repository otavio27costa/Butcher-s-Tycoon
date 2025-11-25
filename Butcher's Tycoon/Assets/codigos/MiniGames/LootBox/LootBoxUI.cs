using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LootBoxUI : MonoBehaviour
{
    public Sprite rewardIcon;
    public TextMeshProUGUI rewardNameText;

    public GameObject panel;

    private LootReward reward;
    public string miniGameName;

    public void ShowReward(LootReward r)
    {
        reward = r;

        
        rewardNameText.text = r.rewardName;

        panel.SetActive(true);

        if(r.moneyAmount > 0)
           PlayerData.instance.AddMoney(r.moneyAmount);
    }

    public void Close()
    {
        SceneController.instance.CloseMinigame(miniGameName);
        panel.SetActive(false);
    }
}
