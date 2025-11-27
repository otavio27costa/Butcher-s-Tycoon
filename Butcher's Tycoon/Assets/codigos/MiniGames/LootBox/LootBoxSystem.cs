using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootBoxSystem : MonoBehaviour
{
    public static LootReward GetRewardByScore(Lootbox lootbox, int score)
    {
        float chanceCommom = 70f;
        float chanceRare = 20f;
        float chanceEpic = 8f;
        float chancelegendary = 2f;

        if (score > 50)
        {
            chanceRare += 10;
            chanceEpic -= 10;
        }

        if (score > 150)
        {
            chanceEpic += 5;
            chanceCommom -= 10;
        }

        if (score > 300)
        {
            chancelegendary += 15;
            chanceCommom -= 20;
        }

        float total = chanceCommom + chanceRare + chanceEpic + chancelegendary;
        float roll = Random.Range(0, total);

        int selectTier = 0;

        if (roll < chanceCommom) selectTier = 0;
        else if (roll < chanceCommom + chanceRare) selectTier = 1;
        else if (roll < chanceCommom + chanceRare + chanceEpic) selectTier = 2;
        else selectTier = 3;

        var tierRewards = lootbox.rewards.Where(r => r.tier == selectTier).ToList();

        return tierRewards[Random.Range(0, tierRewards.Count)];
    }  
}
