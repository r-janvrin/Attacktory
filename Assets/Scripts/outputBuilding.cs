using UnityEngine;

public class outputBuilding : baseBuildingScript
{
    private directionalResource[] positions;
    public override void setupResources(Vector2Int bottomLeftPosition)
    {
        base.setupResources(bottomLeftPosition);

        positions = new directionalResource[buildingSize * 4];
        for (int i = 0; i < buildingSize; i++)
        {
            //up then i right
            positions[i * 4] = new directionalResource(position + Vector2Int.up * buildingSize + Vector2Int.right * i, Vector2Int.up);

            //down then i right
            positions[i * 4 + 1] = new directionalResource(position + Vector2Int.down + Vector2Int.right * i, Vector2Int.down);

            //left then i up
            positions[i * 4 + 2] = new directionalResource(position + Vector2Int.left + Vector2Int.up * i, Vector2Int.left);

            //right then i up
            positions[i * 4 + 3] = new directionalResource(position + Vector2Int.right * buildingSize + Vector2Int.up * i, Vector2Int.right);
        }
        scramblePositions();
    }

    //basic way to output resources - returns true if a resource is outputted
    public bool outputResources(sbyte resourceType)
    {
        for (int i = 0; i < buildingSize * 4; i++)
        {
            if (buildingGrid.grid.addToPosition(positions[i].position, resourceType, positions[i].direction))
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
        for (int i = buildingSize * 4 - 1; i > 0; i--)
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
