public enum BuildingType : byte
{
    conveyor = 0,
    gun,
    factory,
    drill,
    homeBase
}

public enum ResourceType : sbyte
{
    wall = -2,
    empty = -1,
    stone = 0,
    sand,
    wood,
    coal,
    copperOre,
    ironOre,
    sulfurOre,
    aluminumOre,
    ironBar,
    steelBar,
    concrete,
    wiring,
    gunpowder,
    reinforcedConcrete,
    aluminumBar,
    circuitBoard,
    TNT,
    missile
}

public enum ConveyorBehaviour : byte
{
    keep = 0,
    pass = 1,
    delete,
}