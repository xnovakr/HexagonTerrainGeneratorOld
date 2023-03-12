using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPicker : MonoBehaviour
{
    public float perlinValue = 0;
    private TerrainGenerator terrainGenerator;
    public int selectedChunkX;
    public int selectedChunkY;
    public int selectedPositionX;
    public int selectedPositionY;
    public int selectedPositionZ;
    private void OnMouseDown()
    {
        if (GameObject.Find("TerrainGenerator")) terrainGenerator = GameObject.Find("TerrainGenerator").GetComponent<TerrainGenerator>();
        //Debug.Log(transform.name);
        if (GameObject.Find("BlockShowcase"))
        {
            UpdateBlockTypeOverview();
            UpdateBlockPositionOverview();
            UpdateBlockPerlinOverview();
        }
        else
        {
            //Debug.Log(transform.parent.name);
            //Debug.Log("current chunk: " + selectedChunkX + " : " + selectedChunkY +
            //    " position: X: " + selectedPositionX + " Y: " + selectedPositionY + " Z: " + selectedPositionZ);
        }
    }
    private void UpdateBlockTypeOverview()
    {
        GameObject.Find("BlockShowcase").GetComponent<MeshRenderer>().material = this.GetComponent<MeshRenderer>().material;
    }
    private void UpdateBlockPositionOverview()
    {
        terrainGenerator.selectedChunkX = selectedChunkX;
        terrainGenerator.selectedChunkY = selectedChunkY;
        terrainGenerator.selectedPositionX = selectedPositionX;
        terrainGenerator.selectedPositionY = selectedPositionY;
        terrainGenerator.selectedPositionZ = selectedPositionZ;
        GameObject.Find("TextGridCoord").GetComponent<Text>().text = selectedPositionY.ToString("000") + " " + selectedPositionX.ToString("000");
    }
    private void UpdateBlockPerlinOverview()
    {
        GameObject.Find("TextPerlinValue").GetComponent<Text>().text = perlinValue.ToString("0.000");
    }
}
