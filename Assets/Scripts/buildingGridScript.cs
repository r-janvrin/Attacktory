using Unity.VisualScripting;
using UnityEngine;


public class buildingGrid : MonoBehaviour
{
    private int NUM_BUILDING_TYPES;
    public GameObject testingObject;
    public TileDataManager tileManager;
    private baseBuildingScript[,] buildingArray;
    public PrefabReference[] prefabArray; //first index is type of building (wall, gun, etc). second index is number (which wall, gun, etc)
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
        createBuilding(new Vector2Int(304, 294), 2, 3, 0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void createBuilding(Vector2Int position, byte size, byte buildingType, byte buildingID)
    {
        //create the game object & offset it by part of its size to centre the image
        GameObject tempObject = GameObject.Instantiate(getPrefab(buildingType, buildingID), new Vector3((float)position.x + size*0.5f, (float)position.y + size*0.5f, 0), Quaternion.identity);
        buildingArray[position.x, position.y] = tempObject.GetComponent<baseBuildingScript>();
        Debug.Log("got: " + tempObject);
        for (int i = 0; i < size; i++) //store the reference to it in all blocks it is in
        {
            for(int j=0; j<size; j++)
            {
                buildingArray[position.x + i, position.y + j] = buildingArray[position.x, position.y];
            }
        }
    }

    private GameObject getPrefab(byte buildingType, byte buildingID)
    {
        if(buildingType >= NUM_BUILDING_TYPES || buildingType < 0) //make sure it's not out of bounds 
        {
            return testingObject;
        }
        return prefabArray[buildingType].prefabList[buildingType];
    }
}

