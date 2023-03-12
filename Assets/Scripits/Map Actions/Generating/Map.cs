using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Map : MonoBehaviour
{
    public BasicTexturePack texturePack;
    public MapSettings mapSettings;

    public Material baseMaterial;
    public Material baseBlue;
    public Material baseBrown;
    public Material baseGray;
    public Material baseGreen;
    private GameObject[,] mapArray;
    public Noise noise;
    
    public void MapRun()
    {
        mapArray = new GameObject[mapSettings.width, mapSettings.height];
        GenerateMap(mapSettings.height, mapSettings.width, mapSettings.cellSize, mapSettings.originPosition, mapSettings.shape, mapSettings.edgeSize);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * mapSettings.cellSize + mapSettings.originPosition;
    }

    public void GetXYZ(Vector3 worldPosition, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - mapSettings.originPosition).x / mapSettings.cellSize);
        y = Mathf.FloorToInt((worldPosition - mapSettings.originPosition).y / mapSettings.cellSize);
        z = Mathf.FloorToInt((worldPosition - mapSettings.originPosition).z / mapSettings.cellSize);
    }

    public void GenerateMap(int mapHeigh, int mapWidth, float size, Vector3 originPos = default(Vector3), string shape = null, int edgeSize = 0)
    {
        for (int i = 0; i < mapHeigh; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                if (ShapeCheck(mapWidth, mapHeigh, j, i, shape, edgeSize)) continue;
                GameObject plane = new GameObject("Plane" + i + j);
                SetupBlock(plane, size, originPos, i, j);
                mapArray[j, i] = plane;
            }
        }
        //Destroy(mapArray[5, 5]);
    }
    public void SetupBlock(GameObject plane, float size, Vector3 originPos, int i, int j)
    {
        var position = plane.transform.position;
        position = new Vector3(originPos.x + size * j * 2, originPos.y + size * i * 2, 0);
        plane.transform.position = position;
        MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = CreateMesh(size, size);
        MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        //renderer.material.shader = Shader.Find("Particles/Additive");
        renderer.material = SelectMaterial(i,j);
        plane.AddComponent<BlockBrain>();
        plane.GetComponent<BlockBrain>().originPoint = mapSettings.originPosition;
        plane.GetComponent<BlockBrain>().blockSize = size * 2;
    }
    Mesh CreateMesh(float width, float height)
    {
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";
        m.vertices = new Vector3[]
        {
             new Vector3(width, height, 0.0f),
             new Vector3(width, -height, 0.0f),
             new Vector3(-width, -height, 0.0f),
             new Vector3(-width, height, 0.0f)
        };
        m.uv = new Vector2[]
        {
             new Vector2 (0, 0),
             new Vector2 (0, 1),
             new Vector2(1, 1),
             new Vector2 (1, 0)
        };
        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        m.RecalculateNormals();

        return m;
    }
    private bool ShapeCheck(int xMax, int yMax , int xCurr, int yCurr, string shape, int edgeSize = 0)
    {
        if (shape == "octagon" && edgeSize > 0)
        {
            if (yCurr < edgeSize && xCurr < edgeSize - yCurr)// bootom left (xCurr < 1 && yCurr<1)
            {
                return true;
            }
            if (yCurr < edgeSize && xCurr >= xMax-edgeSize + yCurr)//bottom right (yCurr < 1 && xCurr >= xMax-1)
            {
                return true;
            }
            if (yCurr >= yMax - edgeSize && xCurr < edgeSize - (yMax - yCurr - 1))// top left (yCurr >= yMax - 1 && xCurr < 1)
            {
                return true;
            }
            if (yCurr >= yMax - edgeSize && xCurr >= xMax - edgeSize + (yMax - yCurr - 1))// top right (yCurr >= yMax - 1 && xCurr >= xMax - 1)
            {
                return true;
            }
        }
        return false;
    }
    public void RegenerateMapMaterial()
    {
        if (mapArray == null)
        {
            return;
        }
        for (int i = 0; i < mapSettings.height; i++)
        {
            for (int j = 0; j < mapSettings.width; j++)
            {
                if (ShapeCheck(mapSettings.width, mapSettings.height, j, i, mapSettings.shape, mapSettings.edgeSize)) continue;
                mapArray[j, i].GetComponent<Renderer>().material = SelectMaterial(i, j);
            }
        }
    }
    public void UpdateMap(Noise noise)
    {
        this.noise = noise;
        RegenerateMapMaterial();
    }
    private Material SelectMaterial(int x, int y)
    {
        float value = 0;// noise.BasicNoiseValue(x, y);
        if (value < .2f)
        {
            return texturePack.materials[1];
        }
        else if (value < .6f)
        {
            return texturePack.materials[2];
        }
        else if (value < .8f)
        {
            return texturePack.materials[3];
        }
        else if (value >= .8f)
        {
            return texturePack.materials[4];
        }
        else
        {
            Debug.Log("base");
            return texturePack.materials[0];
        }
    }
    public void RegenerateMapMaterial(float[,] noiseMap)
    {
        if (mapArray == null)
        {
            return;
        }
        for (int i = 0; i < mapSettings.height; i++)
        {
            for (int j = 0; j < mapSettings.width; j++)
            {
                if (ShapeCheck(mapSettings.width, mapSettings.height, j, i, mapSettings.shape, mapSettings.edgeSize)) continue;
                mapArray[j, i].GetComponent<Renderer>().material = SelectMaterial(noiseMap[i, j]);
            }
        }
    }
    private Material SelectMaterial(float noiseVal)
    {
        float value = noiseVal;
        if (value < .2f)
        {
            return texturePack.materials[1];
        }
        else if (value < .6f)
        {
            return texturePack.materials[2];
        }
        else if (value < .8f)
        {
            return texturePack.materials[3];
        }
        else if (value >= .8f)
        {
            return texturePack.materials[4];
        }
        else
        {
            Debug.Log("base");
            return texturePack.materials[0];
        }
    }

    public void DestroyMap()
    {
        if (mapArray == null)
        {
            return;
        }
        for (int i = 0; i < mapSettings.height; i++)
        {
            for (int j = 0; j < mapSettings.width; j++)
            {
                if (ShapeCheck(mapSettings.width, mapSettings.height, j, i, mapSettings.shape, mapSettings.edgeSize)) continue;
                Destroy(mapArray[j, i]);
            }
        }
    }

    public int GetWidht()
    {
        return mapSettings.width;
    }
    public int GetHeight()
    {
        return mapSettings.height;
    }
}
