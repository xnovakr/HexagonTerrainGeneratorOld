using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator
{
    public static GameObject[,][,] GeneratePrewievMap(MapSettings mapSettings, float[,][,] noiseMap, int mapSize, Material material, Gradient gradient)
    {
        GameObject[,][,] previewMap = new GameObject[mapSize, mapSize][,];

        for (int currentHeightChunk = 0; currentHeightChunk < mapSize; currentHeightChunk++)
        {
            for (int currentWidthChunk = 0; currentWidthChunk < mapSize; currentWidthChunk++)
            {
                previewMap[currentHeightChunk, currentWidthChunk] = GenerateHexagonalMap(mapSettings, noiseMap, 1500 * currentWidthChunk, 1000 * currentHeightChunk, currentHeightChunk, currentWidthChunk, mapSize, material, gradient);
            }
        }

        return previewMap;
    }
    public static GameObject[,] GenerateHexagonalMap(MapSettings mapSettings, float[,][,] noiseMap, int offSetX, int offSetY, int currentChunkHeight, int currentChunkWidth, int mapSize, Material material, Gradient gradient)
    {
        GameObject[,] map = null;
        GameObject mapObject = GameObject.Find("Map");
        GameObject HexagonalMap = new GameObject("HexagonalMap" + currentChunkHeight + currentChunkWidth);
        int width = mapSettings.width;
        int height = mapSettings.height;
        float size = mapSettings.cellSize * 2;
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
                currentPosition = new Vector3(currentX + offSetX, currentY + offSetY, 1);
                map[currentHeight, currentWidth] = BlockGenerator.GenerateHexagon(currentPosition, 1, size, currentChunkHeight, currentChunkWidth, currentHeight, currentWidth, 0, noiseMap[currentChunkHeight, currentChunkWidth][currentHeight, currentWidth], true, material, gradient);
                map[currentHeight, currentWidth].transform.parent = HexagonalMap.transform; //Basic
            }
        }

        if (mapObject)
        {
            HexagonalMap.transform.parent = mapObject.transform;
        }
        if (GameObject.Find("MapObject"))
        {
            HexagonalMap.transform.parent = GameObject.Find("MapObject").transform;
        }

        HexagonalMap.transform.position = new Vector3(0, 0, 0);
        return map;
    }


    public static void RegeneratePreviewMap(GameObject[,][,] previewMap, MapSettings mapSettings, float[,][,] noiseMap, int mapSize, Material material, Gradient gradient)
    {
        DestroyPreviewMap(previewMap);
        GameObject.Find("Map").transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("Map").transform.position = new Vector3(0, 0, 0);
        GameObject.Find("MapCamera").GetComponent<MapCameraHandler>().SetupMapCamera();
        previewMap = GeneratePrewievMap(mapSettings, noiseMap, mapSize, material, gradient);
    }
    public static void DestroyPreviewMap(GameObject[,][,] previewMap)
    {
        if (previewMap != null)
        {
            int size = previewMap.GetLength(0);
            foreach (var obj in previewMap)
            {
                foreach (var ob in obj)
                {
                    GameObject.Destroy(ob);
                }
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    GameObject.Destroy(GameObject.Find("HexagonalMap" + i + j));
                }
            }
        }
    }
}
