using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridHexagonal<TGridObject>
{
    private int width;
    private int height;
    private string shape;
    private float cellSize;
    Vector3 originPosition;
    private TGridObject[,] gridArray;
    TextMesh[,] debugTextArray;

    public GridHexagonal(int width, int height, float cellSize, Vector3 originPosition, string shape = null)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.shape = shape;

        gridArray = new TGridObject[width, height];
        debugTextArray = new TextMesh[width, height];

        bool showDebug = false;
        if (showDebug)
        {
            Debug.DrawLine(GetWorldPosition(0, 0), HexFlatTopCorner(GetWorldPosition(0, 0), 5, 1), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(0, 0), HexFlatTopCorner(GetWorldPosition(0, 0), 5, 2), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(0, 0), HexFlatTopCorner(GetWorldPosition(0, 0), 5, 3), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(0, 0), HexFlatTopCorner(GetWorldPosition(0, 0), 5, 4), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(0, 0), HexFlatTopCorner(GetWorldPosition(0, 0), 5, 5), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(0, 0), HexFlatTopCorner(GetWorldPosition(0, 0), 5, 6), Color.white, 100f);

            Debug.DrawLine(GetWorldPosition(0, 0), HexPointyTopCorner(GetWorldPosition(0, 0), 5, 1), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(0, 0), HexPointyTopCorner(GetWorldPosition(0, 0), 5, 2), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(0, 0), HexPointyTopCorner(GetWorldPosition(0, 0), 5, 3), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(0, 0), HexPointyTopCorner(GetWorldPosition(0, 0), 5, 4), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(0, 0), HexPointyTopCorner(GetWorldPosition(0, 0), 5, 5), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(0, 0), HexPointyTopCorner(GetWorldPosition(0, 0), 5, 6), Color.white, 100f);


            //Debug.DrawLine(GetWorldPosition(0, 0), GetWorldPosition(1, 0), Color.white, 100f);
            //Debug.DrawLine(GetWorldPosition(1, 0), HexFlatTopCorner(GetWorldPosition(1,0), 1, 1), Color.white, 100f);
            //Debug.DrawLine(GetWorldPosition(1, 0), HexFlatTopCorner(GetWorldPosition(1, 0), 1, 1), Color.white, 100f);
            //Debug.DrawLine(HexFlatTopCorner(GetWorldPosition(1, 0), 1, 1), HexFlatTopCorner(HexFlatTopCorner(GetWorldPosition(1, 0), 1, 1), 1, 8), Color.white, 100f);
            //Debug.DrawLine(GetWorldPosition(0, 0), HexFlatTopCorner(GetWorldPosition(-1, 0), 1, 1), Color.white, 100f);
            //Debug.DrawLine(HexFlatTopCorner(GetWorldPosition(-1, 0),1,1), HexFlatTopCorner(HexFlatTopCorner(GetWorldPosition(-1, 0), 1, 1), 1, 1), Color.white, 100f);
            //Debug.DrawLine(HexFlatTopCorner(HexFlatTopCorner(GetWorldPosition(-1, 0), 1, 1), 1, 1), HexFlatTopCorner(HexFlatTopCorner(GetWorldPosition(1, 0), 1, 1), 1, 8), Color.white, 100f);

            //Debug.DrawLine(HexFlatTopCorner(GetWorldPosition(1, 0), 1, 2), HexFlatTopCorner(GetWorldPosition(1, 0), 1, 1), Color.white, 100f);
            //Debug.DrawLine(GetWorldPosition(0, 2), GetWorldPosition(1, 2), Color.white, 100f);
            //for (int x = 0; x < gridArray.GetLength(0); x++)
            //{
            //    for (int y = 0; y < gridArray.GetLength(1); y++)
            //    {
            //        //debugTextArray[x,y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter);
            //        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
            //        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            //    }
            //}
            //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            //Debug.DrawLine(GetWorldPosition(width, height), GetWorldPosition(width, 0), Color.white, 100f);
        }
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetXYZ(Vector3 worldPosition, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }
    public void GetXYZ(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetValue(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            //if (!debugTextArray[x, y])
            //{
            //    debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter);
            //}
            //debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, TGridObject value)
    {
        int x, y, z;
        GetXYZ(worldPosition, out x, out y, out z);
        SetValue(x, y, value);
    }
    public TGridObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }
    public TGridObject GetValue(Vector3 worldPosition)
    {
        int x, y, z;
        GetXYZ(worldPosition, out x, out y, out z);
        return GetValue(x, y);
    }// potrebujem aby mi to vratilo poziciu x @ y
    public int GetLenght(int index)
    {
        if (index == 0)
        {
            return gridArray.GetLength(0);
        }
        if (index == 1)
        {
            return gridArray.GetLength(1);
        }
        return -1;
    }
    public Vector3 HexFlatTopCorner(Vector3 center, float size, int i)
    {
        float angleDeg = 60 * i;
        float angleRad = Mathf.PI / 180 * angleDeg;

        return new Vector3(center.x + size * Mathf.Cos(angleRad),
                            center.y + size * Mathf.Sin(angleRad));
    }
    public Vector3 HexPointyTopCorner(Vector3 center, float size, int i)
    {
        float angleDeg = 60 * i - 30;
        float angleRad = Mathf.PI / 180 * angleDeg;

        return new Vector3(center.x + size * Mathf.Cos(angleRad),
                            center.y + size * Mathf.Sin(angleRad));
    }
}
