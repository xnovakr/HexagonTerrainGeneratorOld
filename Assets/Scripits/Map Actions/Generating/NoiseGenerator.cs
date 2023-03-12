using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator
{
    public static float[,] GenerateNoiseMap(PerlinSettings perlinSettings, MapSettings mapSettings, float[,] fallofMap, int posX, int posY)
    {
        Noise noise = new Noise(perlinSettings,mapSettings);
        return noise.GenerateNoiseMap(perlinSettings, mapSettings, fallofMap, perlinSettings.seed, false, posX, posY);
    }
    public static float[,][,] GenerateNoiseWorldMap(int mapSize, PerlinSettings perlinSettings, MapSettings mapSettings, float[,] fallofMap)
    {
        float[,][,] noiseWorldMap = new float[mapSize, mapSize][,];
        Noise noise = new Noise(perlinSettings, mapSettings);
        for (int height = 0; height < mapSize; height++)
        {
            for (int width = 0; width < mapSize; width++)
            {
                noiseWorldMap[height, width] = noise.GenerateNoiseMap(perlinSettings, mapSettings, fallofMap, perlinSettings.seed, false, height, width);
                ApplyFallofMap(noiseWorldMap, height, width, mapSize, mapSettings.height, mapSettings.width);
            }
        }
        return noiseWorldMap;
    }
    public static void ApplyFallofMap(float[,][,] noiseMap, int currentChunkHeight, int currentChunkWidth, int mapSize, int mapHeight, int mapWidth)
    {
        float[,] currentMapChunk = noiseMap[currentChunkHeight, currentChunkWidth];
        if (mapSize == 1)
        {
            noiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentMapChunk, currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapHeight, mapWidth));
            return;
        }
        if (currentChunkHeight == 0) // bot
        {
            if (currentChunkWidth == 0)//left && bot
            {
                noiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentMapChunk, currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapHeight, mapWidth, -1, -1, 0));
            }
            else if (currentChunkWidth == mapSize - 1)//right && bot
            {
                noiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentMapChunk, currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapHeight, mapWidth, -1, -1, 1));
            }
            else//bot
            {
                noiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentMapChunk, currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapHeight, mapWidth, 0, 0));
            }
        }
        else if (currentChunkHeight == mapSize - 1) // top
        {
            if (currentChunkWidth == 0)//left && top
            {
                noiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentMapChunk, currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapHeight, mapWidth, -1, -1, 2));
            }
            else if (currentChunkWidth == mapSize - 1)//right && top
            {
                noiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentMapChunk, currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapHeight, mapWidth, - 1, -1, 3));
            }
            else//top
            {
                noiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentMapChunk, currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapHeight, mapWidth, 0, 1));
            }
        }
        else
        {
            if (currentChunkWidth == 0)//left
            {
                noiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentMapChunk, currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapHeight, mapWidth, 1, 0));
            }
            else if (currentChunkWidth == mapSize - 1)//right
            {
                noiseMap[currentChunkHeight, currentChunkWidth] = ApplyFallofchunk(currentMapChunk, currentChunkHeight, currentChunkWidth, FallOffGenerator.GenerateFallOffMap(mapHeight, mapWidth, 1, 1));
            }
            else
            {
            }
        }
    }
    public static float[,] ApplyFallofchunk(float[,] currentMapChunk, int currentChunkHeight, int currentChunkWidth, float[,] fallofMap)
    {
        for (int i = 0; i < currentMapChunk.GetLength(0); i++)
        {
            for (int j = 0; j < currentMapChunk.GetLength(1); j++)
            {
                currentMapChunk[i, j] = Mathf.Clamp01(currentMapChunk[i, j] - fallofMap[j, i]);
            }
        }
        return currentMapChunk;
    }
}
