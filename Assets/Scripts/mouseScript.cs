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
    public float scrollValue;

    private int currentRotation;
    private Quaternion[] rotations;

    Vector2Int sizeOffset;
    Vector2 mousePosition;
    Vector2Int gridPosition;
    private void Awake()
    {
        currentSize = currentBuilding.GetComponent<baseBuildingScript>().buildingSize;
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
        scrollValue = scroll.action.ReadValue<float>();
        if (scrollValue > 0) ScrollUp();
        else if (scrollValue < 0) ScrollDown();
        mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        gridPosition = TileDataManager.manager.getVector2Int(mousePosition) - sizeOffset;
        validPosition = areaIsValid(gridPosition, currentSize);
        //Debug.Log(validPosition);
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
    }

    private void ScrollDown()
    {
        Debug.Log("Scrolled down!");
        currentRotation--;
        if (currentRotation < 0) currentRotation = 3;
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
    }
}
