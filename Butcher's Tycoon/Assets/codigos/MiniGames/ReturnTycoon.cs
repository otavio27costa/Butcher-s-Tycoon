using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnTycoon : MonoBehaviour
{



    void ReloadPlacedItems()
    {
        foreach(PlacedItemData data in BuildSaveSystem.instance.PlacedItems)
        {
            GameObject obj = Instantiate(ItemDataBase.instance.GetItemPrefab(data.itemID)).gameObject;
            obj.transform.position = data.position;
            obj.transform.rotation = data.rotation;
        }
    }

    public void LootBox()
    {
        SceneManager.LoadScene("LootBox");
    }
    public void ReturnToTycoon()
    {
        SceneManager.LoadScene("Tycoon");
    }
}
