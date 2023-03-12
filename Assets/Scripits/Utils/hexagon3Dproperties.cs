using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon3Dproperties
{
    public Vector3 position;
    public int size;
    int posY;
    int posX;

    public Hexagon3Dproperties(Vector3 position, int size, int posY = 0, int posX = 0)
    {
        this.position = position;
        this.size = size;
        this.posX = posX;
        this.posY = posY;
    }

    //[bot, botLeft, botRight, top, topLeft, topRight]
    string[] names = new string[6] { "bot", "botLeft", "botRight", "top", "topLeft", "topRight" };

    //[bot, botLeft, botRight, top, topLeft, topRight]
    Vector3[] positions = new Vector3[6] { new Vector3(-.25f, .433f, 1) , new Vector3(.25f, .433f, 1) ,
            new Vector3(-.5f, 0, 1) , new Vector3(.25f, -.433f, 1) , new Vector3(.5f, 0, 1) , new Vector3(-.25f, -.433f, 1) };

    //[bot, botLeft, botRight, top, topLeft, topRight]
    Vector3[] rotations = new Vector3[6] { new Vector3(90, 90) , new Vector3(150, 90) ,
            new Vector3(30, 90) , new Vector3(-90, 90) , new Vector3(-150, 90) , new Vector3(-30, 90) };
    public string GetName(int index)
    {
        return names[index];
    }
    public Vector3 GetPosition(int index)
    {
        return positions[index];
    }
    public Vector3 GetRotation(int index)
    {
        return rotations[index];
    }
    public void SetupSide(GameObject side, int index)
    {
        side.transform.name = names[index];
        side.transform.position = positions[index];
        side.transform.eulerAngles = rotations[index];
    }
}
