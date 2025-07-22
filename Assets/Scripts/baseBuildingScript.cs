using UnityEngine;

public abstract class baseBuildingScript : MonoBehaviour
{
    [HideInInspector] public buildingGrid grid;
    public byte buildingSize;
    [HideInInspector] public Vector2Int position;
    public float health;

    public virtual void setupResources(Vector2Int bottomLeftPosition)
    {
        grid = GameObject.Find("BuildingManager").GetComponent<buildingGrid>(); //get reference to building grid
        position = bottomLeftPosition;
    }

    public virtual bool AddResource(sbyte resourceType, Vector2Int direction)
    {
        return false;//by default, do not accept input.
    }

    //deal damage, return false if this object should be removed (died)
    public virtual bool dealDamage(float damage)
    {
        health -= damage;
        return health < 0;
    }

    //default way to output resources - returns true if a resource is outputted
    public virtual bool outputResources(sbyte resourceType)
    {
        for(int i = 0; i < buildingSize; i++)
        {
            //up and then i right
            if (grid.addToPosition(position + Vector2Int.up * buildingSize + Vector2Int.right * i, resourceType, Vector2Int.up)) return true;
            //right and then i up
            if (grid.addToPosition(position + Vector2Int.right * buildingSize + Vector2Int.up * i, resourceType, Vector2Int.up)) return true;
            //down and then i right
            if (grid.addToPosition(position + Vector2Int.down + Vector2Int.right * i, resourceType, Vector2Int.up)) return true;
            //left and then i up
            if (grid.addToPosition(position + Vector2Int.left + Vector2Int.up * i, resourceType, Vector2Int.up)) return true;
        }
        return false;
    }
}
