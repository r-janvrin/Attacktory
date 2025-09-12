using Unity.VisualScripting;
using UnityEngine;


public class buildingGrid : MonoBehaviour
{
    [HideInInspector] public static buildingGrid grid;
    private int NUM_BUILDING_TYPES;
    public GameObject testingObject;
    public GameObject conveyorObject;
    public GameObject homeBaseObject;

    [HideInInspector] public TileDataManager tileManager;
    private baseBuildingScript[,] buildingArray;
    [SerializeField] private SpriteReference resourceSprites;
    public GameObject baseConveyorResource;

    public short xSize;
    public short ySize;
    void Awake()//awake is called before start
    {
        grid = this;
        buildingArray = new baseBuildingScript[xSize, ySize]; //each building starts as null
        tileManager = GameObject.Find("TileDataManager").GetComponent<TileDataManager>();
        
    }

    private void Start() //start is called on creation, but after awake
    {
        //testing call - position, size, which building type, which specific building
        createBuilding(new Vector2Int(306, 294), testingObject, Quaternion.identity);
        //createBuilding(new Vector2Int(304, 292), testingObject);
        //createBuilding(new Vector2Int(306, 291), testingObject);
        //for (int i = 0; i < 10; i++)
        //{
        //    createBuilding(new Vector2Int(306 + i, 293), conveyorObject);
        //}
        //createBuilding(new Vector2Int(316, 293), homeBaseObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void createBuilding(Vector2Int position, GameObject prefab, Quaternion rotation)
    {
        //create the game object & offset it by part of its size to centre the image
        byte size = prefab.GetComponent<baseBuildingScript>().buildingSize;
        GameObject tempObject = GameObject.Instantiate(prefab, new Vector3((float)position.x + size*0.5f, (float)position.y + size*0.5f, 0), rotation);

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

    public bool addToConveyor(Vector2Int position, conveyorResourceController conveyorResource, Vector2Int direction)
    {
        if (buildingArray[position.x, position.y] == null)
        {
            //Debug.Log("should not be accessing " + position);
            return false;
        }
        //if (buildingArray[position.x, position.y] is not conveyorScript) return false;
        return buildingArray[position.x, position.y].addFromConveyor(conveyorResource, direction);

    }

    public GameObject getConveyorResource(sbyte resourceType)
    {
       //Debug.Log("GET CONVEYOR RESOURCE FIX!!!!");
        return baseConveyorResource;
    }

    public Sprite getResourceSprite(sbyte resource)
    {
        if (resource >= resourceSprites.sprites.Length) return resourceSprites.sprites[0];
        return resourceSprites.sprites[resource];
    }

    public bool tilesAreEmpty(Vector2Int BL, byte size)
    {
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                if (buildingArray[BL.x + i, BL.y + j] != null) return false;
            }
        }
        return true;
    }
}

