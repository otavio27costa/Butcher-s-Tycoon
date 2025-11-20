using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase instance;
    public List<ShopItem> allItems;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public Item GetItemPrefab(string id)
    {
        foreach (var item in allItems)
        {
            Debug.Log("Item no banco: " + item.itemID);
            if (item.itemID == id)
            {
                // retorna o componente Item do prefab
                return item.itemPreFab.GetComponent<Item>();
            }
        }

        Debug.LogError("[ItemDataBase] Nenhum item encontrado com ID: " + id);
        return null;
    }
}
