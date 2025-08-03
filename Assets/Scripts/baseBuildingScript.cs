using UnityEngine;

public abstract class baseBuildingScript : MonoBehaviour
{
    public byte buildingSize;
    [HideInInspector] public Vector2Int position;
    public float health;

    public virtual void setupResources(Vector2Int bottomLeftPosition)
    {
        position = bottomLeftPosition;
    }

    public virtual bool AddResource(sbyte resourceType, Vector2Int direction)
    {
        return false;//by default, do not accept input.
    }

    //deal damage, return true if this object should be removed (died)
    public virtual bool dealDamage(float damage)
    {
        health -= damage;
        return health < 0;
    }

    public virtual bool addFromConveyor(conveyorResourceController resourceToAdd, Vector2Int direction)
    {
        if(AddResource(resourceToAdd.resourceType, direction))
        {
            resourceToAdd.delete();
            return true;
        }
        return false;
    }
}

