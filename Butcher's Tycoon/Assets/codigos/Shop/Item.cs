using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{
    public GridManager manager;
    public Item item;


    [Header("Grid Size")]
    public int width = 1;
    public int height = 1;

    [Header("Economia")]
    public int sellValue = 50;

    public ShopItem info;
    public int maxQnt;
    public int currentQnt;

    private void Awake()
    {
        manager = FindAnyObjectByType<GridManager>();
        item = GetComponent<Item>();

        currentQnt = 0;
        sellValue = info.itemPrice;
    }

    void OnMouseDown()
    {
        manager.selectedPlacedItem = this;
        if (GridManager.Instance.IsPlacingNewItem()) return;

        ItemSelectionUI.Instance.Open(item);
    }
}
