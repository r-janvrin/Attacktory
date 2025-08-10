using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.AllocatorManager;

public class testScript : MonoBehaviour
{
    [SerializeField] private InputActionReference click;
    [SerializeField] private InputActionReference scroll;
    [SerializeField] private GameObject currentBuilding;
    [SerializeField] private byte currentSize;
    [SerializeField] private bool validPosition;
    [SerializeField] private GameObject hoverObject;
    private SpriteRenderer hoverSprite;
    private bool isMouseDown;

    private Color whiteColor;
    private Color redColor;

    public float scrollValue;
    private int currentRotation;
    private Quaternion[] rotations;
    Vector2 lastDirection;
    creationQueue plannedBuildings;

    Vector2Int sizeOffset;
    Vector2 mousePosition;
    Vector2Int gridPosition;
    Vector2 clickPosition;
    private void Awake()
    {
        //create the 2 colors: white is slightly transparent, red is red & transparent
        whiteColor = new Color(1, 1, 1, 0.8f);
        redColor = new Color(1, 0.35f, 0.35f, 0.8f);
        plannedBuildings = new creationQueue(500);
        currentSize = currentBuilding.GetComponent<baseBuildingScript>().buildingSize;
        hoverSprite = hoverObject.GetComponent<SpriteRenderer>();
        int degrees = 0;
        rotations = new Quaternion[4];
        for (int i = 0; i < 4; i++)
        {
            rotations[i] = Quaternion.AngleAxis(degrees, Vector3.forward);
            degrees += 90;
        }
        isMouseDown = false;
    }
    void Update()
    {
        //check to see if scroll up or down and rotate based on that
        scrollValue = scroll.action.ReadValue<float>();
        if (scrollValue > 0) ScrollUp();
        else if (scrollValue < 0) ScrollDown();

        //get where the mouse is
        mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        gridPosition = TileDataManager.manager.getVector2Int(mousePosition) - sizeOffset;
        validPosition = areaIsValid(gridPosition, currentSize);

        //show where the building will be built and if it can be built
        hoverObject.transform.position = gridPosition + new Vector2(currentSize * 0.5f, currentSize * 0.5f);
        if (validPosition)
        {
            hoverSprite.color = whiteColor;
        }
        else
        {
            hoverSprite.color = redColor;
        }
        if (isMouseDown) mouseDownBehaviour();
    }

    void mouseDownBehaviour()
    {
        //check which direction the mouse is furthest from
        int numBuildings;
        Vector2 mouseDirection = getMouseDirection(out numBuildings);
        if(lastDirection == mouseDirection) //if we're in the same direction
        {
            plannedBuildings.setNumBuildings(numBuildings, currentBuilding);
            return;
        }
        //get rid of all old buildings and add all the new ones
        plannedBuildings.changeDirection(mouseDirection, numBuildings, hoverObject);
    }

    private void OnEnable()
    {
        click.action.started += ClickStart;
        click.action.canceled += ClickEnd;
        click.action.Enable();
        scroll.action.Enable();
    }

    private void OnDisable()
    {
        click.action.started -= ClickStart;
        click.action.canceled -= ClickEnd;
        click.action.Disable();
        scroll.action.Disable();
    }

    private void ClickStart(InputAction.CallbackContext obj)
    {
        //getting values of start of click
        isMouseDown = true;
        clickPosition = new Vector2(gridPosition.x + currentSize * 0.5f, gridPosition.y + currentSize * 0.5f);
        plannedBuildings.setPosition(clickPosition);
        lastDirection = Vector2.zero;
        Debug.Log("CLICKED");
        //putting a building at the location - to be removed
        //if (!validPosition) return;
        //buildingGrid.grid.createBuilding(gridPosition, currentBuilding, rotations[currentRotation]);
    }

    private void ClickEnd(InputAction.CallbackContext obj)
    {
        Debug.Log("STOPPED CLICKING");
        isMouseDown = false;
        //put all our planned buildings to the building manager
        plannedBuildings.clearQueue();
    }

    private void ScrollUp()
    {
        Debug.Log("Scrolled Up!");
        currentRotation++;
        if (currentRotation > 3) currentRotation = 0;
        hoverObject.transform.rotation = rotations[currentRotation];
    }
    private void ScrollDown()
    {
        Debug.Log("Scrolled down!");
        currentRotation--;
        if (currentRotation < 0) currentRotation = 3;
        hoverObject.transform.rotation = rotations[currentRotation];
    }

    public Vector2 getMouseDirection(out int numBuildings){
        Vector2 mouseOffset = mousePosition - clickPosition;
        Debug.Log("Mouse Offset: " + mouseOffset);


        //if we are more horizontal than vertical
        if (Mathf.Abs(mouseOffset.x) > Mathf.Abs(mouseOffset.y))
        {
            numBuildings = 1 + (int)(Mathf.Abs(mouseOffset.x) + currentSize*0.5f) / currentSize;
            if (mouseOffset.x < 0) return Vector2.left;
            //numBuildings++;
            return Vector2.right;
        }
        numBuildings = 1 + (int)(Mathf.Abs(mouseOffset.y) + currentSize*0.5f) / currentSize;
        if (mouseOffset.y < 0) return Vector2.down;
        //numBuildings++;
        return Vector2.up;

    }

    public bool areaIsValid(Vector2Int bottomLeft, byte size)
    {
        if (!buildingGrid.grid.tilesAreEmpty(bottomLeft, size)) return false; //make sure there's no buildings
        if (!TileDataManager.manager.tilesAreValid(bottomLeft, size)) return false; //make sure there's no walls
        return true;
    }

    public void setPrefab(GameObject prefab)
    {
        currentBuilding = prefab;
        currentSize = currentBuilding.GetComponent<baseBuildingScript>().buildingSize;
        plannedBuildings.setBuildingSize(currentSize);
        sizeOffset = new Vector2Int(currentSize / 2, currentSize / 2);
        hoverSprite.sprite = currentBuilding.GetComponent<SpriteRenderer>().sprite;
    }
}

class creationQueue
{
    public int numPlanned;
    public GameObject[] buildings;
    private Vector2 direction;
    private Vector2 position;
    private Quaternion rotation;
    private int buildingSize;
    private GameObject prefab;
    public creationQueue(int sizeArray)
    {
        numPlanned = 0;
        buildings = new GameObject[sizeArray];
        rotation = Quaternion.identity;
        buildingSize = 1;
    }
    public void setPosition(Vector2 newPosition)
    {
        position = newPosition;
    }
    public void setBuildingSize(int newSize)
    {
        buildingSize = newSize;
    }

    public void addToQueue(GameObject objToAdd)
    {
        buildings[numPlanned] = GameObject.Instantiate(objToAdd, position + direction * numPlanned, rotation);
        numPlanned++;
    }
    public void setRotation(Quaternion targetRotation)
    {
        rotation = targetRotation;
        for(int i = 0; i < numPlanned; i++)
        {
            buildings[i].transform.rotation = rotation;
        }
    }
    public void removeFromQueue()
    {
        if (numPlanned == 0) return;
        GameObject.Destroy(buildings[--numPlanned]);
    }

    public void clearQueue()
    {
        while (numPlanned > 0) removeFromQueue();
    }
    public void setNumBuildings(int howMany, GameObject objToAdd)
    {
        if (howMany < 0) return;
        Debug.Log("How many:" + howMany);
        while(howMany > numPlanned)
        {
            addToQueue(objToAdd);
        }
            //remove
        while(numPlanned > howMany)
        {
            removeFromQueue();
        }
    }
    public void changeDirection(Vector2 newDirection, int howMany, GameObject objToAdd)
    {
        direction = newDirection;
        clearQueue();
        setNumBuildings(howMany, objToAdd);
    }
}