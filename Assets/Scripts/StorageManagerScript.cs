using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

public class StorageManagerScript : MonoBehaviour
{
    //public static event Action<StorageManagerScript> OnResourceReceived;
    [SerializeField] private float sizePerDisplay;

    [HideInInspector] public static StorageManagerScript manager;
    int num_ids = 28;
    [SerializeField] private GameObject[] ResourceDisplayObjects;
    private TextMeshProUGUI[] ResourceTexts;
    public int[] StoredResources;
    public int max;
    public TextMeshProUGUI UI_text;
 
    public GameObject UI_GameObject;


    public void add_to_storage (sbyte store_id) {
        if (StoredResources[store_id] < max)
        {
            StoredResources[store_id] += 1;
        }
        //update the display for this resource
        if (store_id >= ResourceDisplayObjects.Length) return;
        ResourceDisplayObjects[store_id].SetActive(true);
        ResourceTexts[store_id].SetText( StoredResources[store_id].ToString() );
    }

    public void updateDisplay(sbyte resourceID)
    {

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
        UI_text = UI_GameObject.GetComponent<TextMeshProUGUI>();
        ResourceTexts = new TextMeshProUGUI[num_ids];
        for(sbyte i = 0; i < ResourceDisplayObjects.Length; i++)
        {
            ResourceTexts[i] = ResourceDisplayObjects[i].GetComponentInChildren<TextMeshProUGUI>();
            ResourceDisplayObjects[i].GetComponentInChildren<Image>().sprite = buildingGrid.grid.getResourceSprite(i);
        }
    }

    public void Update() {
        UI_text.SetText("it works");
    }
}
