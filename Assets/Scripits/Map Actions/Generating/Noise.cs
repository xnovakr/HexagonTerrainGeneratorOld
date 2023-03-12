using UnityEngine;

public class Noise
{
    public PerlinSettings settings;
    public MapSettings mapSettings;
    public Noise(PerlinSettings ps, MapSettings ms)
    {
        settings = ps;
        mapSettings = ms;
    }
    public void UpdateNoise(PerlinSettings ps, MapSettings ms)
    {
        settings = ps;
        mapSettings = ms;
    }
    public float BasicNoiseValue(int x, int y, PerlinSettings settings, MapSettings mapSettings)
    {
        float noiseVal;
        float xCoord = (float)x / mapSettings.width * settings.scale + settings.offsetX;
        float yCoord = (float)y / mapSettings.height * settings.scale + settings.offsetY;
        noiseVal = Mathf.PerlinNoise(xCoord, yCoord) * settings.strenght;
        return noiseVal;
    }


    public float[,] GenerateNoiseMap(PerlinSettings settings, MapSettings mapSettings, float[,] fallofMap, long seed = 0, bool fallofMapSwitch = true, int currentHeightChunk = 0, int currentWidthChunk = 0)
    {
        float[,] noiseMap = new float[mapSettings.height, mapSettings.width];
        if (settings.scale <= 0) settings.scale = 0.001f;
        if (seed == 0)
        {
            seed = Random.Range(-999999, 999999);
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfHeihgt = mapSettings.height / 2f;
        float halfWidth = mapSettings.width / 2f;


        for (int i = 0; i < mapSettings.height; i++)
        {
            for (int j = 0; j < mapSettings.width; j++)
            {
                float amplitude = 1;
                float frequency = settings.frequency;
                float noiseHeight = 0;
                float weight = settings.weightMultiplier;
                for (int k = 0; k < settings.octaves; k++)
                {
                    float sampleX = (((i - halfHeihgt + settings.offsetY + seed + (mapSettings.height * currentHeightChunk)) / settings.scale) * frequency);// ;
                    float sampleY = (((j - halfWidth + settings.offsetX + seed + (mapSettings.width * currentWidthChunk)) / settings.scale) * frequency);//;

                    sampleX /= 2;
                    //sampleY /= 2;

                    float noiseValue = Mathf.PerlinNoise(sampleY, sampleX) * 2 - 1 * amplitude;
                    noiseHeight += noiseValue * amplitude;

                    amplitude *= settings.persistance;
                    frequency *= settings.lacunarity;
                }
                if (noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;
                float temp = noiseHeight * settings.strenght;
                noiseMap[i, j] = temp;
            }
        }

        for (int i = 0; i < mapSettings.height; i++)
        {
            for (int j = 0; j < mapSettings.width; j++)
            {
                float temp = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[i, j]);
                if (fallofMapSwitch)
                {
                    noiseMap[i, j] = Mathf.Clamp01(noiseMap[i, j] - fallofMap[i, j]);
                }
            }
        }
        return noiseMap;
    }
    public float[,] GenerateMountinesMap(PerlinSettings settings, MapSettings mapSettings, float[,] map, BasicTexturePack texture, long seed = 0)
    {
        float[,] noiseMap = new float[mapSettings.height, mapSettings.width];
        if (settings.scale <= 0) settings.scale = 0.001f;
        if (seed == 0) seed = Random.Range(-999999, 999999);

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfHeihgt = mapSettings.height / 2f;
        float halfWidth = mapSettings.width / 2f;

        for (int i = 0; i < mapSettings.height; i++)
        {
            for (int j = 0; j < mapSettings.width; j++)
            {
                float amplitude = 1;
                float frequency = settings.frequency;
                float noiseHeight = 0;
                float weight = settings.weightMultiplier;
                for (int k = 0; k < settings.octaves; k++)
                {
                    float sampleX = (i - halfHeihgt) / settings.scale * frequency + settings.offsetX + seed;
                    float sampleY = (j - halfWidth) / settings.scale * frequency + settings.offsetY + seed;

                    float noiseValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1 * amplitude;
                    noiseHeight += noiseValue * amplitude;

                    amplitude *= settings.persistance;
                    frequency *= settings.lacunarity;
                }
                if (noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;
                float temp = noiseHeight * settings.strenght;
                noiseMap[i, j] = temp;
            }
        }

        for (int i = 0; i < mapSettings.height; i++)
        {
            for (int j = 0; j < mapSettings.width; j++)
            {
                if (map[i, j] <= texture.values[1])
                {
                    noiseMap[i, j] = texture.values[1] - .01f;
                }
                if (noiseMap[i, j] > 1)
                {
                    noiseMap[i, j] = 1;
                }
                if (noiseMap[i, j] < 0)
                {
                    noiseMap[i, j] = 0;
                }
            }
        }
        return map;
    }
}
