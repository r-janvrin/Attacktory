using UnityEngine;

public abstract class baseBuildingScript : MonoBehaviour
{
    //what happens when a DIFFERENT building wants to give this a resource - true if received, false if rejected
    public abstract bool AddResource(sbyte resourceType);
}
