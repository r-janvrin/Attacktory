using Unity.VisualScripting;
using UnityEngine;


public class buildingGrid : MonoBehaviour
{
    public GameObject testingObject;
    public TileDataManager tileManager;
    private GameObject[,] buildingArray;
    public GameObject[,] prefabArray; //first index is type of building (wall, gun, etc). second index is number (which wall, gun, etc)
    public short xSize;
    public short ySize;
    void Awake()//awake is called before start
    {
        buildingArray = new GameObject[xSize, ySize]; //each building starts as null
        tileManager = GameObject.Find("TileDataManager").GetComponent<TileDataManager>();
    }

    private void Start() //start is called on creation, but after awake
    {
        //testing call
        createBuilding(new Vector2Int(304, 295), 2, 0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void createBuilding(Vector2Int position, byte size, byte buildingType, byte buildingID)
    {
        //create the game object & offset it by part of its size to centre the image
        GameObject tempObject = GameObject.Instantiate(getPrefab(buildingType, buildingID), new Vector3((float)position.x + size * 0.5f, (float)position.y, 0), Quaternion.identity);
        for(int i = 0; i < size; i++)
        {
            for(int j=0; j<size; j++)
            {
                buildingArray[position.x + i, position.y + j] = tempObject;
            }
        }
    }

    private GameObject getPrefab(byte buildingType, byte buildingID)
    {
        Debug.Log("TODO: make getprefab get correct prefab");
        return testingObject;
    }

    public float getDrillSpeed(byte numTiles, byte buildingID, sbyte resourceType)
    {
        Debug.Log("TODO: create variable drill speeds");
        return 1f;
    }
    void testFunction()
    {
        Debug.Log("Function called successfully!");
    }

}

