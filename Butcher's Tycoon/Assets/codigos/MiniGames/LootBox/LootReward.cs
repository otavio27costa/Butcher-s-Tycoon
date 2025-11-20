using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LootReward", menuName = "Lootbox/New Reward")]
public class LootReward : ScriptableObject
{
    public string rewardName;
    public Sprite icon;
    public int tier;

    [Range(1,100)]
    public int weight = 10;

    public int moneyAmount;
    public Item itemReward;
}
