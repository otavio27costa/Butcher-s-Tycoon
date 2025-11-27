using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{
    public Item item;

    [Header("Grid Size")]
    public int width = 1;
    public int height = 1;

    [Header("Economia")]
    public int sellValue = 50;

    private void Awake()
    {
        
        item = GetComponent<Item>();
    }

    void OnMouseDown()
    {
        if (GridManager.Instance.IsPlacingNewItem()) return;

        ItemSelectionUI.Instance.Open(item);
    }
}
