using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopItem[] itens;
    [SerializeField] private GameObject shopButtonPrefab;
    [SerializeField] private Transform buttonContainer;

    private void Start()
    {
        GenerateShop();
    }

    private void GenerateShop()
    {
        foreach (ShopItem item in itens)
        {
            GameObject buttonGO = Instantiate(shopButtonPrefab, buttonContainer);
            ShopButton button = buttonGO.GetComponent<ShopButton>();

            if(button != null)
            {
                button.Setup(item);
            }
        }
    }
}
