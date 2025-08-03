using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

public class StorageManagerScript : MonoBehaviour
{
    [HideInInspector] public static StorageManagerScript manager;
    int num_ids = 28;
    public int[] stored_stuff;
    public int max;
    public TextMeshProUGUI UI_text;
 
    public GameObject UI_GameObject;


    public void add_to_storage (sbyte store_id) {
        if (stored_stuff[store_id] < max)
        {
            stored_stuff[store_id] += 1;
        }
    }

    public void max_change (sbyte max_num) {
        max += max_num;
        for (int i = 0; i < num_ids; i += 1) {
            stored_stuff[i] = Mathf.Min(stored_stuff[i], max);

        } 
        
    }

    void Awake ()
    {
        manager = this;
        stored_stuff = new int[num_ids];
        UI_text = UI_GameObject.GetComponent<TextMeshProUGUI>();
    }

    public void Update() {
        
        UI_text.SetText("it works");
        

}
}
