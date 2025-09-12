using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingCost", menuName = "Scriptable Objects/BuildingCost")]
public class BuildingCost : ScriptableObject
{
    public costPair[] cost;
}

[Serializable] public struct costPair
{
    public ResourceType type;
    public short quantity;
}
