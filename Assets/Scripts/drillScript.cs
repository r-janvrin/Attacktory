using UnityEngine;

public class drillScript : baseBuildingScript
{
    [HideInInspector] public buildingGrid grid; // the 2d grid
    [HideInInspector] public Vector2Int position; //bottom left corner of where this is located
    public byte buildingSize;
    public byte maxStorage;
    [SerializeField] DrillSpeedData speedData;

    //private storageScript myStorage;
    public ResourceType myMaxTier;
    private byte buildingID; //which drill, wall, gun, this building is

    private float maxProgress;
    private float currentProgress=0;
    private resourcePair output;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = GameObject.Find("BuildingManager").GetComponent<buildingGrid>(); //get reference to building grid
        position = grid.tileManager.getVector2Int(transform.position);
        position = position - new Vector2Int(buildingSize / 2, buildingSize / 2);//set position to bottom left corner

        byte numTiles;
        //get which resource im producing and set how many to 0
        output = new resourcePair(grid.tileManager.getResourceType(position, buildingSize, myMaxTier, out numTiles), 0);
        //determine how much time must pass before adding a resource
        maxProgress = getDrillSpeed(output.resourceType, numTiles);

    }

    // Update is called once per frame
    void Update()
    {

        currentProgress += Time.deltaTime;

        if (currentProgress > maxProgress)//add resource to storage
        {
            if (addToOutput()) currentProgress -= maxProgress;
        }
        currentProgress = Mathf.Min(currentProgress, maxProgress);//cap progress at max

    }

    //what happens when ANOTHER building tries adding to this
    public override bool AddResource(sbyte resourceType)
    {
        return false; //drills do not accept input
    }

    //tries to add resource to output, if full return false
    bool addToOutput()
    {
        if (output.quantity >= maxStorage) return false;
        output.quantity++;
        return true;
    }

    //get how long before 1 of this type is produced
    float getDrillSpeed(sbyte type, byte tilecount)
    {
        if (type < 0 || type >= speedData.speed.Length) return 0f;
        //use reciprocal to get seconds/resource instead of resource/second
        return 1 / (speedData.speed[type] * tilecount);
    }
}
