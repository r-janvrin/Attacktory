using UnityEngine;

public class HomeBaseScript : baseBuildingScript
{
    StorageManagerScript StorageRef;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void setupResources(Vector2Int bottomLeftPosition)
    {
        base.setupResources(bottomLeftPosition);
        StorageRef = GameObject.Find("StorageManager").GetComponent<StorageManagerScript>();
        
    }

    public override bool AddResource(sbyte resourceType, Vector2Int direction)
    {
        StorageRef.add_to_storage(resourceType);
        
        return true;
    }
}
