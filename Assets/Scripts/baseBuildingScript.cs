using UnityEngine;

public abstract class baseBuildingScript : MonoBehaviour
{
    [HideInInspector] public buildingGrid grid;
    public byte buildingSize;
    [HideInInspector] public Vector2Int position;
    public float health;
    private directionalResource[] positions;

    public virtual void setupResources(Vector2Int bottomLeftPosition)
    {
        grid = GameObject.Find("BuildingManager").GetComponent<buildingGrid>(); //get reference to building grid
        position = bottomLeftPosition;
        positions = new directionalResource[buildingSize * 4];

        for(int i = 0; i < buildingSize; i++) { 
            //up then i right
            positions[i*4] = new directionalResource(position+Vector2Int.up * buildingSize + Vector2Int.right*i, Vector2Int.up);
            //down then i right
            positions[i*4 + 1] = new directionalResource(position+Vector2Int.down + Vector2Int.right*i, Vector2Int.down);

            //left then i up
            positions[i*4 + 2] = new directionalResource(position+Vector2Int.left + Vector2Int.up*i, Vector2Int.left);

            //right then i up
            positions[i*4 + 3] = new directionalResource(position + Vector2Int.right*buildingSize + Vector2Int.up*i, Vector2Int.right);
        }
        scramblePositions();
        
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
        for(int i = 0; i < buildingSize*4; i++)
        {
            if (grid.addToPosition(positions[i].position, resourceType, positions[i].direction))
            {
                scramblePositions();
                return true;
            }
        }
        return false;
    }

    //Fisher-Yates shuffle algorithm
    private void scramblePositions()
    {
        int rand;
        for(int i=buildingSize*4-1; i>0; i--)
        {
            rand = Random.Range(0, i);
            swap(i, rand);
        }
    }

    private void swap(int a, int b)
    {
        directionalResource temp = positions[a];
        positions[a] = positions[b];
        positions[b] = temp;
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

public struct directionalResource
{
    public Vector2Int direction;
    public Vector2Int position;
    public directionalResource(Vector2Int pos, Vector2Int dir)
    {
        position = pos;
        direction = dir;
    }
}