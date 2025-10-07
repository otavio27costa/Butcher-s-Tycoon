using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    private Item placedItem;

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }



    public bool isFree()
    {
        return placedItem == null;
    }
    
    public void SetItem(Item item)
    {
        placedItem = item;
    }
    
    public void CleatItem()
    {
        placedItem = null;
    }

    public override string ToString()
    {
        return base.ToString();
    }

}