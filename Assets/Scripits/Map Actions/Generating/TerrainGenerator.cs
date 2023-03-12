using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using CodeMonkey.Utils;

public class TerrainGenerator : MonoBehaviour
{
    public BasicTexturePack texturePack;
    public MapSettings mapSettings;
    public PerlinSettings perlinWorldSettings;
    public PerlinSettings perlinMountinesSettings;
    public GameObject objectOfOrigin;
    public Camera cam;
    public Material unlitMaterial;
    public Gradient gradient;

    private GameObject[,] hexagonalMap = null;
    private Noise noise;
    private GameObject[,] mapArray;
    private Grid<int> grid;

    public float[,] fallOfMap;
    public float[,] noiseMap;
    public bool useFallofMap;
    public int[,] layersMask;
    public long seed;
    public int mapSize = 1;

    public int selectedChunkX;
    public int selectedChunkY;
    public int selectedPositionX;
    public int selectedPositionY;
    public int selectedPositionZ;

    GameObject[,][,] previewMap;


    public float[,][,] worldNoiseMap;

    private void Start()
    {
        seed = perlinWorldSettings.seed;
        noise = new Noise(perlinWorldSettings, mapSettings);
        fallOfMap = FallOffGenerator.GenerateFallOffMap(mapSettings.width, mapSettings.height);
        noise = new Noise(perlinWorldSettings, mapSettings);
        UpdateNoise();
        layersMask = new int[mapSettings.width, mapSettings.height];
        mapArray = new GameObject[mapSettings.height, mapSettings.width];

        previewMap = GeneratePrewievMap(mapSize);
        //GenerateWorld();
        Vector3 gridOrigin;
        if (GameObject.Find("map"))
        {
            gridOrigin = GameObject.Find("map").transform.position;
        }
        else
        {
            gridOrigin = objectOfOrigin.transform.position;
        }
        gridOrigin.x -= mapSettings.cellSize;
        gridOrigin.y -= mapSettings.cellSize;

        grid = new Grid<int>(mapSettings.width, mapSettings.height, mapSettings.cellSize * 2, gridOrigin, "octagon");
    }
    private void OnValidate()
    {
        if (previewMap != null)
        {
            for (int currentChunkHeight = 0; currentChunkHeight < previewMap.GetLength(0); currentChunkHeight++)
            {
                for (int currentChunkWidth = 0; currentChunkWidth < previewMap.GetLength(1); currentChunkWidth++)
                {
                    worldNoiseMap[currentChunkHeight, currentChunkWidth] = GenerateNoise(currentChunkHeight, currentChunkWidth);
                    ApplyFallofMap(currentChunkHeight, currentChunkWidth, previewMap.GetLength(0));

                    for (int currentHeight = 0; currentHeight < previewMap[currentChunkHeight,currentChunkWidth].GetLength(0); currentHeight++)
                    {
                        for (int currentWidth = 0; currentWidth < previewMap[currentChunkHeight, currentChunkWidth].GetLength(1); currentWidth++)
                        {
                            previewMap[currentChunkHeight, currentChunkWidth][currentHeight, currentWidth].GetComponent<MeshRenderer>().material.color = gradient.Evaluate(worldNoiseMap[currentChunkHeight, currentChunkWidth][currentHeight, currentWidth]);
                        }
                    }
                }
            }
        }
                            //UpdateNoise();
                            //seed = perlinWorldSettings.seed;
                            //RegenerateMapMaterial(noiseMap);
                            //RegenerateMapMaterial();                        
    }
    private void Update()
    {
        //if (Input.GetMouseButtonDown(0)) //mouse leftcik
        //{
        //    int x, y, z;
        //    grid.GetXYZ(UtilsClass.GetMouseWorldPosition(Mathf.Abs(cam.transform.position.z)), out x, out y, out z);
        //    PlayerPrefs.SetInt("selectedX", x);
        //    PlayerPrefs.SetInt("selectedY", y);
        //    PlayerPrefs.Save();
        //    if (x < mapSettings.width && x > 0 && y < mapSettings.height && y > 0)
        //    {
        //        GameObject.Find("MenuBrain").GetComponent<MenuUtils>().SetupBlockInfoTab(x, y, mapArray[x, y]);
        //        grid.SetValue(UtilsClass.GetMouseWorldPosition(Mathf.Abs(cam.transform.position.z)), 1);
        //    }
        //}
        //if (Input.GetMouseButtonDown(1))//mouse rightclick
        //{
        //    Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition(Mathf.Abs(cam.transform.position.z))));
        //    int x, y, z;
        //    grid.GetXYZ(UtilsClass.GetMouseWorldPosition(Mathf.Abs(cam.transform.position.z)), out x, out y, out z);
        //    Debug.Log(noiseMap[y,x] + " " + x + " " + y + " " + z);
        //}
    }
    public void ReGenerate()
    {
        seed = perlinWorldSettings.seed;
        DestroyMap();
        UpdateNoise();
        GenerateWorld();
    }
    private void GenerateWorld()
    {
        //Debug.Log("GenerateMap");
        GameObject map;
        map = new GameObject("map");
        if (!GameObject.Find("map"))
        {
            map = new GameObject("map");
            if (GameObject.Find("NewGame") != null)
            {
                map.transform.parent = GameObject.Find("NewGame").transform;
            }
        }

        for (int i = 0; i < mapSettings.height; i++)
        {
            for (int j = 0; j < mapSettings.width; j++)
            {
                if (ShapeCheck(j, i)) continue;
                GameObject plane = new GameObject("Plane" + i + j);
                if (GameObject.Find("map") != null) plane.transform.parent = GameObject.Find("map").transform;
                SetupBlock(plane, i, j, noiseMap);
                mapArray[i, j] = plane;
            }
        }

        var position = map.transform.position;
        position = new Vector3(-mapSettings.width, -mapSettings.height, 1);
        map.transform.position = position;
    }
    private bool ShapeCheck(int xCurr, int yCurr)
    {
        if (mapSettings.shape == "octagon" && mapSettings.edgeSize > 0)
        {
            if (yCurr < mapSettings.edgeSize && xCurr < mapSettings.edgeSize - yCurr)// bootom left (xCurr < 1 && yCurr<1)
            {
                return true;
            }
            if (yCurr < mapSettings.edgeSize && xCurr >= mapSettings.width - mapSettings.edgeSize + yCurr)//bottom right (yCurr < 1 && xCurr >= xMax-1)
            {
                return true;
            }
            if (yCurr >= mapSettings.height - mapSettings.edgeSize && xCurr < mapSettings.edgeSize - (mapSettings.height - yCurr - 1))// top left (yCurr >= yMax - 1 && xCurr < 1)
            {
                return true;
            }
            if (yCurr >= mapSettings.height - mapSettings.edgeSize && xCurr >= mapSettings.width - mapSettings.edgeSize + (mapSettings.height - yCurr - 1))// top right (yCurr >= yMax - 1 && xCurr >= xMax - 1)
            {
                return true;
            }
        }
        return false;
    }
    public void SetupBlock(GameObject plane, int i, int j, float[,] noiseArray)
    {
        //Debug.Log(gradient.Evaluate(29f));
        var position = plane.transform.position;
        position = new Vector3(mapSettings.originPosition.x + mapSettings.cellSize * i * 2, mapSettings.originPosition.y + mapSettings.cellSize * j * 2, 0);
        plane.transform.position = position;
        MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = CreateMesh(mapSettings.cellSize, mapSettings.cellSize);
        MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        //renderer.material.shader = Shader.Find("Particles/Additive");
        renderer.material = SelectMaterial(noiseArray[i,j]);
        plane.AddComponent<BlockBrain>();
        plane.GetComponent<BlockBrain>().originPoint = mapSettings.originPosition;
        plane.GetComponent<BlockBrain>().blockSize = mapSettings.cellSize * 2;
        plane.GetComponent<BlockBrain>().blockNoiseValue = noiseArray[i, j];
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
    public void RegenerateMapMaterial()
    {
        if ( hexagonalMap == null)
        {
            return;
        }
        else
        {
            for (int i = 0; i < mapSettings.height; i++)
            {
                for (int j = 0; j < mapSettings.width; j++)
                {
                    hexagonalMap[i, j].GetComponent<Renderer>().material.color = gradient.Evaluate(noiseMap[i, j]);
                }
            }
        }
        if (mapArray == null)
        {
            //Debug.Log("NULA je mapa vole");
            return;
        }
        for (int i = 0; i < mapSettings.height; i++)
        {
            for (int j = 0; j < mapSettings.width; j++)
            {
                if (ShapeCheck(j, i)) continue;
                mapArray[j, i].GetComponent<Renderer>().material = SelectMaterial(noiseMap[i, j]);
            }
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
                if (ShapeCheck(j, i)) continue;
                mapArray[j, i].GetComponent<Renderer>().material = SelectMaterial(noiseMap[i, j]);
            }
        }
    }
    public void DestroyMap()
    {
        if (mapArray != null)
        {
            for (int i = 0; i < mapSettings.height; i++)
            {
                for (int j = 0; j < mapSettings.width; j++)
                {
                    if (mapArray[j, i] == null) continue;
                    Destroy(mapArray[j, i]);
                }
            }
        }
    }
    public void DestroyPreviewMap()
    {
        if (previewMap != null)
        {
            int size = previewMap.GetLength(0);
            foreach (var obj in previewMap)
            {
                foreach (var ob in obj)
                {
                    Destroy(ob);
                }
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Destroy(GameObject.Find("HexagonalMap" + i + j));
                }
            }
        }
    }
    private Material SelectMaterial(float noiseVal)
    {
        float value = noiseVal;
        if (value < texturePack.values[1])
        {
            return texturePack.materials[1];
        }
        else if (value < texturePack.values[2])
        {
            return texturePack.materials[2];
        }
        else if (value < texturePack.values[3])
        {
            return texturePack.materials[3];
        }
        else if (value <= texturePack.values[4])
        {
            return texturePack.materials[4];
        }
        else if (value >= texturePack.values[5])
        {
            return texturePack.materials[5];
        }
        else
        {
            return texturePack.materials[0];
        }
    }
    private Material SelectMaterial(int x, int y)
    {
        float value = noise.BasicNoiseValue(x, y, perlinWorldSettings, mapSettings);
        if (value < texturePack.values[1])
        {
            return texturePack.materials[1];
        }
        else if (value < texturePack.values[2])
        {
            return texturePack.materials[2];
        }
        else if (value < texturePack.values[3])
        {
            return texturePack.materials[3];
        }
        else if (value <= texturePack.values[4])
        {
            return texturePack.materials[4];
        }
        else if (value >= texturePack.values[5])
        {
            return texturePack.materials[5];
        }
        else
        {
            return texturePack.materials[0];
        }
    }
    public void UpdateNoise()
    {
        if (noise == null) return;
        noiseMap = noise.GenerateNoiseMap(perlinWorldSettings, mapSettings,fallOfMap, seed, false);
        //noiseMap = noise.GenerateMountinesMap(perlinMountinesSettings, mapSettings, noiseMap, texturePack, seed);
    }
    public float[,] GenerateNoise(int posX, int posY)
    {
        //PerlinSettings currentNoiseSettings = perlinWorldSettings;
        //currentNoiseSettings.offsetY += posX * mapSettings.width;
        //currentNoiseSettings.offsetX +=  posY *mapSettings.height;
        //perlinWorldSettings.offsetY++;
        //perlinWorldSettings.offsetX++;
        if (noise == null)
        {
            Debug.Log("No noise found");
            return null;
        }
        return noise.GenerateNoiseMap(perlinWorldSettings, mapSettings, fallOfMap, 10, false, posX, posY);//seed boiiiiiiii
    }
    public void CopyNoiseSettings(PerlinSettings s1, PerlinSettings s2)
    {
        s1.active = s2.active;
        s1.frequency = s2.frequency;
        s1.lacunarity = s2.lacunarity;
        s1.octaves = s2.octaves;
        s1.offsetX = s2.offsetX;
        s1.offsetY = s2.offsetY;
        s1.persistance = s2.persistance;
        s1.scale = s2.scale;
        s1.seed = s2.seed;
        s1.strenght = s2.strenght;
    }
    public void CopyMapSettings(MapSettings s1, MapSettings s2)
    {
        s1.width = s2.width;
        s1.height = s2.height;
        s1.edgeSize = s2.edgeSize;
        s1.shape = s2.shape;
        s1.cellSize = s2.cellSize;
        s1.originPosition = s2.originPosition;
    }
    public bool CompareNoiseSettings(PerlinSettings s1, PerlinSettings s2)
    {
        if (s1.active != s2.active) return true;
        if (s1.frequency != s2.frequency) return true;
        if (s1.lacunarity != s2.lacunarity) return true;
        if (s1.octaves != s2.octaves) return true;
        if (s1.offsetX != s2.offsetX) return true;
        if (s1.offsetY != s2.offsetY) return true;
        if (s1.persistance != s2.persistance) return true;
        if (s1.scale != s2.scale) return true;
        if (s1.seed != s2.seed) return true;
        if (s1.strenght != s2.strenght) return true;
        return false;
    }
    public GameObject GenerateHexagon(Vector3 position, int size, float longDiagonal = 1, int currentHeight = 0, int currentWidth = 0, int currentChunkHeight = 0, int currentChunkWidth = 0, float gradientValue = 0f)
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
        hexagon.GetComponent<MeshRenderer>().material = unlitMaterial;
        hexagon.GetComponent<MeshRenderer>().material.color = gradient.Evaluate(gradientValue);
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
        hexagon.layer = 10;

        return hexagon;
    }
    public GameObject GenerateHalfHexagon(Vector3 position,int side, int size, float longDiagonal = 1)
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
        hexagon.GetComponent<MeshRenderer>().material = unlitMaterial;
        hexagon.transform.position = position;
        hexagon.transform.localScale = new Vector3(size, size, 1);

        return hexagon;
    }
    public GameObject[,] GenerateHexagonalMap(int offSetX, int offSetY, int currentChunkHeight, int currentChunkWidth, int mapSize)
    {
        GameObject[,] map = null;
        GameObject mapObject = GameObject.Find("Map");
        GameObject HexagonalMap = new GameObject("HexagonalMap" + currentChunkHeight  + currentChunkWidth);
        int width = mapSettings.width;
        int height = mapSettings.height;
        float size = mapSettings.cellSize * 2;
        //int offSetX = -800;
        //int offSetY = -500;
        map = new GameObject[height, width];

        for (int currentHeight = 0; currentHeight < height; currentHeight++)
        {
            for (int currentWidth = 0; currentWidth < width; currentWidth++)
            {
                float currentX = ((size + size / 2) * currentWidth) + ((size / 4 * 3) * (currentHeight % 2));
                float currentY;
                Vector3 currentPosition;
                if (currentHeight > 1)
                {
                    currentY = (size * currentHeight) - ((size / 2) * (currentHeight % 2)) - size * (currentHeight / 2);
                }
                else
                {
                    currentY = (size * currentHeight) - ((size / 2) * (currentHeight % 2));
                }
                currentPosition = new Vector3(currentX, currentY, 1);
                map[currentHeight, currentWidth] = GenerateHexagon(currentPosition, 1, size, currentHeight, currentWidth, currentChunkHeight, currentChunkWidth, worldNoiseMap[currentChunkHeight, currentChunkWidth][currentHeight, currentWidth]);
                map[currentHeight, currentWidth].transform.parent = HexagonalMap.transform; //Basic
            }
        }
        HexagonalMap.transform.position = new Vector3(offSetX, offSetY);
        HexagonalMap.transform.parent = mapObject.transform;

        return map;
    }
    public GameObject[,][,] GeneratePrewievMap(int mapSize)
    {
        GameObject[,][,] previewMap = new GameObject[mapSize,mapSize][,];
        worldNoiseMap = new float[mapSize,mapSize][,];
        MapSettings localMapSettings = new MapSettings();
        CopyMapSettings(localMapSettings, mapSettings);        
        localMapSettings.width *= 3;
        localMapSettings.height *= 3;

        //noiseMap = noise.GenerateNoiseMap(perlinWorldSettings, localMapSettings, fallOfMap, texturePack, seed, false);
        //Debug.Log(noiseMap.GetLength(0) + " " + noiseMap.GetLength(1) + "      " + localMapSettings.height + " " + localMapSettings.width);
        for (int currentHeightChunk = 0; currentHeightChunk < mapSize; currentHeightChunk++)
        {
            for (int currentWidthChunk = 0; currentWidthChunk < mapSize; currentWidthChunk++)
            {
                worldNoiseMap[currentHeightChunk, currentWidthChunk] = GenerateNoise(currentHeightChunk, currentWidthChunk);
                ApplyFallofMap(currentHeightChunk, currentWidthChunk, mapSize);
                previewMap[currentHeightChunk,currentWidthChunk] = GenerateHexagonalMap(1500 * currentWidthChunk, 1000 * currentHeightChunk, currentHeightChunk, currentWidthChunk, mapSize);
            }
        }
        GameObject.Find("Map").transform.localScale = new Vector3(.1f, .1f);
        GameObject.Find("Map").transform.position = new Vector3(0, 0, 0);//(mapSettings.width * mapSize, (mapSettings.height * mapSize) / 2);
        return previewMap;
    }
    public void ApplyFallofMap(int currentChunkHeight, int currentChunkWidth, int mapSize)
    {        
        if (mapSize == 1)
        {
            worldNoiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapSettings.height, mapSettings.width));
            return;
        }
        if (currentChunkHeight == 0) // bot
        {
            if (currentChunkWidth == 0)//left && bot
            {
                worldNoiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapSettings.height, mapSettings.width, -1, -1, 0));
            }
            else if (currentChunkWidth == mapSize - 1)//right && bot
            {
                worldNoiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapSettings.height, mapSettings.width, -1, -1, 1));
            }
            else//bot
            {
                worldNoiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapSettings.height, mapSettings.width, 0, 0));
            }
        }
        else if (currentChunkHeight == mapSize - 1) // top
        {
            if (currentChunkWidth == 0)//left && top
            {
                worldNoiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapSettings.height, mapSettings.width, -1, -1, 2));
            }
            else if (currentChunkWidth == mapSize - 1)//right && top
            {
                worldNoiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapSettings.height, mapSettings.width, -1, -1, 3));
            }
            else//top
            {
                worldNoiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapSettings.height, mapSettings.width, 0, 1));
            }
        }
        else
        {
            if (currentChunkWidth == 0)//left
            {
                worldNoiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapSettings.height, mapSettings.width, 1, 0));
            }
            else if (currentChunkWidth == mapSize - 1)//right
            {
                worldNoiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapSettings.height, mapSettings.width, 1, 1));
            }
            else
            {
            }
        }
    }
    public float[,] ApplyFallofchunk(int currentChunkHeight, int currentChunkWidth, float[,] fallofMap)
    {
        float[,] currentMap = worldNoiseMap[currentChunkHeight, currentChunkWidth];
        for (int i = 0; i < mapSettings.height; i++)
        {
            for (int j = 0; j < mapSettings.width; j++)
            {
                currentMap[i, j] = Mathf.Clamp01(currentMap[i, j] - fallofMap[j, i]);
            }
        }
        return currentMap;
    }
    public int GetMaptSize()
    {
        return previewMap.GetLength(0);
    }
    public void RegeneratePreviewMap()
    {
        DestroyPreviewMap();
        GameObject.Find("Map").transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("Map").transform.position = new Vector3(0, 0, 0);
        GameObject.Find("MapCamera").GetComponent<MapCameraHandler>().SetupMapCamera();
        previewMap = GeneratePrewievMap(mapSize);
    }
    public void SetMapSize(GameObject size)
    {
        Debug.Log(size.GetComponent<Text>().text);
        switch (size.GetComponent<Text>().text)
        {
            case "Small":
                mapSize = 2;
                return;
            case "Medium":
                mapSize = 3;
                return;
            case "Large":
                mapSize = 4;
                return;
            default:
                mapSize = 1;
                return;
        }
    }
}
