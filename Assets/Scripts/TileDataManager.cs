using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class TileDataManager : MonoBehaviour
{
    [HideInInspector] public static TileDataManager manager;

    public InputActionReference checkTile;
    [SerializeField] private Tilemap environmentMap;
    private int i, j;
    private TileBase currentTile;

    [SerializeField] private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTiles;

    private void Awake()
    {
        manager = this;
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
        checkTile.action.started += CheckTile;
        checkTile.action.Enable();
    }

    private void OnDisable()
    {
        checkTile.action.started -= CheckTile;
        checkTile.action.Disable();
    }

    private void CheckTile(InputAction.CallbackContext obj)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3Int gridPosition = environmentMap.WorldToCell(mousePosition);

        TileBase clickedTile = environmentMap.GetTile(gridPosition);

        if (!dataFromTiles.ContainsKey(clickedTile))
        {
            Debug.Log("Please choose a tile!");
            return;
        }
        sbyte tyleType = (sbyte)dataFromTiles[clickedTile].tileType;
        Debug.Log("Tile Type:" + tyleType + "   Position: " + gridPosition);
    }

    //maxTier is the highest tier the drill can mine (i.e. making sure not to give the basic drill aluminum)
    public sbyte getResourceType(Vector2Int position, byte size, ResourceType maxTier, out byte numTiles)//gets the id # of the resource the drill will produce
    {
        Debug.Log("position: "+position + "size" + size);
        ResourceType resourceType = ResourceType.empty;
        numTiles = 0;
        for(int i = 0; i < size; i++)//check each tile under it to see if it is the highest tier
        {
            for(int j = 0; j < size; j++)
            {
                TileBase currentTile = environmentMap.GetTile(new Vector3Int(position.x + i, position.y + j, 0));
                //Debug.Log("testing tile position: " + (position.x + i) + " " + (position.y + j));
                //get the highest resource ID
                if(dataFromTiles[currentTile].tileType > resourceType && dataFromTiles[currentTile].tileType <= maxTier)
                {
                    resourceType = dataFromTiles[currentTile].tileType;
                    //Debug.Log(dataFromTiles[currentTile].tileType);
                    numTiles = 0; //reset the count of tiles if we change type
                }
                //count how many tiles of that type there are
                if (dataFromTiles[currentTile].tileType == resourceType)
                {
                    numTiles++;
                }
            }
        }

        return (sbyte)resourceType;
    }


    public Vector2Int getVector2Int(Vector2 position)
    {
        Vector3Int gridPosition = environmentMap.WorldToCell(new Vector3(position.x, position.y, 0));
        return new Vector2Int(gridPosition.x, gridPosition.y);
    }

    public bool tilesAreValid(Vector2Int BL, byte size)
    {
        for (i = 0; i < size; i++)
        {
            for(j=0; j<size; j++)
            {
                //check to see if the tile is null or a wall
                currentTile = environmentMap.GetTile(new Vector3Int(BL.x + i, BL.y + j, 0));
                if (currentTile == null || dataFromTiles[currentTile].tileType == ResourceType.wall) return false;
            }
        }
        return true;
    }
}
