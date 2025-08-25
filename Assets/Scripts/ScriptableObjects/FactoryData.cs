using UnityEngine;

[CreateAssetMenu(fileName = "FactoryData", menuName = "Scriptable Objects/FactoryData")]
public class FactoryData : ScriptableObject
{
    public ResourceType[] requiredResources;
    public sbyte[] numRequired;

    public ResourceType[] outputResources;
    public sbyte[] numOutputted;
}
