using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [SerializeField] private Item itemPreFab;

    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        if (itemPreFab == null)
        {
            Debug.LogError($"[ShopButton] itemPreFab n�o atribu�do no bot�o '{name}'.");
            return;
        }

        if (GridManager.Instance == null)
        {
            Debug.LogError("[ShopButton] GridManager.Instance � null. Verifique se h� um GridManager ativo na cena.");
            return;
        }

        GridManager.Instance.SelectItem(itemPreFab);
        Debug.Log($"[ShopButton] Selecionado prefab: {itemPreFab.itemName}");
    }
}
