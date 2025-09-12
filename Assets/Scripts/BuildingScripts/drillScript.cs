using System;
using UnityEngine;

[Serializable] public struct resourcePair
{
    public byte quantity;
    public sbyte resourceType;

    public resourcePair(sbyte type, byte storage)
    {
        quantity = storage;
        resourceType = type;
    }
}

public class drillScript : outputBuilding
{
    public byte maxStorage;
    [SerializeField] DrillSpeedData speedData;

    //private storageScript myStorage;
    public ResourceType myMaxTier;

    private float maxProgress;
    private float currentProgress=0;
    private resourcePair output;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        byte numTiles;
        //get which resource im producing and set how many to 0
        output = new resourcePair(TileDataManager.manager.getResourceType(position, buildingSize, myMaxTier, out numTiles), 0);
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

        if (output.quantity >0)
        {
            if (base.outputResources(output.resourceType)) output.quantity--;
        }
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