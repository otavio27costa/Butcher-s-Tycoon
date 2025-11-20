using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildSaveSystem : MonoBehaviour
{
    public static BuildSaveSystem instance;

    public List<PlacedItemData> PlacedItems = new List<PlacedItemData>();

    public GridManager gridManager;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // assina o evento de mudança de cena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SaveItem(string itemID, Vector3 pos, Quaternion rot)
    {
        PlacedItemData data = new PlacedItemData();
        data.itemID = itemID;
        data.position = pos;
        data.rotation = rot;

        PlacedItems.Add(data);
    }

    public void LoadPlacedItems()
    {
        if (ItemDataBase.instance == null) return;
        if (PlacedItems.Count == 0)
        {
            Debug.Log("[BuildSaveSystem] Nenhum item salvo para carregar.");
            return;
        }

        if (gridManager == null)
        {
            Debug.LogError("[BuildSaveSystem] gridManager é null ao tentar carregar itens!");
            return;
        }

        Debug.Log("[BuildSaveSystem] Carregando " + PlacedItems.Count + " itens...");

        foreach (var data in PlacedItems)
        {
            // 1. Encontra o prefab correspondente
            GameObject prefabGO = ItemDataBase.instance.GetItemPrefab(data.itemID).gameObject;
            Item prefab = prefabGO.GetComponent<Item>();

            if (prefab.gameObject == null)
            {
                Debug.LogWarning("[BuildSaveSystem] Não encontrei prefab com itemID: " + data.itemID);
                continue;
            }

            // 2. Cria o item no mundo
            GameObject obj = GameObject.Instantiate(prefab.gameObject, data.position, data.rotation);
            Item item = obj.GetComponent<Item>();

            if (item == null)
            {
                Debug.LogError("[BuildSaveSystem] Prefab não possui componente Item!");
                continue;
            }

            // 3. Ocupa os tiles corretos
            Vector2Int basePos = new Vector2Int(Mathf.RoundToInt(data.position.x), Mathf.RoundToInt(data.position.y));

            List<Tile> tiles = gridManager.CheckTilesForItem(basePos, item.width, item.height, out bool canPlace);

            if (!canPlace)
            {
                Debug.LogWarning("[BuildSaveSystem] Tiles ocupados ao tentar restaurar item " + data.itemID);
                continue;
            }

            foreach (Tile t in tiles)
                t.SetItem(item);

            Debug.Log("Carregando item com ID: " + data.itemID);
        }

        Debug.Log("[BuildSaveSystem] Itens restaurados!");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (ItemDataBase.instance == null)
        {
            ItemDataBase.instance = FindObjectOfType<ItemDataBase>();
        }
        // procura grid novo
        gridManager = FindObjectOfType<GridManager>();

        if (gridManager == null)
        {
            Debug.LogWarning("[BuildSaveSystem] Nenhum GridManager encontrado na cena " + scene.name);
            return;
        }

        Debug.Log("[BuildSaveSystem] GridManager encontrado na cena " + scene.name);

        // carrega itens
        LoadPlacedItems();
    }

}
