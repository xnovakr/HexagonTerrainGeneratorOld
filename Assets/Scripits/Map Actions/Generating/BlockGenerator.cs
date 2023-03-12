using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockGenerator
{
    public static GameObject GenerateRectangle(Vector3 position, int size, int currentHeight = 0, int currentWidth = 0, float gradientValue = 0f)
    {
        string name = currentHeight.ToString("000") + " " + currentWidth.ToString("000");
        GameObject rectangle = new GameObject("Rectangle" + name, typeof(MeshFilter), typeof(MeshRenderer));
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        //vertices
        {
            vertices[0] = new Vector3(0, 0);
            vertices[1] = new Vector3(size, 0);
            vertices[2] = new Vector3(size, size / 2f);
            vertices[3] = new Vector3(0, size / 2f);
        }
        //uv
        {
            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(1, 0);
            uv[2] = new Vector2(1, 1 / 2f);
            uv[3] = new Vector2(0, 1 / 2f);
        }
        //triangles
        {
            triangles[0] = 0;
            triangles[1] = 2;
            triangles[2] = 1;

            triangles[3] = 0;
            triangles[4] = 3;
            triangles[5] = 2;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        rectangle.GetComponent<MeshFilter>().mesh = mesh;
        rectangle.GetComponent<MeshRenderer>().material = null;
        //rectangle.GetComponent<MeshRenderer>().material.color = gradient.Evaluate(gradientValue);/////////////////////////////////////// farba bloku
        rectangle.AddComponent<MeshCollider>();
        rectangle.GetComponent<MeshCollider>().sharedMesh = mesh;
        rectangle.GetComponent<MeshCollider>().cookingOptions = 0;
        rectangle.transform.position = position;
        rectangle.transform.localScale = new Vector3(size, size, 1);
        //rectangle.layer = 10;

        return rectangle;
    }
    public static GameObject GenerateHexagon(Vector3 position, int size, float longDiagonal = 1, int currentChunkHeight = 0, int currentChunkWidth = 0, int currentHeight = 0, int currentWidth = 0, int currentDepth = 0, float gradientValue = 0f, bool map = false, Material material = null, Gradient gradient = null)
    {
        string name = currentHeight.ToString("000") + " " + currentWidth.ToString("000");
        GameObject hexagon = new GameObject("Hexagon" + name, typeof(MeshFilter), typeof(MeshRenderer));
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[7];
        Vector2[] uv = new Vector2[7];
        int[] triangles = new int[18];
        //float edgeLenght = .5f, float shortDiagonal = .866f, 
        float edgeLenght, shortDiagonal;
        edgeLenght = longDiagonal / 2f;
        shortDiagonal = Mathf.Sqrt(3) * edgeLenght;

        //vertices
        {
            vertices[0] = new Vector3(-edgeLenght / 2f, shortDiagonal / 2f);
            vertices[1] = new Vector3(edgeLenght / 2f, shortDiagonal / 2f);
            vertices[2] = new Vector3(edgeLenght, 0);
            vertices[3] = new Vector3(edgeLenght / 2f, -shortDiagonal / 2f);
            vertices[4] = new Vector3(-edgeLenght / 2f, -shortDiagonal / 2f);
            vertices[5] = new Vector3(-edgeLenght, 0);
            vertices[6] = new Vector3(0, 0);
        }
        //uv
        {
            uv[0] = new Vector2(-edgeLenght / 2f, shortDiagonal / 2f);
            uv[1] = new Vector2(edgeLenght / 2f, shortDiagonal / 2f);
            uv[2] = new Vector2(edgeLenght, 0);
            uv[3] = new Vector2(edgeLenght / 2f, -shortDiagonal / 2f);
            uv[4] = new Vector2(-edgeLenght / 2f, -shortDiagonal / 2f);
            uv[5] = new Vector2(-edgeLenght, 0);
            uv[6] = new Vector2(0, 0);
        }
        //triangles
        {
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 6;

            triangles[3] = 1;
            triangles[4] = 2;
            triangles[5] = 6;

            triangles[6] = 2;
            triangles[7] = 3;
            triangles[8] = 6;

            triangles[9] = 3;
            triangles[10] = 4;
            triangles[11] = 6;

            triangles[12] = 4;
            triangles[13] = 5;
            triangles[14] = 6;

            triangles[15] = 5;
            triangles[16] = 0;
            triangles[17] = 6;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        hexagon.GetComponent<MeshFilter>().mesh = mesh;
        if (map)
        {
            hexagon.GetComponent<MeshRenderer>().material = material;
            hexagon.GetComponent<MeshRenderer>().material.color = gradient.Evaluate(gradientValue);
            hexagon.layer = 10;
        }
        hexagon.AddComponent<MeshCollider>();
        hexagon.GetComponent<MeshCollider>().sharedMesh = hexagon.GetComponent<MeshFilter>().mesh;
        hexagon.GetComponent<MeshCollider>().cookingOptions = 0;
        hexagon.transform.position = position;
        hexagon.transform.localScale = new Vector3(size, size, 1);
        hexagon.AddComponent<SpawnPicker>();
        hexagon.GetComponent<SpawnPicker>().perlinValue = gradientValue;
        hexagon.GetComponent<SpawnPicker>().selectedChunkX = currentChunkWidth;
        hexagon.GetComponent<SpawnPicker>().selectedChunkY = currentChunkHeight;
        hexagon.GetComponent<SpawnPicker>().selectedPositionX = currentWidth;
        hexagon.GetComponent<SpawnPicker>().selectedPositionY = currentHeight;
        hexagon.GetComponent<SpawnPicker>().selectedPositionZ = 0;

        return hexagon;
    }
    public static GameObject GenerateHalfHexagon(Vector3 position, int side, int size, float longDiagonal = 1)
    {
        //GenerateHexagon(new Vector3(0, 0, 1), 30); //Basic
        //GenerateHalfHexagon(new Vector3(50, 0, 1), 0, 30); //top half
        //GenerateHalfHexagon(new Vector3(100, 0, 1), 1, 30); //bot half
        //GenerateHalfHexagon(new Vector3(150, 0, 1), 2, 30); //left half
        //GenerateHalfHexagon(new Vector3(200, 0, 1), 3, 30); //right half
        // if side is equal to: 0 its top; 1 its bottom; 2 its left; 3 its right
        GameObject hexagon = new GameObject("Hexagon", typeof(MeshFilter), typeof(MeshRenderer));
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[5];
        Vector2[] uv = new Vector2[5];
        int[] triangles = new int[9];
        float edgeLenght, shortDiagonal;
        edgeLenght = longDiagonal / 2f;
        shortDiagonal = Mathf.Pow(3, 1f / 3f) * edgeLenght;

        //vertices
        {
            if (side == 0 || side == 2)
            {
                vertices[0] = new Vector3(-edgeLenght / 2f, shortDiagonal / 2f);
                vertices[1] = new Vector3(edgeLenght / 2f, shortDiagonal / 2f);
                vertices[2] = new Vector3(edgeLenght, 0);
                vertices[3] = new Vector3(-edgeLenght, 0);
            }
            else if (side == 1 || side == 3)
            {
                vertices[0] = new Vector3(-edgeLenght / 2f, -shortDiagonal / 2f);
                vertices[1] = new Vector3(edgeLenght / 2f, -shortDiagonal / 2f);
                vertices[2] = new Vector3(edgeLenght, 0);
                vertices[3] = new Vector3(-edgeLenght, 0);
            }
            vertices[4] = new Vector3(0, 0);
        }
        //uv
        {
            uv[0] = new Vector2(-edgeLenght / 2f, shortDiagonal / 2f);
            uv[1] = new Vector2(edgeLenght / 2f, shortDiagonal / 2f);
            uv[2] = new Vector2(edgeLenght, 0);
            //uv[3] = new Vector2(edgeLenght / 2f, -shortDiagonal / 2f);
            //uv[4] = new Vector2(-edgeLenght / 2f, -shortDiagonal / 2f);
            uv[3] = new Vector2(-edgeLenght, 0);
            uv[4] = new Vector2(0, 0);
        }
        //triangles
        {

            if (side == 0 || side == 2)
            {
                triangles[0] = 0;
                triangles[1] = 1;
                triangles[2] = 4;

                triangles[3] = 1;
                triangles[4] = 2;
                triangles[5] = 4;

                triangles[6] = 3;
                triangles[7] = 0;
                triangles[8] = 4;
            }
            else if (side == 1 || side == 3)
            {
                triangles[0] = 0;
                triangles[1] = 3;
                triangles[2] = 4;

                triangles[3] = 1;
                triangles[4] = 0;
                triangles[5] = 4;

                triangles[6] = 2;
                triangles[7] = 1;
                triangles[8] = 4;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        if (side == 2 || side == 3)
        {
            var rotation = hexagon.transform.rotation;
            rotation.eulerAngles = new Vector3(0, 0, 90);
            hexagon.transform.rotation = rotation;
        }
        hexagon.GetComponent<MeshFilter>().mesh = mesh;
        hexagon.GetComponent<MeshRenderer>().material = null;
        hexagon.transform.position = position;
        hexagon.transform.localScale = new Vector3(size, size, 1);

        return hexagon;
    }
    
    public static GameObject Generate3DHexagon(Vector3 position, int size, float longDiagonal = 1, int currentHeight = 0, int currentWidth = 0, float gradientValue = 0f)
    {
        string name = currentHeight.ToString("000") + " " + currentWidth.ToString("000");
        GameObject hexagon = new GameObject("Hexagon" + name);
        GameObject surface = GenerateHexagon(new Vector3(0, 0), size);
        GameObject bot = GenerateRectangle(new Vector3(0, 0), size);
        GameObject botLeft = GenerateRectangle(new Vector3(0, 0), size);
        GameObject botRight = GenerateRectangle(new Vector3(0, 0), size);
        GameObject top = GenerateRectangle(new Vector3(0, 0), size);
        GameObject topLeft = GenerateRectangle(new Vector3(0, 0), size);
        GameObject topRight = GenerateRectangle(new Vector3(0, 0), size);

        surface.transform.parent = hexagon.transform;
        bot.transform.parent = hexagon.transform;
        botLeft.transform.parent = hexagon.transform;
        botRight.transform.parent = hexagon.transform;
        top.transform.parent = hexagon.transform;
        topLeft.transform.parent = hexagon.transform;
        topRight.transform.parent = hexagon.transform;

        bot.transform.name = "bot";
        botLeft.transform.name = "botLeft";
        botRight.transform.name = "botRight";
        top.transform.name = "top";
        topLeft.transform.name = "topLeft";
        topRight.transform.name = "topRight";

        bot.transform.position = new Vector3(-.25f, .433f, 1);
        botLeft.transform.position = new Vector3(.25f, .433f, 1);
        botRight.transform.position = new Vector3(-.5f, 0, 1);
        top.transform.position = new Vector3(.25f, -.433f, 1);
        topLeft.transform.position = new Vector3(.5f, 0, 1);
        topRight.transform.position = new Vector3(-.25f, -.433f, 1);

        bot.transform.eulerAngles = new Vector3(90, 90);
        botLeft.transform.eulerAngles = new Vector3(150, 90);
        botRight.transform.eulerAngles = new Vector3(30, 90);
        top.transform.eulerAngles = new Vector3(-90, 90);
        topLeft.transform.eulerAngles = new Vector3(-150, 90);
        topRight.transform.eulerAngles = new Vector3(-30, 90);

        hexagon.transform.position = position;
        return hexagon;
    }
    public static GameObject SetupCubePrefab(float[,][,] noiseMap, Grid<BlockInfo> blockGrid, GameObject cubePrefab,MapSettings mapSettings, BasicTexturePack texturePack, GameObject parent, float posX, float posY, int i, int j, float noiseValue, int height = 1)
    {
        GameObject cube;
        cube = GameObject.Instantiate(cubePrefab, new Vector3(posX, posY, -mapSettings.cellSize * height), Quaternion.identity);
        cube.transform.parent = parent.transform;
        cube.GetComponent<Renderer>().material = WorldGenerator.SelectMaterial(noiseValue, texturePack);
        cube.name = "cube " + posX + " " + posY;
        cube.AddComponent<BlockInfo>();
        //mapArray[i, j] = cube;

        cube.GetComponent<BlockInfo>().blockSize = mapSettings.cellSize;
        cube.GetComponent<BlockInfo>().blockNoiseValue = noiseValue;
        {//walkableSetup
            if (noiseMap[0, 0][i, j] < texturePack.values[1])
            {
                cube.GetComponent<BlockInfo>().walkable = false;
            }
            else if (noiseMap[0, 0][i, j] < texturePack.values[3])
            {
                cube.GetComponent<BlockInfo>().walkable = true;
            }
            if (noiseMap[0, 0][i, j] > texturePack.values[3])
            {
                cube.GetComponent<BlockInfo>().walkable = false;
            }
            blockGrid.SetValue(j, i, cube.GetComponent<BlockInfo>());
        }
        return cube;
    }
}
