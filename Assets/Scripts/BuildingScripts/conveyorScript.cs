using UnityEngine;

public class conveyorScript : baseBuildingScript
{
    private Vector2Int direction;
    private Vector2 frontPosition;
    conveyorResourceController backResource;
    conveyorResourceController frontResource;

    private float backTimer;
    private float frontTimer;
    private float maxTimer;
    public float resourcePerSecond;
    [SerializeField] byte numSections;

    public override void setupResources(Vector2Int bottomLeftPosition)
    {
        base.setupResources(bottomLeftPosition);
        direction = new Vector2Int((int)transform.right.x, (int)transform.right.y);//1 tile in the direction the conveyor is facing
        maxTimer = 1 / resourcePerSecond; //how long before a resource can move
        frontPosition = new Vector2(transform.position.x + direction.x * 0.5f, transform.position.y + direction.y * 0.5f);
    }

    private void Update()
    {
        if( frontResource != null)
        {
            frontTimer += Time.deltaTime;
            if (frontTimer >= maxTimer && buildingGrid.grid.addToConveyor(position + direction, frontResource, direction))
            {
                frontTimer = 0;
                frontResource = null;
            }
        }

        if ( backResource != null)
        {
            backTimer += Time.deltaTime;
            if(backTimer >= maxTimer && moveToFront(backResource))
            {
                backTimer = 0;
                backResource = null;
            }

        }

    }



    public override bool addFromConveyor(conveyorResourceController resToAdd, Vector2Int dir)
    {
        if (backResource != null) return false;
        if (direction + dir == Vector2Int.zero) return false;//dont accept backwards input
        backResource = resToAdd;
        backResource.setTarget(transform.position); //move to center of conveyor
        backResource.setSpeed(resourcePerSecond / numSections);
        return true;
    }

    //create a new resource and put it in the back
    public override bool AddResource(sbyte resourceType, Vector2Int dir)
    {
        if (backResource != null) return false;
        if (direction + dir == Vector2Int.zero) return false;//dont accept backwards input
        GameObject temp = GameObject.Instantiate(buildingGrid.grid.getConveyorResource(resourceType), creationPosition(dir), Quaternion.identity);
        backResource = temp.GetComponent<conveyorResourceController>();
        backResource.setup(resourceType, transform.position, (resourcePerSecond / numSections) );
        backResource.setSprite(buildingGrid.grid.getResourceSprite(resourceType));
        return true;
    }

    private Vector3 creationPosition(Vector2Int dir)
    {
        return transform.position - new Vector3(dir.x * 0.5f, dir.y * 0.5f, 0);
    }
    private bool moveToFront(conveyorResourceController resourceToMove) 
    {
        if (frontResource != null) return false;
        frontResource = backResource;
        frontResource.setTarget(frontPosition);
        return true;
    }
    

    
}
