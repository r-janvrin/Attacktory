using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class TileDataManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public InputActionReference click;
    [SerializeField] private Tilemap environmentMap;

    [SerializeField] private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTiles;

    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach(var tileData in tileDatas)
        {
            foreach(var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    private void OnEnable()
    {
        click.action.started += Click;
        click.action.Enable();
    }

    private void OnDisable()
    {
        click.action.started -= Click;
        click.action.Disable();
    }

    private void Click(InputAction.CallbackContext obj)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3Int gridPosition = environmentMap.WorldToCell(mousePosition);

        TileBase clickedTile = environmentMap.GetTile(gridPosition);

        byte tyleType = dataFromTiles[clickedTile].tileType;
        Debug.Log("Tile Type:" + tyleType);
    }
}
