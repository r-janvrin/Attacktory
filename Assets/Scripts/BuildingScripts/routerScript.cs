using UnityEngine;

public class routerScript : baseBuildingScript
{
    routerResource output;
    Vector2Int[] directions;
    byte i;
    int current;
    sbyte resType;
    public override void setupResources(Vector2Int bottomLeftPosition)
    {
        base.setupResources(bottomLeftPosition);
        output = null;
        directions = new Vector2Int[3];
    }
    public override bool AddResource(sbyte resourceType, Vector2Int direction)
    {
        if (output != null) return false;
        resType = resourceType;
        //randomize the directions it can go in
        current = (byte)Random.Range(0, 2); //0 or 1
        directions[0] = direction;
        if(direction.x != 0)//dir is left or right
        {
            directions[2 - i] = Vector2Int.up;
            directions[1 + i] = Vector2Int.down;
        }
        else //dir is up or down
        {
            directions[2 - i] = Vector2Int.right;
            directions[1 + i] = Vector2Int.left;
        }
        current = (byte)Random.Range(0, 4);//random starting position
        return true;
    }

    private void Update()
    {
        if (resType >= 0) outputResources();
    }

    private void outputResources()
    {
        for (i = 0; i < 3; i++)
        {
            if( buildingGrid.grid.addToPosition(position, resType, directions[current]) )
            {
                resType = -1;//empty
                return;
            }
            current = (current+1) % 3;
        }
    }
}

class routerResource
{
    Vector2Int direction;
    sbyte resourceType;
    public routerResource(sbyte type, Vector2Int dir)
    {
        resourceType = type;
        direction = dir;
    }
}