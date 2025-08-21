using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.AllocatorManager;
using UnityEngine.EventSystems;

public class mouseScript : MonoBehaviour
{
    public static mouseScript controller;
    [SerializeField] private InputActionReference click;
    [SerializeField] private InputActionReference scroll;
    [SerializeField] private GameObject currentBuilding;
    [SerializeField] private byte currentSize;
    [SerializeField] private bool validPosition;
    [SerializeField] private GameObject hoverObject;
    private SpriteRenderer hoverSprite;
    private bool isMouseDown;
    private bool isOverUI;

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
    Vector2Int clickPosition;
    private void Awake()
    {
        controller = this;
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
        isOverUI = EventSystem.current.IsPointerOverGameObject();
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
        Vector2Int mouseDirection = getMouseDirection(out numBuildings);
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
        if (isOverUI || currentBuilding == null) return;//if its a UI click or no choice, don't build
        //getting values of start of click
        isMouseDown = true;
        clickPosition = gridPosition;
        plannedBuildings.setPosition(new Vector2(gridPosition.x + currentSize * 0.5f, gridPosition.y + currentSize * 0.5f));
        lastDirection = Vector2.zero;
    }

    private void ClickEnd(InputAction.CallbackContext obj)
    {
        isMouseDown = false;
        //put all our planned buildings to the building manager
        Vector2Int tempPosition = clickPosition;
        for(int i = 0; i < plannedBuildings.numPlanned; i++)
        {
            if(areaIsValid(tempPosition, currentSize)) buildingGrid.grid.createBuilding(tempPosition, currentBuilding, rotations[currentRotation]);
            tempPosition = tempPosition + plannedBuildings.direction * currentSize;
        }
        plannedBuildings.clearQueue();
    }

    private void ScrollUp()
    {
        currentRotation++;
        if (currentRotation > 3) currentRotation = 0;
        hoverObject.transform.rotation = rotations[currentRotation];
        plannedBuildings.setRotation(rotations[currentRotation]);
    }
    private void ScrollDown()
    {
        currentRotation--;
        if (currentRotation < 0) currentRotation = 3;
        hoverObject.transform.rotation = rotations[currentRotation];
        plannedBuildings.setRotation(rotations[currentRotation]);
    }

    public Vector2Int getMouseDirection(out int numBuildings){
        Vector2 mouseOffset = mousePosition - clickPosition;// + new Vector2(currentSize * 0.5f, currentSize * 0.5f);


        //if we are more horizontal than vertical
        if (Mathf.Abs(mouseOffset.x) > Mathf.Abs(mouseOffset.y))
        {
            numBuildings = 1 + (int)(Mathf.Abs(mouseOffset.x)) / currentSize;
            if (mouseOffset.x > 0) return Vector2Int.right;
            numBuildings++;
            return Vector2Int.left;
        }
        numBuildings = 1 + (int)(Mathf.Abs(mouseOffset.y)) / currentSize;
        if (mouseOffset.y > 0) return Vector2Int.up;
        numBuildings++;
        return Vector2Int.down;

    }
    public bool areaIsValid(Vector2Int bottomLeft, byte size)
    {
        if (!buildingGrid.grid.tilesAreEmpty(bottomLeft, size)) return false; //make sure there's no buildings
        if (!TileDataManager.manager.tilesAreValid(bottomLeft, size)) return false; //make sure there's no walls
        return true;
    }

    public void setPrefab(GameObject prefab)
    {
        if(prefab == null)
        {
            currentBuilding = null;
            hoverObject.SetActive(false);
            return;
        }
        hoverObject.SetActive(true);
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
    public Vector2Int direction;
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
    public void changeDirection(Vector2Int newDirection, int howMany, GameObject objToAdd)
    {
        direction = newDirection;
        clearQueue();
        setNumBuildings(howMany, objToAdd);
    }
}