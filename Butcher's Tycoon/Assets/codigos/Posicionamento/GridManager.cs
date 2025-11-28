using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [Header("Grid")]
    public int width;
    public int height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;

    [Header("Item System")]
    public Item itemToPlace;               // item para colocar (novo OU reposicionado)
    private GameObject previewItem;         // preview visual
    private int selectedItemPrice;

    // Item já colocado que está sendo reposicionado
    public Item selectedPlacedItem = null;
    private List<Tile> selectedItemTiles = new List<Tile>();

    private Dictionary<Vector2, Tile> _tiles;

    private void Awake()
    {
        Instance = this;
    }
    public bool IsPlacingNewItem()
    {
        return itemToPlace != null;
    }
    private void Start()
    {
        GenerateGrid();
        Time.timeScale = 1f;
    }

    private void Update()
    {
        HandlePlacementUpdate();
    }

    // ========== SISTEMA DE SELEÇÃO DE ITENS (LOJA) ==========
    public void SelectItem(Item prefabItem, int price)
    {
        CancelSelection();

        // Guardar o prefab real (SEM instanciar ainda)
        itemToPlace = prefabItem;
        selectedItemPrice = price;

        // Criar preview SEPARADO com cor alterada
        previewItem = Instantiate(prefabItem.gameObject);
        DisableColliders(previewItem);
        SetPreviewColor(previewItem, new Color(1, 1, 1, 0.5f));
    }

    // ========== SISTEMA DE REPOSICIONAR ITEM ==========
    public void SelectPlacedItem(Item placed)
    {
        if (itemToPlace != null) return; // impedindo conflito com loja

        selectedPlacedItem = placed;
        selectedItemTiles.Clear();

        // pega todos os tiles ocupados
        foreach (var t in _tiles.Values)
            if (t.placedItem == placed)
                selectedItemTiles.Add(t);

        // libera tiles
        foreach (var t in selectedItemTiles)
            t.ClearItem();

        // cria o preview
        previewItem = Instantiate(placed.gameObject);
        DisableColliders(previewItem);
        SetPreviewColor(previewItem, new Color(1, 1, 1, 0.5f));

        // esconde o item real
        placed.gameObject.SetActive(false);

        itemToPlace = placed;
        ItemSelectionUI.Instance.Close();
    }

    // ========== VENDA ==========
    public void SellSelectedItem()
    {
        // 1) validações iniciais
        if (selectedPlacedItem == null)
        {
            Debug.LogWarning("[GridManager] SellSelectedItem chamado mas nenhum item está selecionado.");
            return;
        }

        // 2) limpar tiles ocupados (se houver)
        if (selectedItemTiles != null && selectedItemTiles.Count > 0)
        {
            foreach (var t in selectedItemTiles)
            {
                if (t != null)
                    t.ClearItem();
            }
        }
        else
        {
            Debug.LogWarning("[GridManager] SellSelectedItem: selectedItemTiles vazio ou nulo. Talvez o item não tenha sido corretamente registrado nos tiles.");
        }

        // 3) devolver dinheiro (verifica PlayerData)
        if (PlayerData.instance != null)
        {
            PlayerData.instance.AddMoney(selectedPlacedItem.sellValue);
            Debug.Log("[GridManager] Dinheiro adicionado: " + selectedPlacedItem.sellValue + ". Saldo agora: " + PlayerData.instance.money);
        }
        else
        {
            Debug.LogError("[GridManager] PlayerData.instance é null! Não foi possível devolver dinheiro.");
        }

        // 5) destruir o objeto do mundo
        GameObject toDestroy = selectedPlacedItem.gameObject;
        selectedPlacedItem.currentQnt--;
        selectedPlacedItem = null; // limpa a referência antes de destruir para evitar usos posteriores
        Destroy(toDestroy);
        Debug.Log("[GridManager] Item destruído.");

        // 6) limpar estado interno e UI
        selectedItemTiles.Clear();
        CancelSelection(); // garante que preview e seleção fiquem limpos

        if (ItemSelectionUI.Instance != null)
            ItemSelectionUI.Instance.Close();

        // 7) remover qualquer destaque residual
        // (assumindo que HighlightItem(item, bool) exista)
        // HighlightItem(selectedPlacedItem, false); -> não pode chamar depois de null

        Debug.Log("[GridManager] Venda concluída.");
    }

    // ========== CANCELAR ==========
    public void CancelSelection()
    {
        if (selectedPlacedItem != null)
        {
            // volta o item para os tiles antigos
            foreach (var t in selectedItemTiles)
                t.SetItem(selectedPlacedItem);

            selectedPlacedItem.gameObject.SetActive(true);
            selectedPlacedItem = null;
            selectedItemTiles.Clear();
        }

        if (previewItem != null)
            Destroy(previewItem);

        previewItem = null;
        itemToPlace = null;
        selectedPlacedItem = null;
        ItemSelectionUI.Instance.Close();
    }

    // ========== SISTEMA DE POSICIONAMENTO ==========
    private void HandlePlacementUpdate()
    {
        if (itemToPlace == null) return;

        // ESC cancela
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelSelection();
            return;
        }

        // calcula tile atual
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;

        Vector2Int gridPos = new Vector2Int(
            Mathf.RoundToInt(mouseWorld.x),
            Mathf.RoundToInt(mouseWorld.y)
        );

        Tile hoveredTile = GetTileAtPosition(gridPos);

        if (hoveredTile == null)
        {
            previewItem.SetActive(false);
            return;
        }

        previewItem.SetActive(true);

        bool canPlace;
        List<Tile> tiles = CheckTilesForItem(
            gridPos,
            itemToPlace.width,
            itemToPlace.height,
            out canPlace
        );

        // move preview
        MovePreview(hoveredTile, itemToPlace);

        // cor
        SetPreviewColor(previewItem, canPlace ?
            new Color(0.5f, 1f, 0.5f, 0.6f) :
            new Color(1f, 0.5f, 0.5f, 0.6f));

        // clicar coloca
        if (Input.GetMouseButtonDown(0) && itemToPlace.currentQnt < itemToPlace.maxQnt)
        {
            if (canPlace)
            {
                FinalizePlacement(gridPos, tiles);
                itemToPlace.currentQnt++;
            }
        }
    }

    private void FinalizePlacement(Vector2Int gridPos, List<Tile> tiles)
    {
        Tile baseTile = GetTileAtPosition(gridPos);

        Vector3 offset = new Vector3(
            (itemToPlace.width - 1) * 0.5f,
            (itemToPlace.height - 1) * 0.5f,
            0
        );

        Vector3 pos = baseTile.transform.position + offset;

        GameObject finalObj;

        // caso for reposicionamento → reaproveita item real
        if (selectedPlacedItem != null)
        {
            selectedPlacedItem.transform.position = pos;

            foreach (var t in tiles)
                t.SetItem(selectedPlacedItem);

            selectedPlacedItem.gameObject.SetActive(true);
            selectedPlacedItem = null;
        }
        else
        {
            // posição de item novo
            finalObj = Instantiate(itemToPlace.gameObject, pos, Quaternion.identity);
            SetPreviewColor(finalObj, Color.white);

            Item realItem = finalObj.GetComponent<Item>();

            foreach (var t in tiles)
                t.SetItem(realItem);
        }

        // limpa
        Destroy(previewItem);
        previewItem = null;
        itemToPlace = null;
        selectedItemTiles.Clear();
    }

    // ========== PREVIEW ==========
    void PreparePreview(GameObject obj)
    {
        previewItem = obj;
        DisableColliders(previewItem);
        SetPreviewColor(previewItem, new Color(1, 1, 1, 0.5f));
    }

    void MovePreview(Tile tile, Item item)
    {
        Vector3 offset = new Vector3(
            (item.width - 1) * 0.5f,
            (item.height - 1) * 0.5f,
            0
        );

        previewItem.transform.position = tile.transform.position + offset;
    }

    void DisableColliders(GameObject go)
    {
        foreach (var c in go.GetComponentsInChildren<Collider2D>())
            c.enabled = false;
    }

    void SetPreviewColor(GameObject go, Color c)
    {
        foreach (var sr in go.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = c;
        }
    }

    // ========== GRID ==========
    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                var t = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                t.name = $"Tile {x} {y}";
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0); 
                t.Init(isOffset);
                _tiles[new Vector2(x, y)] = t;
            }

        _cam.position = new Vector3(width / 2f, height / 2f, -10);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        _tiles.TryGetValue(pos, out Tile t);
        return t;
    }

    public List<Tile> CheckTilesForItem(Vector2Int basePos, int w, int h, out bool canPlace)
    {
        List<Tile> list = new List<Tile>();
        canPlace = true;

        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
            {
                Tile t = GetTileAtPosition(new Vector2(basePos.x + x, basePos.y + y));

                if (t == null || !t.isFree())
                {
                    canPlace = false;
                    return list;
                }

                list.Add(t);
            }

        return list;
    }
}