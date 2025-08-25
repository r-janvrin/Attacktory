using System;
using UnityEngine;

public struct productionResource
{
    public sbyte resourceType;
    public byte quantity;
    public byte numRequired;

    public productionResource(sbyte type, byte qty, byte howMany)
    {
        resourceType = type;
        quantity = qty;
        numRequired = howMany;
    }
}

public class FactoryScript : outputBuilding
{
    [SerializeField] private int maxQuantity;
    [SerializeField] FactoryData data;

    private productionResource[] inputResources;
    private productionResource[] producedResources;
    private float currentProduction;
    public override void setupResources(Vector2Int bottomLeftPosition)
    {
        base.setupResources(bottomLeftPosition);
        currentProduction = 0;
        inputResources = new productionResource[data.numRequired.Length];

        for (int i = 0; i < inputResources.Length; i++)
        {
            inputResources[i] = new productionResource((sbyte)data.requiredResources[i], 0, data.numRequired[i]);
        }

        producedResources = new productionResource[data.numOutputted.Length];
        for (int i = 0; i < producedResources.Length; i++)
        {
            producedResources[i] = new productionResource((sbyte)data.outputResources[i], 0, data.numOutputted[i]);
        }

    }

    void Update()
    {
        //check if we have everything required
        if (haveProductionRequirements(inputResources)) produce();
        for(int i = 0; i < producedResources.Length; i++)
        {
            if (producedResources[i].quantity > 0 && outputResources(producedResources[i].resourceType)) producedResources[i].quantity--;
        }
    }

    bool haveProductionRequirements(productionResource[] requirements)
    {
        for(int i = 0; i < requirements.Length; i++)
        {
            if (requirements[i].numRequired > requirements[i].quantity) return false;
        }
        return true;
    }

    bool haveSpace(productionResource[] output)
    {
        for(int i = 0; i < output.Length; i++)
        {
            if ((output[i].quantity + output[i].numRequired) > maxQuantity) return false;
        }
        return true;
    }

    void produce()
    {
        currentProduction += Time.deltaTime;
        if (currentProduction < data.productionTime) return;
        if (!haveSpace(producedResources)) return;
        //add the resources and reset timer
        currentProduction = 0;
        for (int i = 0; i < inputResources.Length; i++) inputResources[i].quantity -= inputResources[i].numRequired;
        for (int i = 0; i < producedResources.Length; i++) producedResources[i].quantity += producedResources[i].numRequired; 
    }
    //add the resource if it's a type we produce and there's space
    public override bool AddResource(sbyte resourceType, Vector2Int direction)
    {
        for(int i = 0; i < inputResources.Length; i++)
        {
            if (inputResources[i].resourceType == resourceType && inputResources[i].quantity < maxQuantity)
            {
                inputResources[i].quantity++;
                return true;
            }
        }
        return false;
    }
}
