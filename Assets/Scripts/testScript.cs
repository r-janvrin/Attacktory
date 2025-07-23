using UnityEngine;

public class testScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log((int)transform.right.x + " " + (int)transform.right.y + " " + (int)transform.right.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
