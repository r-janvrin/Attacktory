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

    private Color whiteColor;
    private Color redColor;

    public float scrollValue;
    private int currentRotation;
    private Quaternion[] rotations;

    Vector2Int sizeOffset;
    Vector2 mousePosition;
    Vector2Int gridPosition;
    private void Awake()
    {
        //create the 2 colors: white is slightly transparent, red is red & transparent
        whiteColor = new Color(1, 1, 1, 0.8f);
        redColor = new Color(1, 0.35f, 0.35f, 0.8f);
        currentSize = currentBuilding.GetComponent<baseBuildingScript>().buildingSize;
        hoverSprite = hoverObject.GetComponent<SpriteRenderer>();
        int degrees = 0;
        rotations = new Quaternion[4];
        for(int i = 0; i < 4; i++)
        {
            rotations[i] = Quaternion.AngleAxis(degrees, Vector3.forward);
            degrees += 90;
        }
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
    }

    private void OnEnable()
    {
        click.action.started += Click;
        click.action.Enable();
        scroll.action.Enable();
    }

    private void OnDisable()
    {
        click.action.started -= Click;
        click.action.Disable();
        scroll.action.Disable();
    }

    private void Click(InputAction.CallbackContext obj)
    {
        Debug.Log("CLICKED");
        if (!validPosition) return;
        buildingGrid.grid.createBuilding(gridPosition, currentBuilding, rotations[currentRotation]);
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
        sizeOffset = new Vector2Int(currentSize / 2, currentSize / 2);
        hoverSprite.sprite = currentBuilding.GetComponent<SpriteRenderer>().sprite;
    }
}
