using System;
using UnityEngine;

public class FactoryScript : outputBuilding
{
    [SerializeField] public ResourcePairs requiredResources;
    public int test2;
    public override void setupResources(Vector2Int bottomLeftPosition)
    {
        base.setupResources(bottomLeftPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable] public struct ResourcePairs
{
    public Resources[] whichResource;
    public int[] howMany;
    public int test;
}
