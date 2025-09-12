using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;


public class StorageManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject displayPrefab;
    [SerializeField] private Transform displayPositionObject;
    //public static event Action<StorageManagerScript> OnResourceReceived;
    [SerializeField] private float hztlPerDisplay;
    [SerializeField] private float verticalPerDisplay;
    [SerializeField] private int numPerRow;

    [HideInInspector] public static StorageManagerScript manager;
    int num_ids = 17;
    private GameObject[] ResourceDisplayObjects;
    private TextMeshProUGUI[] ResourceTexts;
    public int[] StoredResources;
    public int max;

    public void add_to_storage (sbyte store_id) {
        if (store_id >= ResourceDisplayObjects.Length) return;
        if (StoredResources[store_id] >= max) return;
        StoredResources[store_id] += 1;

        //update the display for this resource
        if(ResourceDisplayObjects[store_id].activeSelf == false)
        {
            ResourceDisplayObjects[store_id].SetActive(true);
            updateDisplayPositions();
        }
        ResourceTexts[store_id].SetText( StoredResources[store_id].ToString() );
    }

    public void add_qty_to_storage(sbyte store_id, int qty)
    {
        StoredResources[store_id] = Mathf.Min(max, StoredResources[store_id] + qty);
        ResourceTexts[store_id].SetText(StoredResources[store_id].ToString());
    }

    public void remove_from_storage(sbyte store_id, int qty)
    {
        StoredResources[store_id] = Mathf.Max(0, StoredResources[store_id] - qty);
        ResourceTexts[store_id].SetText(StoredResources[store_id].ToString());
    }

    public void updateDisplayPositions()
    {
        int numRows = 0;
        int numInRow = 0;
        for(int i = 0; i < ResourceDisplayObjects.Length; i++)
        {
            if (!ResourceDisplayObjects[i].activeSelf) continue;
            ResourceDisplayObjects[i].transform.position = displayPositionObject.transform.position + new Vector3(hztlPerDisplay * numInRow, verticalPerDisplay * numRows, 0);
            numInRow++;
            if(numInRow == numPerRow)
            {
                numInRow = 0;
                numRows++;
                //increase size of border
            }
        }
    }

    public void max_change (sbyte max_num) {
        max += max_num;
        for (int i = 0; i < num_ids; i += 1) {
            StoredResources[i] = Mathf.Min(StoredResources[i], max);
        } 
    }

    void Awake ()
    {
        manager = this;
        StoredResources = new int[num_ids];
    }

    private void Start()
    {
        ResourceTexts = new TextMeshProUGUI[num_ids];
        ResourceDisplayObjects = new GameObject[num_ids];
        for (sbyte i = 0; i < num_ids; i++)
        {
            ResourceDisplayObjects[i] = Instantiate(displayPrefab, displayPositionObject);
            ResourceTexts[i] = ResourceDisplayObjects[i].GetComponentInChildren<TextMeshProUGUI>();
            ResourceDisplayObjects[i].GetComponentInChildren<Image>().sprite = buildingGrid.grid.getResourceSprite(i);
            ResourceDisplayObjects[i].SetActive(false);
        }
        //TEST ADD EACH RESOURCE
        for (sbyte i = 0; i < num_ids; i++)
        {
            StoredResources[i] += 500;
            add_to_storage(i);
        }
    }

    public bool haveResources(costPair[] costs)
    {
        foreach(costPair cost in costs)
        {
            if (cost.quantity > StoredResources[(sbyte)cost.type])
            {
                Debug.Log("Not enough of " + cost.type);
                Debug.Log("need:" + cost.quantity + "have" + StoredResources[(sbyte)cost.type]);
                return false;
            }
        }
        Debug.Log("We have enough");
        return true;
    }

    public void removeResources(costPair[] costs)
    {
        foreach(costPair cost in costs)
        {
            remove_from_storage((sbyte)cost.type, cost.quantity);
        }
    }
}
