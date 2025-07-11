using UnityEngine;

public class storageScript : MonoBehaviour
{
    [HideInInspector] public buildingGrid grid; //reference to the grid
    public Vector2Int position; //square that this is on (bottom left)

    public byte buildingSize;//1x1, 2x2, etc
    public byte buildingType;//whether building is drill, wall, gun, etc
    public byte maxStorage; //how many of each resource type can be stored

    /*[HideInInspector]*/  public sbyte[] storage;//how much each resource is stored: -1 means not accepted

    private void Awake()
    {
        grid = GameObject.Find("BuildingManager").GetComponent<buildingGrid>(); //get reference to building grid
        position = grid.tileManager.getVector2(transform.position);
        position = position - new Vector2Int(buildingSize / 2, buildingSize / 2);//set position to bottom left corner
    }

    private void Start()
    {
        storage = new sbyte[20];
    }

    //tries to add the resource, returns false if it can't, true if it does
    public bool addResource(sbyte resourceType)
    {

        if (resourceType < 0) return false;

        if (storage[resourceType] < 0) return false;

        if (storage[resourceType] >= maxStorage) return false;

        storage[resourceType]++;
        return true;
    }
}
