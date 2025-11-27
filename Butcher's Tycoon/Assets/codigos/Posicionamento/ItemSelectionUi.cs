using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectionUI : MonoBehaviour
{
    public static ItemSelectionUI Instance;

    [Header("UI Elements")]
    public GameObject panel;
    public Button moveButton;
    public Button sellButton;

    private Item currentItem;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Open(Item item)
    {
        currentItem = item;
        panel.SetActive(true);
        panel.transform.position = Camera.main.WorldToScreenPoint(item.transform.position);
    }

    public void Close()
    {
        panel.SetActive(false);
        currentItem = null;
    }

    public void OnMovePressed()
    {
        if (currentItem == null) return;

        GridManager.Instance.SelectPlacedItem(currentItem);
        Close();
    }

    public void OnSellPressed()
    {
        if (currentItem == null) return;

        GridManager.Instance.SellSelectedItem();
        Close();
    }
}
