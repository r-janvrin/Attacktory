using UnityEngine;

public class routerScript : baseBuildingScript
{
    [SerializeField] private float maxTime;
    private float currentTime;
    Vector2Int[] directions;
    byte i;
    int current;
    sbyte resType;
    public override void setupResources(Vector2Int bottomLeftPosition)
    {
        base.setupResources(bottomLeftPosition);
        resType = (sbyte)ResourceType.empty;
        directions = new Vector2Int[3];
    }
    public override bool AddResource(sbyte resourceType, Vector2Int direction)
    {
        if (resType >= 0) return false; // if there's already something in the router
        resType = resourceType;
        //randomize the directions it can go in
        i = (byte)Random.Range(0, 2); //0 or 1
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
        current = (byte)Random.Range(0, 3);//random starting position
        return true;
    }

    private void Update()
    {
        if (resType >= 0) outputResources();
    }

    private void outputResources()
    {
        currentTime += Time.deltaTime;
        if (currentTime < maxTime) return;
        for (i = 0; i < 3; i++)
        {
            if ( buildingGrid.grid.addToPosition(position + directions[current], resType, directions[current]) )
            {
                resType = -1;//empty
                currentTime = 0;
                return;
            }
            current = (current+1) % 3;
        }
    }
}