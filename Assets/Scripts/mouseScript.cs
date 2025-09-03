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
    [SerializeField] private GameObject hoverPrefab;
    private SpriteRenderer hoverSprite;
    private bool isMouseDown;
    private bool isOverUI;

    private Color whiteColor;
    private Color redColor;

    public float scrollValue;
    private int currentRotation;
    public Quaternion[] rotations;
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
        plannedBuildings = new creationQueue(500, whiteColor, redColor);
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
        if(lastDirection == mouseDirection || numBuildings == 1) //if we're in the same direction
        {
            plannedBuildings.setNumBuildings(numBuildings, hoverPrefab, hoverSprite.sprite);
            plannedBuildings.setValidColors();
            return;
        }

        //get rid of all old buildings and add all the new ones
        lastDirection = mouseDirection;
        plannedBuildings.changeDirection(mouseDirection, numBuildings, hoverPrefab, hoverSprite.sprite);
        plannedBuildings.setValidColors();
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
        hoverObject.SetActive(false);
        if (isOverUI || currentBuilding == null) return;//if its a UI click or no choice, don't build
        //getting values of start of click
        isMouseDown = true;
        clickPosition = gridPosition;
        plannedBuildings.setPosition(gridPosition);
        lastDirection = Vector2.zero;
        //Debug.Log(mousePosition);
        //Debug.Log(clickPosition);
    }

    private void ClickEnd(InputAction.CallbackContext obj)
    {
        isMouseDown = false;
        if (currentBuilding == null) return;
        hoverObject.SetActive(true);
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

        //get offset from middle of building
        Vector2 mouseOffset = mousePosition - clickPosition - new Vector2(currentSize * 0.5f, currentSize * 0.5f);
        //Debug.Log(mouseOffset + "" + clickPosition);

        //if we are more horizontal than vertical
        if (Mathf.Abs(mouseOffset.x) > Mathf.Abs(mouseOffset.y))
        {
            numBuildings = (int)Mathf.Ceil(( (Mathf.Abs(mouseOffset.x) + currentSize * 0.5f)/ currentSize));
            if (mouseOffset.x > 0) return Vector2Int.right;
            //numBuildings++;
            return Vector2Int.left;
        }
        numBuildings = (int)Mathf.Ceil(((Mathf.Abs(mouseOffset.y) + currentSize * 0.5f) / currentSize));
        if (mouseOffset.y > 0) return Vector2Int.up;
        //numBuildings++;
        return Vector2Int.down;

    }
    public static bool areaIsValid(Vector2Int bottomLeft, byte size)
    {
        if (!buildingGrid.grid.tilesAreEmpty(bottomLeft, size)) return false; //make sure there's no buildings
        if (!TileDataManager.manager.tilesAreValid(bottomLeft, size)) return false; //make sure there's no walls
        return true;
    }

    public void setPrefab(GameObject prefab)
    {
        if(prefab == null)
        {
            Debug.Log("Invalid Prefab!");
            currentBuilding = null;
            hoverObject.SetActive(false);
            return;
        }
        //Debug.Log("Valid Prefab!");
        hoverObject.SetActive(true);
        currentBuilding = prefab;
        currentSize = currentBuilding.GetComponent<baseBuildingScript>().buildingSize;
        plannedBuildings.setBuildingSize(currentSize);
        sizeOffset = new Vector2Int(currentSize / 2, currentSize / 2);
        hoverSprite.sprite = currentBuilding.GetComponent<SpriteRenderer>().sprite;
    }

    //returns the correct rotation for a direction AND sets the current rotation to that direction
    public Quaternion DirectionToRotation(Vector2Int direction)
    {
        if (direction == Vector2Int.right)
        {
            currentRotation = 0;
            return rotations[0];
        }
        if (direction == Vector2Int.up)
        {
            currentRotation = 1;
            return rotations[1];
        }
        if (direction == Vector2Int.left)
        {
            currentRotation = 2;
            return rotations[2];
        }
        currentRotation = 3;
        return rotations[3];
    }
}

class creationQueue
{
    public int numPlanned;
    public GameObject[] buildings;
    public Vector2Int direction;
    private Vector2 creationPosition;
    private Vector2Int BottomLeftPosition;
    private Quaternion rotation;
    private byte buildingSize;
    private GameObject prefab;
    private Color whiteColor;
    private Color redColor;
    public creationQueue(int sizeArray, Color whiteColor, Color redColor)
    {
        numPlanned = 0;
        buildings = new GameObject[sizeArray];
        rotation = Quaternion.identity;
        buildingSize = 1;
        this.whiteColor = whiteColor;
        this.redColor = redColor;
    }
    public void setPosition(Vector2Int BL_Position)
    {
        BottomLeftPosition = BL_Position;
        creationPosition = BottomLeftPosition + new Vector2(buildingSize * 0.5f, buildingSize * 0.5f);
    }
    public void setBuildingSize(byte newSize)
    {
        buildingSize = newSize;
    }

    public void addToQueue(GameObject objToAdd, Sprite sprite)
    {
        buildings[numPlanned] = GameObject.Instantiate(objToAdd, creationPosition + direction * numPlanned * buildingSize, rotation);
        buildings[numPlanned].GetComponent<SpriteRenderer>().sprite = sprite;
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
    public void setNumBuildings(int howMany, GameObject objToAdd, Sprite sprite)
    {
        if (howMany < 0) return;
        while(howMany > numPlanned)
        {
            addToQueue(objToAdd, sprite);
        }
            //remove
        while(numPlanned > howMany)
        {
            removeFromQueue();
        }
    }
    public void changeDirection(Vector2Int newDirection, int howMany, GameObject objToAdd, Sprite sprite)
    {
        direction = newDirection;
        setRotation(mouseScript.controller.DirectionToRotation(direction));
        clearQueue();
        setNumBuildings(howMany, objToAdd, sprite);
    }

    public void setValidColors()
    {
        Debug.Log("setting colors for " + numPlanned);
        for(int i = 0; i < numPlanned; i++)
        {
            buildings[i].GetComponent<SpriteRenderer>().color = mouseScript.areaIsValid(BottomLeftPosition + direction * i * buildingSize, buildingSize) ? whiteColor : redColor;
        }
    }
}