using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 position; // position in world
    public int positionInArray; // position in array of parent
    public BlockNodes nodesParent;
    public bool walkable;
    public bool buildable;
    public float walkSpeed;

    //public int gCost;
    //public int hCost;
    //public int fCost;

    public Node (Vector3 position, int positionInArray, BlockNodes nodesParent, bool walkable = false, bool buildable = false)
    {
        this.position = position;
        this.positionInArray = positionInArray;
        this.nodesParent = nodesParent;
        this.walkable = walkable;
        this.buildable = buildable;
    }
    //public void UpdateMovementValues(int newGcost, int newHcost)
    //{
    //    gCost = newGcost;
    //    hCost = newHcost;
    //    fCost = gCost + hCost;
    //}
}
