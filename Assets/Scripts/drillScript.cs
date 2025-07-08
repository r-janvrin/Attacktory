using UnityEngine;

public class drillScript : MonoBehaviour
{
    public storageScript myStorage;
    public sbyte resourceType;
    public sbyte myMaxTier;
    private byte buildingType=3;//whether building is drill, wall, gun, etc
    public byte buildingID; //which drill, wall, gun, this building is

    public float maxProgress;
    public float currentProgress=0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myStorage = transform.gameObject.GetComponent<storageScript>();
        byte numTiles;
        //get which resource im producting
        resourceType = myStorage.grid.tileManager.getResourceType(myStorage.position, myStorage.buildingSize, myMaxTier, out numTiles);
        //determine how much time must pass before 1 
        maxProgress = myStorage.grid.getDrillSpeed(numTiles, buildingID, resourceType);

    }

    // Update is called once per frame
    void Update()
    {
        currentProgress += Time.deltaTime;

        if (currentProgress > maxProgress)//add resource to storage
        {
            currentProgress = maxProgress; //ensure no overflow
            if (myStorage.addResource(resourceType)) currentProgress = 0;
        }
    }
}
