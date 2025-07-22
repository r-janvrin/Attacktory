using UnityEngine;

public class StorageManagerScript : MonoBehaviour
{
    int num_ids = 28;
    int[] stored_stuff;
    public int max;

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
        stored_stuff = new int[num_ids];
    }
        
}
