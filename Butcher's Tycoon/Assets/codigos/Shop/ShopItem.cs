using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Shop Item", menuName = "Shop/Shop Item")]

public class ShopItem : ScriptableObject
{
    public string itemName;
    public int itemPrice;
    [SerializeField] public GameObject itemPreFab;
    public Sprite icon;
    public string itemID;
    public int maxItem;
    public int currentQnt;
}
