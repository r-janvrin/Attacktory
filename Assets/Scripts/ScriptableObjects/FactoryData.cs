using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FactoryData", menuName = "Scriptable Objects/FactoryData")]
public class FactoryData : ScriptableObject
{
    public ResourceType[] requiredResources;
    public byte[] numRequired;

    public ResourceType[] outputResources;
    public byte[] numOutputted;

    public float productionTime;
}
