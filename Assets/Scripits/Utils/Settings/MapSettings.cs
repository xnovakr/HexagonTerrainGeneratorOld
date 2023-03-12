using System;
using UnityEngine;

[Serializable]
public class MapSettings
{
    public int width;
    public int height;
    public int edgeSize;
    public string shape;
    public float cellSize;
    public Vector3 originPosition;
}
