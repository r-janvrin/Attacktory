using UnityEngine;


public class buildingGrid : MonoBehaviour
{
    const int NUM_RESOURCES = 20;

    private Building[,] buildingArray;
    public short xSize;
    public short ySize;
    void Awake()//awake is called before start
    {
        buildingArray = new Building[xSize, ySize]; //each building starts as null

    }

    //the building class will store all the data of a building
    class Building
    {
        private buildingGrid grid;
        private short xPos;
        private short yPos;

        byte buildingSize;//1x1, 2x2, etc
        byte buildingType;//whether building is drill, wall, gun, etc
        sbyte[] storage;//how much each resource is stored: -1 means not accepted

        public Building(buildingGrid grid, short x, short y)//constructor for building object
        {
            
            this.grid = grid;
            this.xPos = x;
            this.yPos = y;
            this.storage = new sbyte[NUM_RESOURCES];
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    void testFunction()
    {
        Debug.Log("Function called successfully!");
    }
}

