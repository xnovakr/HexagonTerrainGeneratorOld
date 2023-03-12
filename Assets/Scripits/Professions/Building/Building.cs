using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Building : MonoBehaviour
{
    private WorldGenerationHandler worldGenerationHandler;
    private Material currentTexture;
    private Sprite currentTexturePrefab;
    public GameObject cubePrefab;
    public GameObject spritePrefab;
    public List<GameObject> toBuild;
    public List<GameObject> building;
    private bool buildingFlag;

    private void Awake()
    {
        worldGenerationHandler = GameObject.Find("WorldHandler").GetComponent<WorldGenerationHandler>();
        buildingFlag = false;
    }
    private void Update()
    {
        if (!buildingFlag) return;
        if (Input.GetMouseButtonDown(2))
        {
            if (currentTexture == null) return;
            Debug.Log(currentTexture.name);
            currentTexture = null;
            currentTexturePrefab = null;
        }
        if (Input.GetMouseButtonDown(0) && currentTexture != null && !GameObject.Find("GameHandler").GetComponent<Utils>().IsMouseOverUI())
        {
            //Debug.Log("LMB pressed in building");
            BuildCube(GameObject.Find("map"), UtilsClass.GetMouseWorldPosition3D(Mathf.Abs(Camera.main.transform.position.z)),currentTexture,1,true);
        }
        if (toBuild.Count > 0 && building.Count == 0)
        {
            BuildObject();
            building.Add(toBuild[0]);
            toBuild.RemoveAt(0);
        }
    }
    public void SetCurrentTexture(Material texture, Sprite texturePrefab)
    {
        currentTexture = texture;
        currentTexturePrefab = texturePrefab;
    }
    public void BuildCube(GameObject parent, Vector3 position, Material material, int height = 1, bool prefab = false)
    {
        MapSettings mapSettings = worldGenerationHandler.mapSettings;
        GameObject cube;
        GameObject[,][][,] map = GameObject.Find("WorldHandler").GetComponent<WorldGenerationHandler>().world;
        int posX, posY;
        worldGenerationHandler.blockGrid.GetXYZ(position, out posX, out posY);
        
        if (!prefab)
        {
            cube = Instantiate(cubePrefab, new Vector3((posX * 2 - mapSettings.width), (posY * 2 - mapSettings.height), -mapSettings.cellSize * height), Quaternion.identity);
            cube.GetComponent<Renderer>().material = material;
            cube.name = "cube " + posX + " " + posY;
            cube.transform.parent = parent.transform;
            //mapArray[i, j] = cube;
            cube.AddComponent<BlockInfo>();
            cube.GetComponent<BlockInfo>().blockSize = mapSettings.cellSize;
            //blockInfo.blockNoiseValue = noiseValue;
            cube.GetComponent<BlockInfo>().walkable = false;
            worldGenerationHandler.blockGrid.SetValue(posX, posY, cube.GetComponent<BlockInfo>());
            switch (height)
            {
                case -6:
                    WorldGenerator.ChangeBlock(map, cube, 0, 0, 4, posX, posY);
                    break;
                case -3:
                    WorldGenerator.ChangeBlock(map, cube, 0, 0, 3, posX, posY);
                    break;
                case -1:
                    WorldGenerator.ChangeBlock(map, cube, 0, 0, 2, posX, posY);
                    break;
                case 1:
                    WorldGenerator.ChangeBlock(map, cube, 0, 0, 1, posX, posY);
                    break;
                case 3:
                    WorldGenerator.ChangeBlock(map, cube, 0, 0, 0, posX, posY);
                    break;
                default:
                    break;
            }
        }
        else
        {
            cube = Instantiate(spritePrefab, new Vector3((posX * 2 - mapSettings.width), (posY * 2 - mapSettings.height), -mapSettings.cellSize * height), Quaternion.identity);
            cube.name = "cubeToBuild" + posX + " " + posY;
            cube.GetComponent<SpriteRenderer>().sprite = currentTexturePrefab;
            cube.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, .5f);
            cube.GetComponent<BlockSelector>().material = material;

            toBuild.Add(cube);
        }
    }
    public void BuildObject()
    {
        if (building.Count == 0) return;

        foreach (Transform builder in GameObject.Find("Settlers").transform)
        {
            builder.gameObject.GetComponent<UnitRTS>().MoveTo(building[0].transform.position);
        }
        BuildCube(GameObject.Find("map"), building[0].transform.position,building[0].GetComponent<BlockSelector>().material, 1, false);
        Destroy(building[0]);
        building.RemoveAt(0);
    }
    public void SetBuildingFlag(bool value)
    {
        if (value == true)
        {
            GameObject.Find("GameRTSControllerObject").GetComponent<GameRTSController>().SetUnitControlFlag(false);
            GameObject.Find("Examining").GetComponent<Examining>().SetExamineFlag(false);
            GameObject.Find("Mining").GetComponent<Mining>().SetMiningFlag(false);
            GameObject.Find("Hunting").GetComponent<Hunting>().SetHuntingFlag(false);
        }
        buildingFlag = value;
    }
}
