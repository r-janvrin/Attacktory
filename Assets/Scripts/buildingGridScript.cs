using Unity.VisualScripting;
using UnityEngine;


public class buildingGrid : MonoBehaviour
{
    private int NUM_BUILDING_TYPES;
    public GameObject testingObject;
    public TileDataManager tileManager;
    private baseBuildingScript[,] buildingArray;

    public short xSize;
    public short ySize;
    void Awake()//awake is called before start
    {
        buildingArray = new baseBuildingScript[xSize, ySize]; //each building starts as null
        tileManager = GameObject.Find("TileDataManager").GetComponent<TileDataManager>();
        
    }

    private void Start() //start is called on creation, but after awake
    {
        //testing call - position, size, which building type, which specific building
        createBuilding(new Vector2Int(304, 294), testingObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void createBuilding(Vector2Int position, GameObject prefab)
    {
        //create the game object & offset it by part of its size to centre the image
        byte size = prefab.GetComponent<baseBuildingScript>().buildingSize;
        GameObject tempObject = GameObject.Instantiate(prefab, new Vector3((float)position.x + size*0.5f, (float)position.y + size*0.5f, 0), Quaternion.identity);

        buildingArray[position.x, position.y] = tempObject.GetComponent<baseBuildingScript>();
        buildingArray[position.x, position.y].setupResources(position);

        for (int i = 0; i < size; i++) //store the reference to it in all blocks it is in
        {
            for(int j=0; j<size; j++)
            {
                buildingArray[position.x + i, position.y + j] = buildingArray[position.x, position.y];
            }
        }
    }

    public bool addToPosition(Vector2Int position, sbyte resourceType, Vector2Int direction)
    {
        if (buildingArray[position.x, position.y] == null) return false;
        return buildingArray[position.x, position.y].AddResource(resourceType, direction);
    }
}

