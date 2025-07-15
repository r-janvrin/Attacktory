using UnityEngine;

public class drillScript : MonoBehaviour
{
    private storageScript myStorage;
    public ResourceType myMaxTier;
    private byte buildingID; //which drill, wall, gun, this building is

    private float maxProgress;
    private float currentProgress=0;
    private resourcePair output;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myStorage = transform.gameObject.GetComponent<storageScript>();
        byte numTiles;
        //get which resource im producing and set how many to 0
        output = new resourcePair(myStorage.grid.tileManager.getResourceType(myStorage.position, myStorage.buildingSize, myMaxTier, out numTiles), 0);
        //determine how much time must pass before adding a resource
        maxProgress = myStorage.grid.getDrillSpeed(numTiles, buildingID, output.resourceType);

        for(int i=0; i<myStorage.storage.Length; i++)
        {
            myStorage.storage[i] = (sbyte)ResourceType.empty;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        currentProgress += Time.deltaTime;

        if (currentProgress > maxProgress)//add resource to storage
        {
            if (addToOutput()) currentProgress--;
        }
        currentProgress = Mathf.Min(currentProgress, maxProgress);
    }

    //tries to add resource to output, if full return false
    bool addToOutput()
    {
        if (output.quantity >= myStorage.maxStorage) return false;
        output.quantity++;
        return true;
    }
}
