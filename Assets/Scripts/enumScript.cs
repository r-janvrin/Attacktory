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
    sand = 1,
    wood = 2,
    coal = 3,
    copperOre = 4,
    ironOre = 5,
    sulfurOre = 6,
    aluminumOre = 7,
    ironBar = 8,
    steelBar = 9,
    concrete = 10,
    wiring = 11,
    gunpowder = 12,
    reinforcedConcrete = 13,
    aluminumBar = 14,
    circuitBoard = 15,
    TNT = 16,
    missile = 17
}

public enum ConveyorBehaviour : byte
{
    keep = 0,
    pass = 1,
    delete,
}