using UnityEngine;

public class storageScript : MonoBehaviour
{
    [HideInInspector] public buildingGrid grid; //reference to the grid
    [HideInInspector] public Vector2Int position; //square that this is on (bottom left)

    public byte buildingSize;//1x1, 2x2, etc
    public BuildingType _buildingType;//whether building is drill, wall, gun, etc
    public byte maxStorage; //how many of each resource type can be stored

    [HideInInspector]  public sbyte[] storage;//how much each resource is stored: -1 means not accepted

    private void Awake()
    {

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

        //at this point we know it can be stored - what do we want to do with it?

        storage[resourceType]++;
        return true;
    }
}

public struct resourcePair
{
    public resourcePair(sbyte whichResource, byte howMany)
    {
        resourceType = whichResource;
        quantity = howMany;
    }
    public sbyte resourceType;
    public byte quantity;
}
