using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoad
{
    public static void SaveGame(GameObject[][,] blockData, PerlinSettings perlinSettings, MapSettings mapSettings, int saveIndex)
    {
        string pathBlocks = Application.persistentDataPath + "/hidun" + "Blocks" + saveIndex + ".pactu";
        SaveBlockData(blockData, pathBlocks);
        Debug.Log("Seving path: " + Application.persistentDataPath);

        string pathPerlin = Application.persistentDataPath + "/hidun" + "Perlin" + saveIndex + ".pactu";
        SavePerlinData(perlinSettings, pathPerlin);

        string pathMap = Application.persistentDataPath + "/hidun" + "Map" + saveIndex + ".pactu";
        SaveMapData(mapSettings, pathMap);
    }
    public static void SaveBlockData(GameObject[][,] blockData, string path)
    {
        if (File.Exists(path))
        {
            Debug.Log("Save revritten, " + path);
            File.Delete(path);
        }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream streamBlocks = new FileStream(path, FileMode.Create);
        float[][,] toSave = new float[blockData.GetLength(0)][,];
        //Debug.Log(blockData.Length + " Saving map YxX is " + blockData[0].GetLength(0) + "x" + blockData[0].GetLength(1));
        for (int layer = 0; layer < blockData.Length; layer++)
        {
            toSave[layer] = new float[blockData[layer].GetLength(0), blockData[layer].GetLength(1)];
            for (int y = 0; y < toSave[layer].GetLength(0); y++)
            {
                for (int x = 0; x < toSave[layer].GetLength(1); x++)
                {
                    if (blockData[layer][y, x] == null)
                    {
                        toSave[layer][y, x] = -1;
                        continue;
                    }
                    //Debug.Log(x + " " + y);
                    //Debug.Log(blockData[layer][y, x].GetComponent<BlockBrain>().blockNoiseValue);
                    toSave[layer][y, x] = blockData[layer][y, x].GetComponent<BlockInfo>().blockNoiseValue;
                    //Debug.Log(blockData[layer][x, y].GetComponent<BlockBrain>().blockNoiseValue);
                }
            }
        }
        formatter.Serialize(streamBlocks, toSave);
        streamBlocks.Close();
    }
    public static void SavePerlinData(PerlinSettings perlinData, string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream streamPerlin = new FileStream(path, FileMode.Create);
        formatter.Serialize(streamPerlin, perlinData);
        streamPerlin.Close();
    }
    public static void SaveMapData(MapSettings mapData, string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        float[] mapSettings = new float[8];
        int[][] intMapSettings;

        mapSettings[0] = mapData.width;
        mapSettings[1] = mapData.height;
        mapSettings[2] = mapData.edgeSize;
        mapSettings[3] = GetShapeNuber(mapData.shape);
        mapSettings[4] = mapData.cellSize;
        mapSettings[5] = mapData.originPosition.x;
        mapSettings[6] = mapData.originPosition.y;
        mapSettings[7] = mapData.originPosition.z;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream streamBlocks = new FileStream(path, FileMode.Create);
        intMapSettings = TranslateArrayFloatToInt(mapSettings);
        formatter.Serialize(streamBlocks, mapSettings);
        streamBlocks.Close();
    }
    public static float[][,] LoadBlockData(int index)
    {
        string pathBlocks = Application.persistentDataPath + "/hidun" + "Blocks" + index + ".pactu";

        if (File.Exists(pathBlocks))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(pathBlocks, FileMode.Open);
            float[][,] toLoad = formatter.Deserialize(stream) as float[][,];
            stream.Close();
            return toLoad;

        }
        else
        {
            Debug.Log("No Perlin Save File Found");
            return null;
        }
    }
    public static PerlinSettings LoadPerlinData(int index)
    {
        string pathPerlin = Application.persistentDataPath + "/hidun" + "Perlin" + index + ".pactu";

        if (File.Exists(pathPerlin))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(pathPerlin, FileMode.Open);
            PerlinSettings perlinSettings = formatter.Deserialize(stream) as PerlinSettings;// = new PerlinSettings();
            return perlinSettings;
        }
        else
        {
            Debug.Log("No Perlin Save File Found");
            return null;
        }
    }
    public static MapSettings LoadMapData(int index)
    {
        string pathPerlin = Application.persistentDataPath + "/hidun" + "Map" + index + ".pactu";
        BinaryFormatter formatter = new BinaryFormatter();
        MapSettings mapSettings = new MapSettings();

        if (File.Exists(pathPerlin))
        {
            float[] perlinData;
            FileStream stream = new FileStream(pathPerlin, FileMode.Open);
            perlinData = formatter.Deserialize(stream) as float[];
            stream.Close();
            mapSettings.width = (int)perlinData[0];
            mapSettings.height = (int)perlinData[1];
            mapSettings.edgeSize = (int)perlinData[2];
            mapSettings.shape = GetShapeName((int)perlinData[3]);
            mapSettings.cellSize = perlinData[4];
            mapSettings.originPosition.x = perlinData[5];
            mapSettings.originPosition.y = perlinData[6];
            mapSettings.originPosition.z = perlinData[7];
            return mapSettings;

        }
        else
        {
            Debug.Log("No Map Save File Found");
            return null;
        }

    }
    public static int[][] TranslateArrayFloatToInt(float[] floatArray)
    {
        float accuracy = 1000;
        int[][] intArray = new int[floatArray.Length][];
        for (int i = 0; i < floatArray.Length; i++)
        {
            intArray[i] = new int[2];
            intArray[i][0] = (int)(floatArray[i] / 1);//1,315346     
            intArray[i][1] = (int)((floatArray[i] % 1)* accuracy);
        }
        return intArray;
    }
    public static float[] TranslateArrayIntToFloat(int[][] intArray)
    {
        float accuracy = 1000;
        float[] floatArray = new float[intArray.Length];
        for (int i = 0; i < intArray.Length; i++)
        {
            floatArray[i] = (float)intArray[i][0];  
            floatArray[i] += intArray[i][1] / accuracy;
        }
        return floatArray;
    }
    public static string GetShapeName(int shapeNumber)
    {
        return "octagon";
    }
    public static int GetShapeNuber(string shape)
    {
        return 1;
    }
    public static int BoolToInt(bool val)
    {
        return val == true ? 1 : 0;
    }
    public static GameObject[,] LoadGameBlocks(int saveIndex)
    {
        string pathBlocks = Application.persistentDataPath + "/hidun" + "Blocks" + saveIndex + ".pactu";
        string pathPerlin = Application.persistentDataPath + "/hidun" + "Perlin" + saveIndex + ".pactu";
        string pathMap = Application.persistentDataPath + "/hidun" + "Map" + saveIndex + ".pactu";
        if (File.Exists(pathBlocks))
        {
            GameObject[,] worldData = null;
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(pathBlocks, FileMode.Open);
            worldData = formatter.Deserialize(stream) as GameObject[,];
            stream.Close();
            return worldData;

        }
        else
        {
            Debug.Log("No File Found");
            return null;
        }
    }
    public static bool CheckSave(int index)
    {
        string path = Application.persistentDataPath + "/hidun" + "Blocks" + index + ".pactu";
        return CheckFile(path);
    }
    public static bool CheckFile(string path)
    {
        if (File.Exists(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void DeleteSave(int saveIndex)
    {
        if (!CheckSave(saveIndex)) return;
        File.Delete(Application.persistentDataPath + "/hidun" + "Blocks" + saveIndex + ".pactu");
        File.Delete(Application.persistentDataPath + "/hidun" + "Perlin" + saveIndex + ".pactu");
        File.Delete(Application.persistentDataPath + "/hidun" + "Map" + saveIndex + ".pactu");
    }
    public static bool IntToBool(int val)
    {
        if (val > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
