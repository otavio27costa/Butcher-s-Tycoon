using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;
    private ShopItem currentItem;

    public void Setup(ShopItem item)
    {
        currentItem = item;
        //iconImage.sprite = item.icon;
        nameText.text = item.name;
        priceText.text = item.itemPrice.ToString() + " $";

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    public void OnBuyButtonClicked()
    {
        if(currentItem == null) return;

        if(PlayerData.instance.money >= currentItem.itemPrice)
        {
            PlayerData.instance.AddMoney(-currentItem.itemPrice);
            GridManager.Instance.SelectItem(currentItem.itemPreFab, currentItem.itemPrice);
        }
    }
}
