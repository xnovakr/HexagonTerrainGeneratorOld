using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using CodeMonkey.Utils;

public class WorldGenerationHandler : MonoBehaviour
{
    public BasicTexturePack texturePack;
    public MapSettings mapSettings;
    public PerlinSettings perlinWorldSettings;
    //private PerlinSettings perlinLocalSettings;
    public GameObject objectOfOrigin;
    public Camera cam;
    public GameObject pauseButtons;
    public GameObject camera2D;
    public GameObject camera3D;
    public Grid<BlockInfo> blockGrid;

    private Noise noise;
    public GameObject cubePrefab;
    //private GameObject[,] mapArray;
    public GameObject[][,] mapsArray;
    public GameObject[,][,] previewMap;
    public GameObject[,][][,] world;
    public Hexagon3Dproperties[,][][,] potentialWorld;

    public float[,] fallOfMap;
    //public float[,] noiseWorldMap;
    public float[,][,] noiseMap;
    public bool useFallofMap;
    public int[,] layersMask;
    public int mapSize;


    public Material unlitMaterial;
    public Gradient gradient;

    public int selectedChunkX;
    public int selectedChunkY;
    public int selectedPositionX;
    public int selectedPositionY;
    public int selectedPositionZ;

    public GameObject World;
    public GameObject Map;

    public BlockNodes[,] mapNodes;

    private void Start()
    {
        LoadSettings();
        Debug.Log(mapSettings.width + " 0 " + mapSettings.height);
        mapNodes = new BlockNodes[mapSettings.height, mapSettings.width];
        //mapSize = 3;
        noiseMap = NoiseGenerator.GenerateNoiseWorldMap(mapSize, perlinWorldSettings, mapSettings, fallOfMap);
        noise = new Noise(perlinWorldSettings, mapSettings);
        previewMap = MapGenerator.GeneratePrewievMap(mapSettings, noiseMap, mapSize, unlitMaterial, gradient);
        Debug.Log("Mapsize is: " + mapSize);

        Map.SetActive(false);
        //Map.SetActive(false);
        //previewMap[selectedChunkX, selectedChunkY][selectedPositionY, selectedPositionX].GetComponent<MeshRenderer>().material = unlitMaterial;

        mapSettings.cellSize = 1;////////////////////////////////////////////////////////////////////////////

        mapsArray = new GameObject[5][,];
        for (int i = 0; i < 5; i++)
        {// 0 vrch hory      1 hora         2 zem       3 podzemie1     4 podzemie2 
            mapsArray[i] = new GameObject[mapSettings.height, mapSettings.width];
        }
        world = new GameObject[mapSize,mapSize][][,];
        world[0, 0] = new GameObject[5][,];
        potentialWorld = new Hexagon3Dproperties[mapSize,mapSize][][,];
        blockGrid = new Grid<BlockInfo>(mapSettings.width, mapSettings.height, mapSettings.cellSize, mapSettings.originPosition);
        GridHexagonal<bool> hexGrid = new GridHexagonal<bool>(10, 5, 1, new Vector3(0,0,0));

        //fallOfMap = FallOffGenerator.GenerateFallOffMap(mapSettings.width, mapSettings.height);
        //noise = new Noise(perlinWorldSettings, mapSettings);
        //UpdateNoise();
        //noiseWorldMap = noiseMap;
        //noiseMap = CreateLocalNoiseMap();
        //mapArray = new GameObject[mapSettings.height, mapSettings.width];
        //GenerateWorld(noiseMap[0, 0]);
        //Generate3DHexagon(new Vector3(0, 0), 1);
        //world[0,0] = NewWorldTryout(0);
        //GenerateWholeNewWorld();
        potentialWorld = WorldGenerator.GeneratePotentialMap(mapSettings, mapSize, noiseMap, blockGrid, texturePack, mapNodes);
        WorldGenerator.ApplyPotentialMap(potentialWorld, world, noiseMap, texturePack);

        //DrawNodes();

        //Oprimalise();
        //GenerateWholeWorld();
        //World.SetActive(false);
        if (PlayerPrefs.HasKey("SaveIndex"))
        {
            Debug.Log("Loading from save");
            LoadWorld(PlayerPrefs.GetInt("SaveIndex"));
            PlayerPrefs.DeleteKey("SaveIndex");
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
        else
        {
            //Debug.Log("Creating new and saving");
            //GenerateWorld(noiseMap);
            //mapsArray[2] = mapArray;//zem
        }
        //if (false)
        //{
        //    int3[] moveToTest = new int3[13] { new int3(10, 10, 0), new int3(10, 30, 0), new int3(20, 20, 0), new int3(10, 10, 0), new int3(10, 30, 0), new int3(20, 20, 0), new int3(10, 10, 0), new int3(10, 30, 0), new int3(20, 20, 0), new int3(10, 10, 0), new int3(10, 30, 0), new int3(20, 20, 0), new int3(49,49,0) };

        //    GameObject.Find("Basic Motions Dummy").GetComponent<MovePositionNodes>().SetMovePosition(moveToTest, moveToTest[moveToTest.Length-1]);
        //}
    }
    //public float[,] CreateLocalNoiseMap()
    //{
    //    int x, y;
    //    x = PlayerPrefs.GetInt("selectedX");
    //    y = PlayerPrefs.GetInt("selectedY");
    //    perlinLocalSettings = perlinWorldSettings;
    //    perlinLocalSettings.offsetX = x * mapSettings.width;
    //    perlinLocalSettings.offsetY = y * mapSettings.height;
    //    perlinLocalSettings.scale = 60;
    //    return noise.GenerateNoiseMap(perlinLocalSettings, mapSettings, fallOfMap, perlinWorldSettings.seed, false);
    //}
    //private bool ShapeCheck(int yCurr, int xCurr)
    //{
    //    if (mapSettings.shape == "octagon" && mapSettings.edgeSize > 0)
    //    {
    //        if (yCurr < mapSettings.edgeSize && xCurr < mapSettings.edgeSize - yCurr)// bootom left (xCurr < 1 && yCurr<1)
    //        {
    //            return true;
    //        }
    //        if (yCurr < mapSettings.edgeSize && xCurr >= mapSettings.width - mapSettings.edgeSize + yCurr)//bottom right (yCurr < 1 && xCurr >= xMax-1)
    //        {
    //            return true;
    //        }
    //        if (yCurr >= mapSettings.height - mapSettings.edgeSize && xCurr < mapSettings.edgeSize - (mapSettings.height - yCurr - 1))// top left (yCurr >= yMax - 1 && xCurr < 1)
    //        {
    //            return true;
    //        }
    //        if (yCurr >= mapSettings.height - mapSettings.edgeSize && xCurr >= mapSettings.width - mapSettings.edgeSize + (mapSettings.height - yCurr - 1))// top right (yCurr >= yMax - 1 && xCurr >= xMax - 1)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    //private Material SelectMaterial(float noiseVal)
    //{
    //    if (noiseVal < texturePack.values[1])
    //    {
    //        return texturePack.materials[1];
    //    }
    //    else if (noiseVal < texturePack.values[2])
    //    {
    //        return texturePack.materials[2];
    //    }
    //    else if (noiseVal < texturePack.values[3])
    //    {
    //        return texturePack.materials[3];
    //    }
    //    else if (noiseVal <= texturePack.values[4])
    //    {
    //        return texturePack.materials[4];
    //    }
    //    else if (noiseVal >= texturePack.values[5])
    //    {
    //        return texturePack.materials[5];
    //    }
    //    else
    //    {
    //        return texturePack.materials[0];
    //    }
    //}
    //public void UpdateNoise()
    //{
    //    if (noise == null) return;
    //    //noiseMap = noise.GenerateNoiseMap(perlinWorldSettings, mapSettings, fallOfMap, texturePack, perlinWorldSettings.seed, false);
    //}
    public void LoadSettings()
    {
        mapSize = PlayerPrefs.GetInt("MapSize");

        mapSettings.width =PlayerPrefs.GetInt("mapWidth");
        mapSettings.height = PlayerPrefs.GetInt("mapHeight");
        mapSettings.edgeSize = PlayerPrefs.GetInt("mapEdge");
        mapSettings.cellSize = PlayerPrefs.GetFloat("mapCellSize");
        mapSettings.shape = PlayerPrefs.GetString("mapShape");

        perlinWorldSettings.octaves = PlayerPrefs.GetInt("perlinOcraves");
        perlinWorldSettings.lacunarity = PlayerPrefs.GetFloat("PerlinLacunarity");
        perlinWorldSettings.weightMultiplier = PlayerPrefs.GetFloat("PerlinWeightMultiplier");
        perlinWorldSettings.persistance = PlayerPrefs.GetFloat("PerlinPersistance");
        perlinWorldSettings.strenght = PlayerPrefs.GetFloat("PerlinStrenght");
        perlinWorldSettings.frequency = PlayerPrefs.GetFloat("PerlinFrequency");
        perlinWorldSettings.scale = PlayerPrefs.GetFloat("PerlinScale");
        perlinWorldSettings.offsetX = PlayerPrefs.GetFloat("PerlinOffsetX");
        perlinWorldSettings.offsetY = PlayerPrefs.GetFloat("PerlinOffsetY");
        perlinWorldSettings.seed = PlayerPrefs.GetInt("PerlinSeed");

        selectedChunkX = PlayerPrefs.GetInt("selectedChunkX");
        selectedChunkY = PlayerPrefs.GetInt("selectedChunkY");
        selectedPositionX = PlayerPrefs.GetInt("selectedPositionX");
        selectedPositionY = PlayerPrefs.GetInt("selectedPositionY");
        selectedPositionZ = PlayerPrefs.GetInt("selectedPositionZ");
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("MapSize", mapSize);

        PlayerPrefs.SetInt("mapWidth", mapSettings.width);
        PlayerPrefs.SetInt("mapHeight", mapSettings.height);
        PlayerPrefs.SetInt("mapEdge", mapSettings.edgeSize);
        PlayerPrefs.SetFloat("mapCellSize", mapSettings.cellSize);
        PlayerPrefs.SetString("mapShape", mapSettings.shape);

        PlayerPrefs.SetInt("perlinOcraves", perlinWorldSettings.octaves);
        PlayerPrefs.SetFloat("PerlinLacunarity", perlinWorldSettings.lacunarity);
        PlayerPrefs.SetFloat("PerlinWeightMultiplier", perlinWorldSettings.weightMultiplier);
        PlayerPrefs.SetFloat("PerlinPersistance", perlinWorldSettings.persistance);
        PlayerPrefs.SetFloat("PerlinStrenght", perlinWorldSettings.strenght);
        PlayerPrefs.SetFloat("PerlinFrequency", perlinWorldSettings.frequency);
        PlayerPrefs.SetFloat("PerlinScale", perlinWorldSettings.scale);
        PlayerPrefs.SetFloat("PerlinOffsetX", perlinWorldSettings.offsetX);
        PlayerPrefs.SetFloat("PerlinOffsetY", perlinWorldSettings.offsetY);
        PlayerPrefs.SetInt("PerlinSeed", perlinWorldSettings.seed);

        PlayerPrefs.SetInt("selectedChunkX", selectedChunkX);
        PlayerPrefs.SetInt("selectedChunkY", selectedChunkY);
        PlayerPrefs.SetInt("selectedPositionX", selectedPositionX);
        PlayerPrefs.SetInt("selectedPositionY", selectedPositionY);
        PlayerPrefs.SetInt("selectedPositionZ", selectedPositionZ);
    }
    //public void DeleteWorldSection(int index)
    //{
    //    foreach (GameObject child in mapsArray[index])
    //    {
    //        if (child != null)
    //        {
    //            child.SetActive(!child.activeSelf);
    //        }
    //    }
    //}
    public void DeleteBlock(int index, int x, int y)
    {
        Destroy(mapsArray[index][x, y]);
    }
    public void SaveWorld(int index)
    {
        Debug.Log("saving with index" + index);
        SaveLoad.SaveGame(mapsArray, perlinWorldSettings, mapSettings, index);
    }
    public void DeleteSave(int index)
    {
        SaveLoad.DeleteSave(index);
    }
    public void LoadWorld(int index)
    {
        Debug.Log("loading with index" + index);
        if (!SaveLoad.CheckSave(index)) return;
        if (GameObject.Find("map")) WorldGenerator.DeleteWorld(GameObject.Find("World").transform);

        perlinWorldSettings = SaveLoad.LoadPerlinData(index);
        mapSettings = SaveLoad.LoadMapData(index);
        LoadMapsFromFloats(SaveLoad.LoadBlockData(index));
    }
    public void LoadMapsFromFloats(float[][,] blockData)//broken
    {
        for (int layer = 0; layer < blockData.Length; layer++)
        {
            //WorldGenerator.GenerateWorld(blockData[layer], mapSettings.width, mapSettings.height);
        }
        mapsArray[0] = new GameObject[mapSettings.height, mapSettings.width];//vrch hory
        mapsArray[1] = new GameObject[mapSettings.height, mapSettings.width];//hora
    }



    //public void ApplyMaterialToObject(GameObject obj, Material material)
    //{
    //    if (obj.transform.childCount > 0)
    //    {
    //        //Debug.Log("kids hehe");
    //        foreach (Transform child in obj.transform)
    //        {
    //            child.gameObject.GetComponent<MeshRenderer>().material = material;
    //        }
    //    }
    //    else if (obj.GetComponent<MeshRenderer>())
    //    {
    //        //Debug.Log("4everlonely");
    //        obj.GetComponent<MeshRenderer>().material = material;
    //    }
    //}

    //public void Oprimalise()
    //{
    //    for (int chunkY = 0; chunkY < world.GetLength(0); chunkY++)
    //    {
    //        for (int chunkX = 0; chunkX < world.GetLength(1); chunkX++)
    //        {
    //            for (int layer = 0; layer < world[chunkY, chunkX].Length; layer++)
    //            {
    //                for (int posY = 0; posY < world[chunkY, chunkX][layer].GetLength(0); posY++)
    //                {
    //                    for (int posX = 0; posX < world[chunkY, chunkX][layer].GetLength(1); posX++)
    //                    {
    //                        if (world[chunkY, chunkX][layer][posY, posX]) world[chunkY, chunkX][layer][posY, posX].SetActive(WorldGenerator.CheckVisibility(world, chunkX, chunkY, layer, posX, posY));
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
    public Node GetNodeFromPosition(Vector3 position)
    {
        Node node = null;
        float distance = -1;
        for (int i = 0; i < mapSettings.height; i++)
        {
            for (int j = 0; j < mapSettings.width; j++)
            {
                for (int k = 0; k < mapNodes[i, j].GetNodes().Length; k++)
                {
                    if (distance == -1)
                    {
                        node = mapNodes[i, j].GetNode(k);
                        distance = Mathf.Abs(position.x - node.position.x) + Mathf.Abs(position.y - node.position.y);
                    }
                    Node newNode = mapNodes[i, j].GetNode(k);
                    float newDistance = Mathf.Abs(position.x - newNode.position.x) + Mathf.Abs(position.y - newNode.position.y);
                    if (distance > newDistance)
                    {
                        node = mapNodes[i, j].GetNode(k);
                        distance = Mathf.Abs(position.x - node.position.x) + Mathf.Abs(position.y - node.position.y);
                    }
                }
            }
        }
        return node;
    }
    public void DrawNodes()
    {
        GameObject nodes = new GameObject("Nodes");
        for (int i = 0; i < mapSettings.height; i++)
        {
            for (int j = 0; j < mapSettings.width; j++)
            {
                for (int k = 0; k < mapNodes[i, j].GetNodes().Length; k++)
                {
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.GetComponent<MeshRenderer>().material.color = mapNodes[i, j].GetNode(k).walkable ? Color.green : Color.red;
                    Vector3 pos = mapNodes[i, j].GetNode(k).position;
                    pos.z = -5;
                    sphere.transform.position = pos;
                    sphere.transform.localScale = new Vector3(.1f, .1f, .1f);
                    sphere.transform.name = ("Node with index: " + mapNodes[i, j].GetNode(k).positionInArray + "Parent index: " + mapNodes[i, j].GetNode(k).nodesParent.positionInArray);
                    sphere.transform.parent = nodes.transform;
                }
            }
        }
    }
    public void DrawNode(int x, int y, int index)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Vector3 pos = mapNodes[x, y].GetNode(index).position;
        pos.z = -5;
        sphere.transform.position = pos;
        sphere.transform.localScale = new Vector3(.1f, .1f, .1f);
    }
    public void DrawNode(Node node)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        if (node.walkable) sphere.GetComponent<MeshRenderer>().material.color = Color.green;
        else sphere.GetComponent<MeshRenderer>().material.color = Color.red;
        Vector3 pos = node.position;
        pos.z = -5;
        sphere.transform.position = pos;
        sphere.transform.localScale = new Vector3(.1f, .1f, .1f);
        sphere.transform.name = ("Node with index: " + node.positionInArray + "Parent index: " + node.nodesParent.positionInArray);
    }
    public int GetLayerFromPosition(Vector3 position)
    {
        //if (false) return 10;
        if (position.z >= 1)
        {
            return 4;
        }// bellow sea level
        else if (position.z >= 0)
        {
            return 3;
        }// sea level
        else if (position.z >= -1)
        {
            return 2;
        }// plains
        else if (position.z >= -2)
        {
            return 1;
        }// hills
        else if (position.z >= -3)
        {
            return 0;
        }// mountine
        return 10;
    }
}