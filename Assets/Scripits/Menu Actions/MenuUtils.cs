using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class MenuUtils : MonoBehaviour
{
    public Slider[] sliders;

    public GameObject MenuButtons;
    public GameObject NewGameButtons;
    public GameObject SettingsButtons;
    public GameObject mapGenerator;
    public GameObject blockInfo;
    public GameObject map;
    private PerlinSettings perlinSettings;
    private MapSettings mapSettings;

    private void Awake()
    {
        if (mapGenerator != null) perlinSettings = mapGenerator.GetComponent<TerrainGenerator>().perlinWorldSettings;
        if (mapGenerator != null) mapSettings = mapGenerator.GetComponent<TerrainGenerator>().mapSettings;
        if (sliders.Length > 0) SetupSliders();
    }
    public void SetPerlinSettingStrenght(Slider slider)
    {
        //Debug.Log("Strenddght" + slider.value);
        mapGenerator.GetComponent<TerrainGenerator>().perlinMountinesSettings.strenght = slider.value;
        mapGenerator.GetComponent<TerrainGenerator>().UpdateNoise();
        mapGenerator.GetComponent<TerrainGenerator>().RegenerateMapMaterial();
    }
    public void SetPerlinSettingOffsetX(Slider slider)
    {
        //Debug.Log("offsetx" + slider.value);
        mapGenerator.GetComponent<TerrainGenerator>().perlinMountinesSettings.offsetX = slider.value;
        mapGenerator.GetComponent<TerrainGenerator>().UpdateNoise();
        mapGenerator.GetComponent<TerrainGenerator>().RegenerateMapMaterial();
    }
    public void SetPerlinSettingOffsetY(Slider slider)
    {
        //Debug.Log("offsety" + slider.value);
        mapGenerator.GetComponent<TerrainGenerator>().perlinMountinesSettings.offsetY = slider.value;
        mapGenerator.GetComponent<TerrainGenerator>().UpdateNoise();
        mapGenerator.GetComponent<TerrainGenerator>().RegenerateMapMaterial();
    }
    public void SetPerlinSettingFrequency(Slider slider)
    {
        //Debug.Log("Frequency" + slider.value);
        mapGenerator.GetComponent<TerrainGenerator>().perlinMountinesSettings.frequency = slider.value;
        mapGenerator.GetComponent<TerrainGenerator>().UpdateNoise();
        mapGenerator.GetComponent<TerrainGenerator>().RegenerateMapMaterial();
    }
    public void SetPerlinSettingScale(Slider slider)
    {
        //Debug.Log("Scale" + slider.value);
        mapGenerator.GetComponent<TerrainGenerator>().perlinMountinesSettings.scale = slider.value;
        mapGenerator.GetComponent<TerrainGenerator>().UpdateNoise();
        mapGenerator.GetComponent<TerrainGenerator>().RegenerateMapMaterial();
    }
    public void SetPerlinSettingPersistance(Slider slider)
    {
        //Debug.Log("Persistance" + slider.value);
        mapGenerator.GetComponent<TerrainGenerator>().perlinMountinesSettings.persistance = slider.value;
        mapGenerator.GetComponent<TerrainGenerator>().UpdateNoise();
        mapGenerator.GetComponent<TerrainGenerator>().RegenerateMapMaterial();
    }
    public void SetPerlinSettingLacunarity(Slider slider)
    {
        //Debug.Log("Lacunarity" + slider.value);
        mapGenerator.GetComponent<TerrainGenerator>().perlinMountinesSettings.lacunarity = slider.value;
        mapGenerator.GetComponent<TerrainGenerator>().UpdateNoise();
        mapGenerator.GetComponent<TerrainGenerator>().RegenerateMapMaterial();
    }
    public void SetPerlinSettingSeed(InputField field)
    {

    }
    public void SetMapSize(Dropdown dropdown)
    {
        switch (dropdown.value)
        {
            case 0:
                mapSettings.width = 150;
                mapSettings.height = 100;
                mapSettings.edgeSize = 30;
                mapGenerator.GetComponent<TerrainGenerator>().mapSettings.edgeSize = mapSettings.edgeSize;
                mapGenerator.GetComponent<TerrainGenerator>().ReGenerate();
                Debug.Log("small");
                break;
            case 1:
                mapSettings.width = 150;
                mapSettings.height = 100;
                mapSettings.edgeSize = 10;
                Debug.Log("medium");
                mapGenerator.GetComponent<TerrainGenerator>().mapSettings.edgeSize = mapSettings.edgeSize;
                mapGenerator.GetComponent<TerrainGenerator>().ReGenerate();
                break;
            case 2:
                mapSettings.width = 150;
                mapSettings.height = 100;
                mapSettings.edgeSize = 0;
                Debug.Log("large");
                mapGenerator.GetComponent<TerrainGenerator>().mapSettings.width = mapSettings.width;
                mapGenerator.GetComponent<TerrainGenerator>().mapSettings.height = mapSettings.height;
                mapGenerator.GetComponent<TerrainGenerator>().mapSettings.edgeSize = mapSettings.edgeSize;
                mapGenerator.GetComponent<TerrainGenerator>().ReGenerate();
                break;
            default:
                break;
        }
    }
    public void SwapMenuGroaps(GameObject selection)
    {
        selection.SetActive(!selection.activeSelf);
        MenuButtons.SetActive(!MenuButtons.activeSelf);
        if (GameObject.Find("map") != null && map == null)
        {
            Debug.Log("Nadamapa");
            map = GameObject.Find("map");
        }
        if (NewGameButtons.activeSelf && map != null)
        {
            map.SetActive(true);
        }
        else if (!NewGameButtons.activeSelf && map != null)
        {
            map.SetActive(false);
        }
    }
    public void DisableObject(GameObject gameObject, bool active)
    {
        gameObject.SetActive(active);
    }
    public void DisableObjectGroap(GameObject[] gameObjectGroap, bool active)
    {
        foreach(var button in gameObjectGroap)
        {
            button.SetActive(active);
        }
    }
    public void QuiteGame()
    {
        Debug.Log("Exit this shajty shiat right now man!");
        Application.Quit();
    }
    public void SetupSliders()
    {
        sliders[0].value = mapGenerator.GetComponent<TerrainGenerator>().perlinMountinesSettings.strenght;
        sliders[1].value = mapGenerator.GetComponent<TerrainGenerator>().perlinMountinesSettings.offsetX;
        sliders[2].value = mapGenerator.GetComponent<TerrainGenerator>().perlinMountinesSettings.offsetY;
        sliders[3].value = mapGenerator.GetComponent<TerrainGenerator>().perlinMountinesSettings.scale;
        sliders[4].value = mapGenerator.GetComponent<TerrainGenerator>().perlinMountinesSettings.frequency;
        sliders[5].value = mapGenerator.GetComponent<TerrainGenerator>().perlinMountinesSettings.persistance;
        sliders[6].value = mapGenerator.GetComponent<TerrainGenerator>().perlinMountinesSettings.lacunarity;
    }
    public void SetupBlockInfoTab(int posX, int posY, GameObject gameObject)
    {
        if (!gameObject) return;
        if (!blockInfo.activeSelf) blockInfo.SetActive(true);
        GameObject.Find("TextGridCoord").GetComponent<Text>().text = posX + " " + posY;
        GameObject.Find("TextPerlinValue").GetComponent<Text>().text = gameObject.GetComponent<BlockBrain>().blockNoiseValue.ToString();
        GameObject.Find("BlockShowcase").GetComponent<MeshRenderer>().material = gameObject.GetComponent<MeshRenderer>().material;
    }
    public void SwapActive(GameObject go)
    {
        go.SetActive(!go.activeSelf);
    }
    public void StartNewGame()
    {
        MapSettings ms = mapGenerator.GetComponent<TerrainGenerator>().mapSettings;
        PerlinSettings ps = mapGenerator.GetComponent<TerrainGenerator>().perlinWorldSettings;
        float[,] nm = GameObject.Find("TerrainGenerator").GetComponent<TerrainGenerator>().noiseMap;
        PlayerPrefs.SetInt("mapWidth", ms.width);
        PlayerPrefs.SetInt("mapHeight", ms.height);
        PlayerPrefs.SetInt("mapEdge", ms.edgeSize);
        PlayerPrefs.SetFloat("mapCellSize", ms.cellSize);
        PlayerPrefs.SetString("mapShape", ms.shape);
        PlayerPrefs.SetInt("perlinOcraves", ps.octaves);
        PlayerPrefs.SetFloat("PerlinLacunarity", ps.lacunarity);
        PlayerPrefs.SetFloat("PerlinWeightMultiplier", ps.weightMultiplier);
        PlayerPrefs.SetFloat("PerlinPersistance", ps.persistance);
        PlayerPrefs.SetFloat("PerlinStrenght", ps.strenght);
        PlayerPrefs.SetFloat("PerlinFrequency", ps.frequency);
        PlayerPrefs.SetFloat("PerlinScale", ps.scale);
        PlayerPrefs.SetFloat("PerlinOffsetX", ps.offsetX);
        PlayerPrefs.SetFloat("PerlinOffsetY", ps.offsetY);
        PlayerPrefs.SetInt("PerlinSeed", ps.seed);
        PlayerPrefs.SetInt("MapSize", GameObject.Find("TerrainGenerator").GetComponent<TerrainGenerator>().mapSize);

        PlayerPrefs.SetInt("selectedChunkX", GameObject.Find("TerrainGenerator").GetComponent<TerrainGenerator>().selectedChunkX);
        PlayerPrefs.SetInt("selectedChunkY", GameObject.Find("TerrainGenerator").GetComponent<TerrainGenerator>().selectedChunkY);
        PlayerPrefs.SetInt("selectedPositionX", GameObject.Find("TerrainGenerator").GetComponent<TerrainGenerator>().selectedPositionX);
        PlayerPrefs.SetInt("selectedPositionY", GameObject.Find("TerrainGenerator").GetComponent<TerrainGenerator>().selectedPositionY);
        PlayerPrefs.SetInt("selectedPositionZ", GameObject.Find("TerrainGenerator").GetComponent<TerrainGenerator>().selectedPositionZ);

        PlayerPrefs.Save();
        SceneManager.LoadScene("Game");
    }
    public void LoadSave(int index)
    {
        if (!SaveLoad.CheckSave(index)) return;
        PlayerPrefs.SetInt("SaveIndex", index);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game");
    }
}
