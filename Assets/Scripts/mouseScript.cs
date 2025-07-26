using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.AllocatorManager;

public class testScript : MonoBehaviour
{
    [SerializeField] private InputActionReference click;
    [SerializeField] private GameObject currentBuilding;
    [SerializeField] private byte currentSize;
    private TileDataManager tileManager;
    private buildingGrid grid;
    [SerializeField] private bool validPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector2 mousePosition;
    Vector2Int gridPosition;
    private void Awake()
    {
        tileManager = GameObject.Find("TileDataManager").GetComponent<TileDataManager>();
        grid = GameObject.Find("BuildingManager").GetComponent<buildingGrid>();
        currentSize = currentBuilding.GetComponent<baseBuildingScript>().buildingSize;
    }
    void Start()
    {
    }

    private void OnEnable()
    {
        click.action.started += Click;
        click.action.Enable();
    }

    private void OnDisable()
    {
        click.action.started -= Click;
        click.action.Disable();
    }

    private void Click(InputAction.CallbackContext obj)
    {
        Debug.Log("CLICKED");
        if (!validPosition) return;
        grid.createBuilding(gridPosition, currentBuilding);
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        gridPosition = tileManager.getVector2Int(mousePosition);
        validPosition = areaIsValid(gridPosition, currentSize);
        //Debug.Log(validPosition);
    }

    public bool areaIsValid(Vector2Int bottomLeft, byte size)
    {
        if (!grid.tilesAreEmpty(bottomLeft, size)) return false; //make sure there's no buildings
        if (!tileManager.tilesAreValid(bottomLeft, size)) return false; //make sure there's no walls
        return true;
    }

    public void setPrefab(GameObject prefab)
    {
        currentBuilding = prefab;
        currentSize = currentBuilding.GetComponent<baseBuildingScript>().buildingSize;
    }
}
