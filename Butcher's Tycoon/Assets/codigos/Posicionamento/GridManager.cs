using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform _cam;
    [Header("Itens")]

    private GameObject itemToPlacePrefab;
    private GameObject previewItem;
    private Item itemToPlace;
    private int selectedItemPrice;

    private Dictionary<Vector2, Tile> _tiles;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateGrid();
    }

    void Update()
    {
            
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelSelection();
            return;
        }

        if (itemToPlace == null) return;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(mouseWorld.x), Mathf.RoundToInt(mouseWorld.y));

        Tile hoveredTile = GetTileAtPosition(gridPos);

        if (hoveredTile== null)
        {
            if(previewItem != null)
            {
                Destroy(previewItem);
                previewItem = null;
            }
            return;
        }

        bool canPlace;
        List<Tile> tilesToOccupy = CheckTilesForItem(gridPos, itemToPlace.width, itemToPlace.height, out canPlace);


       

        if (previewItem == null)
        {
            previewItem = Instantiate(itemToPlace.gameObject);

            var cols = previewItem.GetComponentsInChildren<Collider2D>();
            foreach (var c in cols) c.enabled = false;
        }

        Vector3 basePos = hoveredTile.transform.position;
        Vector3 offset = new Vector3((itemToPlace.width - 1) * 0.5f, (itemToPlace.height - 1) * 0.5f, 0f);
        previewItem.transform.position = basePos + offset;

        SetPreviewColor(previewItem, canPlace ? new Color(0.6f, 1f, 0.6f, 0.5f) : new Color(1f, 0.5f, 0.5f, 0.5f));

        if (Input.GetMouseButtonDown(0))
        {
            if (canPlace)
                {
                    PlaceItemAt(gridPos, tilesToOccupy);
                }
            else
            {
                Debug.Log("[GridManager] Espaço ocupado ou fora do grid - não é possível posicionar.");
            }
        }

    }

    public void CancelSelection()
    {
        if(itemToPlace != null)
        {
            PlayerData.instance.AddMoney(selectedItemPrice);
        }

        if(previewItem != null)
        {
            Destroy(previewItem.gameObject);
            previewItem = null;
        }
        itemToPlace = null;
    }



    private void SetPreviewColor(GameObject go, Color color)
    {
        var sprites = go.GetComponentsInChildren <SpriteRenderer>();
        foreach (var sr in sprites)
        {
            Color c = sr.color;
            sr.color = new Color(color.r, color.g, color.b, color.a);
        }
    }

    public void PlaceItemAt(Vector2Int gridPos, List<Tile> tilesToOccupy)
    {
        if(tilesToOccupy == null || tilesToOccupy.Count == 0) return;

        Tile baseTile = GetTileAtPosition(gridPos);
        Vector3 offset = new Vector3((itemToPlace.width - 1) * 0.5f, (itemToPlace.height - 1) * 0.5f, 0f);
        Vector3 worldPos = baseTile.transform.position + offset;

        GameObject placedGO = Instantiate(itemToPlace.gameObject, worldPos, Quaternion.identity);


        Item placedItem = placedGO.GetComponent<Item>();

        if(placedItem == null)
        {
            Destroy(placedGO);
            return;
        }

        foreach (Tile t in tilesToOccupy)
        {
            t.SetItem(placedItem);
        }

        if(placedGO != null)
        {
            Destroy(previewItem);
            previewItem = null;
        }
        itemToPlace = null;
        itemToPlacePrefab = null;
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);


                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }



    public void SelectItem(Item itemPreFab, int price)
    {
        itemToPlace = itemPreFab;
        selectedItemPrice = price;
    }

    private List<Tile> CheckTilesForItem(Vector2Int baseGridPos, int w, int h, out bool canPlace)
    {
        List<Tile> list = new List<Tile>();
        canPlace = true;

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Vector2Int check = new Vector2Int(baseGridPos.x + x, baseGridPos.y + y);
                Tile t = GetTileAtPosition(check);
                if(t == null || !t.isFree())
                {
                    canPlace = false;
                    return list;
                }
                list.Add(t);
            }
        }
        return list;
    }

}