using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootBox", menuName = "Lootbox/Lootbox")]
public class Lootbox : ScriptableObject
{
    public LootReward[] rewards;
}
