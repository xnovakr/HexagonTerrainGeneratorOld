using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldGenerator
{
    public static Hexagon3Dproperties[,][][,] GeneratePotentialMap(MapSettings mapSettings, int mapSize, float[,][,] noiseMap, Grid<BlockInfo> blockGrid, BasicTexturePack texturePack, BlockNodes[,] mapNodes = null)
    {
        Hexagon3Dproperties[,][][,] currentWorld = new Hexagon3Dproperties[mapSize, mapSize][][,];

        for (int chunkHeight = 0; chunkHeight < mapSize; chunkHeight++)
        {
            for (int chunkWidth = 0; chunkWidth < mapSize; chunkWidth++)
            {
                Hexagon3Dproperties[][,] worldChunk = new Hexagon3Dproperties[5][,];
                float offsetX = (mapSettings.width * 1.5f) * (chunkWidth);
                float offsetY = (mapSettings.height * .433f) * (chunkHeight);
                for (int i = 0; i < worldChunk.Length; i++)
                {
                    worldChunk[i] = new Hexagon3Dproperties[mapSettings.height, mapSettings.width];
                }
                for (int height = 0; height < mapSettings.height; height++)
                {
                    for (int widht = 0; widht < mapSettings.width; widht++)
                    {
                        mapNodes[height, widht] = new BlockNodes(new Vector2Int(height, widht));
                        //mapNodes[height, widht].GenerateAllNodeTypes(new Vector3(widht + (widht * .5f) + offsetX, .866f * (height / 2) + offsetY, -5), true);

                        if (ShapeCheck(mapSettings, height, widht) || noiseMap[chunkHeight, chunkWidth][height, widht] == -1)
                        {
                            GameObject temp = new GameObject();
                            temp.AddComponent<BlockInfo>();
                            temp.GetComponent<BlockInfo>().blockSize = mapSettings.cellSize;
                            temp.GetComponent<BlockInfo>().blockNoiseValue = -1;
                            temp.GetComponent<BlockInfo>().walkable = false;
                            blockGrid.SetValue(widht, height, temp.GetComponent<BlockInfo>());
                            GameObject.Destroy(temp);
                            continue;
                        }
                        if (noiseMap[chunkHeight, chunkWidth][height, widht] < texturePack.values[1])
                        {
                            if (height % 2 == 0)
                            {
                                mapNodes[height, widht].GenerateAllNodeTypes(new Vector3(widht + (widht * .5f) + offsetX, .866f * (height / 2) + offsetY, -5), false);
                                worldChunk[3][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + offsetX, .866f * (height / 2) + offsetY, 0), 1, height, widht);
                                worldChunk[4][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + offsetX, .866f * (height / 2) + offsetY, 1), 1, height, widht);
                            }
                            else
                            {
                                mapNodes[height, widht].GenerateAllNodeTypes(new Vector3(widht + (widht * .5f) + .75f + offsetX, .433f * height + offsetY, -5), false);
                                worldChunk[3][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + .75f + offsetX, .433f * height + offsetY, 0), 1, height, widht);
                                worldChunk[4][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + .75f + offsetX, .433f * height + offsetY, 1), 1, height, widht);
                            }
                            continue;
                        }
                        if (noiseMap[chunkHeight, chunkWidth][height, widht] > texturePack.values[1])
                        {

                            if (noiseMap[chunkHeight, chunkWidth][height, widht] > texturePack.values[4])
                            {
                                if (height % 2 == 0)
                                {
                                    mapNodes[height, widht].GenerateAllNodeTypes(new Vector3(widht + (widht * .5f) + offsetX, .866f * (height / 2) + offsetY, -5), false);
                                    worldChunk[2][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + offsetX, .866f * (height / 2) + offsetY, -1), 1, height, widht);
                                    worldChunk[3][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + offsetX, .866f * (height / 2) + offsetY, 0), 1, height, widht);
                                    worldChunk[4][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + offsetX, .866f * (height / 2) + offsetY, 1), 1, height, widht);
                                }
                                else
                                {
                                    mapNodes[height, widht].GenerateAllNodeTypes(new Vector3(widht + (widht * .5f) + .75f + offsetX, .433f * height + offsetY, -5), false);
                                    worldChunk[2][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + .75f + offsetX, .433f * height + offsetY, -1), 1, height, widht);
                                    worldChunk[3][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + .75f + offsetX, .433f * height + offsetY, 0), 1, height, widht);
                                    worldChunk[4][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + .75f + offsetX, .433f * height + offsetY, 1), 1, height, widht);
                                }
                            }
                            else
                            {
                                if (height % 2 == 0)
                                {
                                    mapNodes[height, widht].GenerateAllNodeTypes(new Vector3(widht + (widht * .5f) + offsetX, .866f * (height / 2) + offsetY, -5), true);
                                    worldChunk[2][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + offsetX, .866f * (height / 2) + offsetY, -1), 1, height, widht);
                                    worldChunk[3][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + offsetX, .866f * (height / 2) + offsetY, 0), 1, height, widht);
                                    worldChunk[4][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + offsetX, .866f * (height / 2) + offsetY, 1), 1, height, widht);
                                }
                                else
                                {
                                    mapNodes[height, widht].GenerateAllNodeTypes(new Vector3(widht + (widht * .5f) + .75f + offsetX, .433f * height + offsetY, -5), true);
                                    worldChunk[2][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + .75f + offsetX, .433f * height + offsetY, -1), 1, height, widht);
                                    worldChunk[3][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + .75f + offsetX, .433f * height + offsetY, 0), 1, height, widht);
                                    worldChunk[4][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + .75f + offsetX, .433f * height + offsetY, 1), 1, height, widht);
                                }
                            }
                        }
                        if (noiseMap[chunkHeight, chunkWidth][height, widht] > texturePack.values[3])
                        {
                            if (height % 2 == 0)
                            {
                                mapNodes[height, widht].GenerateAllNodeTypes(new Vector3(widht + (widht * .5f) + offsetX, .866f * (height / 2) + offsetY, -5), false);
                                worldChunk[1][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + offsetX, .866f * (height / 2) + offsetY, -2), 1, height, widht);
                            }
                            else
                            {
                                mapNodes[height, widht].GenerateAllNodeTypes(new Vector3(widht + (widht * .5f) + .75f + offsetX, .433f * height + offsetY, -5), false);
                                worldChunk[1][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + .75f + offsetX, .433f * height + offsetY, -2), 1, height, widht);
                            }
                            if (noiseMap[chunkHeight, chunkWidth][height, widht] > texturePack.values[4])
                            {
                                if (height % 2 == 0)
                                {
                                    mapNodes[height, widht].GenerateAllNodeTypes(new Vector3(widht + (widht * .5f) + offsetX, .866f * (height / 2) + offsetY, -5), false);
                                    worldChunk[0][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + offsetX, .866f * (height / 2) + offsetY, -4), 1, height, widht);
                                }
                                else
                                {
                                    mapNodes[height, widht].GenerateAllNodeTypes(new Vector3(widht + (widht * .5f) + .75f + offsetX, .433f * height + offsetY, -5), false);
                                    worldChunk[0][height, widht] = new Hexagon3Dproperties(new Vector3(widht + (widht * .5f) + .75f + offsetX, .433f * height + offsetY, -4), 1, height, widht);
                                }
                            }
                        }
                    }
                }
                currentWorld[chunkHeight, chunkWidth] = worldChunk;
            }
        }
        return currentWorld;
    }
    public static void ApplyPotentialMap(Hexagon3Dproperties[,][][,] potentialWorld, GameObject[,][][,] world, float[,][,] noiseMap, BasicTexturePack texturePack)
    {
        for (int chunkHeight = 0; chunkHeight < potentialWorld.GetLength(0); chunkHeight++)
        {
            for (int chunkWidth = 0; chunkWidth < potentialWorld.GetLength(1); chunkWidth++)
            {
                GameObject Chunk = new GameObject("Chunk" + chunkHeight + " : " + chunkWidth);
                GameObject LayerOne = new GameObject("LayerOne");
                GameObject LayerTwo = new GameObject("LayerTwo");
                GameObject LayerThree = new GameObject("LayerThree");
                GameObject LayerFour = new GameObject("LayerFour");
                GameObject LayerFive = new GameObject("LayerFive");
                world[chunkHeight, chunkWidth] = new GameObject[potentialWorld[chunkHeight, chunkWidth].Length][,];
                for (int layer = 0; layer < potentialWorld[chunkHeight, chunkWidth].Length; layer++)
                {
                    world[chunkHeight, chunkWidth][layer] = new GameObject[potentialWorld[chunkHeight, chunkWidth][layer].GetLength(0), potentialWorld[chunkHeight, chunkWidth][layer].GetLength(1)];
                    for (int height = 0; height < potentialWorld[chunkHeight, chunkWidth][layer].GetLength(0); height++)
                    {
                        for (int width = 0; width < potentialWorld[chunkHeight, chunkWidth][layer].GetLength(1); width++)
                        {
                            if (potentialWorld[chunkHeight, chunkWidth][layer][height, width] == null) continue;
                            if (CheckForBlockInLayers(world, layer, chunkWidth, chunkHeight, width, height)) continue;
                            //Debug.Log(width);
                            //if (layer == 0)
                            //{
                            /*world[chunkHeight, chunkWidth][layer][height, width] =*/
                            ApplyPotencialBlock(potentialWorld, world, chunkHeight, chunkWidth, layer, height, width); //kid nema renderer!!!
                            world[chunkHeight, chunkWidth][layer][height, width].transform.position = potentialWorld[chunkHeight, chunkWidth][layer][height, width].position;
                            ApplyMaterialToObject(world[chunkHeight, chunkWidth][layer][height, width], SelectMaterial(noiseMap[chunkHeight, chunkWidth][height, width], texturePack));
                            switch (layer)
                            {
                                case 0:
                                    world[chunkHeight, chunkWidth][layer][height, width].transform.parent = LayerOne.transform;
                                    break;
                                case 1:
                                    world[chunkHeight, chunkWidth][layer][height, width].transform.parent = LayerTwo.transform;
                                    break;
                                case 2:
                                    world[chunkHeight, chunkWidth][layer][height, width].transform.parent = LayerThree.transform;
                                    break;
                                case 3:
                                    world[chunkHeight, chunkWidth][layer][height, width].transform.parent = LayerFour.transform;
                                    break;
                                case 4:
                                    world[chunkHeight, chunkWidth][layer][height, width].transform.parent = LayerFive.transform;
                                    break;
                            }
                        }
                    }
                }
                LayerOne.transform.parent = Chunk.transform;
                LayerTwo.transform.parent = Chunk.transform;
                LayerThree.transform.parent = Chunk.transform;
                LayerFour.transform.parent = Chunk.transform;
                LayerFive.transform.parent = Chunk.transform;
                if (GameObject.Find("World")) Chunk.transform.parent = GameObject.Find("World").transform;
            }
        }
    }
    public static void ApplyPotencialBlock(Hexagon3Dproperties[,][][,] potentialWorld, GameObject[,][][,] world, int chunkY, int chunkX, int layer, int posY, int posX, bool mining = false)
    {
        if (layer > world[chunkY, chunkX].Length - 1)
        {
            Debug.Log("Layer was out of range, layer index is: " + layer);
            return;
        }
        if (world[chunkY, chunkX][layer][posY, posX] == null)
        {//blok neni vytvoreny
            GameObject block = new GameObject("block " + posY + " : " + posX);
            GenerateSides(potentialWorld, block, chunkX, chunkY, layer, posX, posY, mining);
            world[chunkY, chunkX][layer][posY, posX] = block;
        }
        else
        {//blok je vytvoreny
            Debug.Log("Using already created obj");


            Vector3 pos;
            WorldGenerationHandler worldGenerationHandler = GameObject.Find("WorldHandler").GetComponent<WorldGenerationHandler>();
            pos = world[chunkY, chunkX][layer][posY, posX].transform.position;
            world[chunkY, chunkX][layer][posY, posX].transform.position = Vector3.zero;
            GenerateSides(potentialWorld, world[chunkY, chunkX][layer][posY, posX], chunkX, chunkY, layer, posX, posY, mining);
            world[chunkY, chunkX][layer][posY, posX].transform.position = pos;

        }
    }
    public static void GenerateSides(Hexagon3Dproperties[,][][,] potentialWorld, GameObject parent, int chunkY, int chunkX, int layer, int posY, int posX, bool mining = false)
    {//need to chceck for childs
        //Debug.Log("chunkX " + chunkX + " chunkY " + chunkY + " layer " + layer + " posX " + posX + " posY " + posY);
        Hexagon3Dproperties currentProperties = potentialWorld[chunkX, chunkY][layer][posX, posY];
        if (currentProperties == null) return;
        //Debug.Log(posX + " " + posY);
        if (layer - 1 >= 0 && potentialWorld[chunkX, chunkY][layer - 1][posX, posY] != null && !mining)
        {
            Debug.Log("Not generating sides.");
            return;
        }
        else if ((!parent.transform.Find("HexagonTop")))
        {
            //Debug.Log("Generating sides.");
            if (mining && CheckForBlockInLayers(GameObject.Find("WorldHandler").GetComponent<WorldGenerationHandler>().world, layer, chunkX, chunkY, posY, posX))
            {
                //Debug.Log("Already under floor.");
            }
            else
            {
                //Debug.Log("skip floor");
                GameObject panel = BlockGenerator.GenerateHexagon(new Vector3(0, 0), currentProperties.size, 1, chunkX, chunkY, posX, posY);
                panel.transform.name = "HexagonTop";
                panel.transform.parent = parent.transform;
            }
        }
        if ((posX - 2 >= 0 && potentialWorld[chunkX, chunkY][layer][posX - 2, posY] == null) || posX - 2 < 0)
        {   //Top i guess
            if (parent.transform.Find("top")){}
            else
            {
                GameObject panel = BlockGenerator.GenerateRectangle(currentProperties.GetPosition(3), currentProperties.size);
                currentProperties.SetupSide(panel, 3);
                panel.transform.parent = parent.transform;
            }
        }
        if ((posX + 2 < potentialWorld[chunkX, chunkY][layer].GetLength(0) && potentialWorld[chunkX, chunkY][layer][posX + 2, posY] == null) 
            || posX + 2 > potentialWorld[chunkX, chunkY][layer].GetLength(0) - 1)
        {   //Bot i guess 
            if (parent.transform.Find("bot")) { }
            else
            {
                GameObject panel = BlockGenerator.GenerateRectangle(currentProperties.GetPosition(0), currentProperties.size);
                currentProperties.SetupSide(panel, 0);
                panel.transform.parent = parent.transform;
            }
        }
        if (posX % 2 == 0)
        {//parne
            if ((posX - 1 >= 0 && potentialWorld[chunkX, chunkY][layer][posX - 1, posY] == null) 
                || posX - 1 < 0)
            {   //TopLeft parne 0 neparne + 1
                if (parent.transform.Find("topLeft")) { }
                else
                {
                    GameObject panel = BlockGenerator.GenerateRectangle(currentProperties.GetPosition(4), currentProperties.size);
                    currentProperties.SetupSide(panel, 4);
                    panel.transform.parent = parent.transform;
                }
            }
            if ((posX - 1 >= 0 && posY - 1 >= 0 && potentialWorld[chunkX, chunkY][layer][posX - 1, posY - 1] == null) 
                || posX - 1 < 0 || posY == 0)
            {   //TopRight parne -1 neparne 0
                if (parent.transform.Find("topRight")) { }
                else
                {
                    GameObject panel = BlockGenerator.GenerateRectangle(currentProperties.GetPosition(5), currentProperties.size);
                    currentProperties.SetupSide(panel, 5);
                    panel.transform.parent = parent.transform;
                }
            }
            if ((posX + 1 < potentialWorld[chunkX, chunkY][layer].GetLength(0) && potentialWorld[chunkX, chunkY][layer][posX + 1, posY] == null) 
                || posX + 1 > potentialWorld[chunkX, chunkY][layer].GetLength(0) - 1)
            {   //BotLeft parne 0 neparne + 1
                if (parent.transform.Find("botLeft")) { }
                else
                {
                    GameObject panel = BlockGenerator.GenerateRectangle(currentProperties.GetPosition(1), currentProperties.size);
                    currentProperties.SetupSide(panel, 1);
                    panel.transform.parent = parent.transform;
                }
            }
            if ((posX + 1 < potentialWorld[chunkX, chunkY][layer].GetLength(0) && posY - 1 >= 0 && potentialWorld[chunkX, chunkY][layer][posX + 1, posY - 1] == null) 
                || posX + 1 > potentialWorld[chunkX, chunkY][layer].GetLength(0) - 1 || posY == 0)
            {   //BotRight i guess parne - 1 neparne 0
                if (parent.transform.Find("botRight")) { }
                else
                {
                    GameObject panel = BlockGenerator.GenerateRectangle(currentProperties.GetPosition(2), currentProperties.size);
                    currentProperties.SetupSide(panel, 2); // { "bot", "botLeft", "botRight", "top", "topLeft", "topRight" };
                    panel.transform.parent = parent.transform;
                }
            }
        }
        else
        {//neparne
            if ((posX - 1 >= 0 && posY + 1 < potentialWorld[chunkX, chunkY][layer].GetLength(1) && potentialWorld[chunkX, chunkY][layer][posX - 1, posY + 1] == null) 
                || posX - 1 < 0 || posY == potentialWorld[chunkX, chunkY][layer].GetLength(1) - 1)
            {   //TopLeft parne 0 neparne + 1
                if (parent.transform.Find("topLeft")) { }
                else
                {
                    GameObject panel = BlockGenerator.GenerateRectangle(currentProperties.GetPosition(4), currentProperties.size);
                    currentProperties.SetupSide(panel, 4);
                    panel.transform.parent = parent.transform;
                }
            }
            if ((posX - 1 >= 0 && potentialWorld[chunkX, chunkY][layer][posX - 1, posY] == null)
                || posX - 1 < 0 && (!parent.transform.Find("")))
            {   //TopRight parne -1 neparne 0
                if (parent.transform.Find("topRight")) { }
                else
                {
                    GameObject panel = BlockGenerator.GenerateRectangle(currentProperties.GetPosition(5), currentProperties.size);
                    currentProperties.SetupSide(panel, 5);
                    panel.transform.parent = parent.transform;
                }
            }
            if ((posX + 1 < potentialWorld[chunkX, chunkY][layer].GetLength(0) && posY + 1 < potentialWorld[chunkX, chunkY][layer].GetLength(1) && potentialWorld[chunkX, chunkY][layer][posX + 1, posY + 1] == null) 
                || posX + 1 > potentialWorld[chunkX, chunkY][layer].GetLength(0) - 1 || posY == potentialWorld[chunkX, chunkY][layer].GetLength(1) - 1)
            {   //BotLeft parne 0 neparne + 1
                if (parent.transform.Find("botLeft")) { }
                else
                {
                    GameObject panel = BlockGenerator.GenerateRectangle(currentProperties.GetPosition(1), currentProperties.size);
                    currentProperties.SetupSide(panel, 1);
                    panel.transform.parent = parent.transform;
                }
            }
            if ((posX + 1 < potentialWorld[chunkX, chunkY][layer].GetLength(0) && potentialWorld[chunkX, chunkY][layer][posX + 1, posY] == null)
                || posX + 1 > potentialWorld[chunkX, chunkY][layer].GetLength(0) - 1)
            {   //BotRight i guess parne - 1 neparne 0
                if (parent.transform.Find("botRight")) { }
                else
                {
                    GameObject panel = BlockGenerator.GenerateRectangle(currentProperties.GetPosition(2), currentProperties.size);
                    currentProperties.SetupSide(panel, 2);
                    panel.transform.parent = parent.transform;
                }
            }
        }
    }


    public static void GenerateWholeNewWorld(GameObject[,][][,] world, int mapSize, MapSettings mapSettings, float[,][,] noiseMap, Grid<BlockInfo> blockGrid, BasicTexturePack texturePack)
    {
        world = new GameObject[mapSize, mapSize][][,];
        for (int currentHeightChunk = 0; currentHeightChunk < mapSize; currentHeightChunk++)
        {
            for (int currentWidthChunk = 0; currentWidthChunk < mapSize; currentWidthChunk++)
            {
                world[currentHeightChunk, currentWidthChunk] = NewWorldTryout(currentHeightChunk, currentWidthChunk, mapSettings, noiseMap, blockGrid, texturePack);
            }
        }
    }
    public static GameObject[][,] NewWorldTryout(int indexHeight, int indexWidth, MapSettings mapSettings, float[,][,] noiseMap, Grid<BlockInfo> blockGrid, BasicTexturePack texturePack)
    {
        GameObject[][,] worldChunk = new GameObject[5][,];
        GameObject LayerOne = new GameObject("LayerOne");
        GameObject LayerTwo = new GameObject("LayerTwo");
        GameObject LayerThree = new GameObject("LayerThree");
        GameObject LayerFour = new GameObject("LayerFour");
        GameObject LayerFive = new GameObject("LayerFive");
        GameObject Chunk = new GameObject("Chunk" + indexHeight + ":" + indexWidth);

        for (int i = 0; i < 5; i++)
        {
            worldChunk[i] = new GameObject[mapSettings.height, mapSettings.width];
        }
        for (int height = 0; height < mapSettings.height; height++)
        {
            for (int widht = 0; widht < mapSettings.width; widht++)
            {
                if (ShapeCheck(mapSettings, height, widht) || noiseMap[height, widht][height, widht] == -1)
                {
                    GameObject temp = new GameObject();
                    temp.AddComponent<BlockInfo>();
                    temp.GetComponent<BlockInfo>().blockSize = mapSettings.cellSize;
                    temp.GetComponent<BlockInfo>().blockNoiseValue = -1;
                    temp.GetComponent<BlockInfo>().walkable = false;
                    blockGrid.SetValue(widht, height, temp.GetComponent<BlockInfo>());
                    GameObject.Destroy(temp);
                    continue;
                }
                if (noiseMap[height, widht][height, widht] < texturePack.values[1])
                {
                    //posx = mapSettings.originPosition.x + mapSettings.cellSize * j * 2 - offsetX;
                    //posy = mapSettings.originPosition.y + mapSettings.cellSize * i * 2 - offsetY;
                    if (height % 2 == 0)
                    {
                        worldChunk[3][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f), .866f * (height / 2), 0), 1, 1, height, widht);
                        worldChunk[4][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f), .866f * (height / 2), 2), 1, 1, height, widht);
                    }
                    else
                    {
                        worldChunk[3][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f) + .75f, .433f * height, 0), 1, 1, height, widht);
                        worldChunk[4][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f) + .75f, .433f * height, 2), 1, 1, height, widht);
                    }
                    ApplyMaterialToObject(worldChunk[3][height, widht], SelectMaterial(noiseMap[height, widht][height, widht], texturePack));
                    ApplyMaterialToObject(worldChunk[4][height, widht], SelectMaterial(noiseMap[height, widht][height, widht], texturePack));
                    //worldChunk[3][i, j] = SetupCube(GameObject.Find("World"), posx, posy, i, j, noiseMap[i, j], -3);
                    //worldChunk[4][i, j] = SetupCube(GameObject.Find("World"), posx, posy, i, j, noiseMap[i, j], -5);
                    if (worldChunk[3][height, widht])
                    {
                        worldChunk[3][height, widht].transform.parent = LayerFour.transform;
                    }
                    if (worldChunk[4][height, widht])
                    {
                        worldChunk[4][height, widht].transform.parent = LayerFive.transform;
                    }
                    continue;
                }
                if (noiseMap[height, widht][height, widht] > texturePack.values[1])
                {
                    //posx = mapSettings.originPosition.x + mapSettings.cellSize * j * 2 - offsetX;
                    //posy = mapSettings.originPosition.y + mapSettings.cellSize * i * 2 - offsetY;

                    if (noiseMap[height, widht][height, widht] > texturePack.values[4])
                    {
                        if (height % 2 == 0)
                        {
                            worldChunk[2][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f), .866f * (height / 2), -1), 1, 1, height, widht);
                            worldChunk[3][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f), .866f * (height / 2), 3), 1, 1, height, widht);
                            worldChunk[4][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f), .866f * (height / 2), 2), 1, 1, height, widht);
                        }
                        else
                        {
                            worldChunk[2][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f) + .75f, .433f * height, -1), 1, 1, height, widht);
                            worldChunk[3][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f) + .75f, .433f * height, 0), 1, 1, height, widht);
                            worldChunk[4][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f) + .75f, .433f * height, 2), 1, 1, height, widht);
                        }
                        ApplyMaterialToObject(worldChunk[2][height, widht], SelectMaterial(texturePack.values[3] + (float)0.1, texturePack));
                        ApplyMaterialToObject(worldChunk[3][height, widht], SelectMaterial(texturePack.values[3] + (float)0.1, texturePack));
                        ApplyMaterialToObject(worldChunk[4][height, widht], SelectMaterial(texturePack.values[3] + (float)0.1, texturePack));
                        //worldChunk[2][i, j] = SetupCube(GameObject.Find("World"), posx, posy, i, j, texturePack.values[3] - (float).01, -1);
                        //worldChunk[3][i, j] = SetupCube(GameObject.Find("World"), posx, posy, i, j, texturePack.values[3] - (float).01, -3);
                        //worldChunk[4][i, j] = SetupCube(GameObject.Find("World"), posx, posy, i, j, texturePack.values[3] - (float).01, -5);
                    }
                    else
                    {
                        if (height % 2 == 0)
                        {
                            worldChunk[2][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f), .866f * (height / 2), -1), 1, 1, height, widht);
                            worldChunk[3][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f), .866f * (height / 2), 0), 1, 1, height, widht);
                            worldChunk[4][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f), .866f * (height / 2), 2), 1, 1, height, widht);
                        }
                        else
                        {
                            worldChunk[2][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f) + .75f, .433f * height, -1), 1, 1, height, widht);
                            worldChunk[3][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f) + .75f, .433f * height, 0), 1, 1, height, widht);
                            worldChunk[4][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f) + .75f, .433f * height, 2), 1, 1, height, widht);
                        }
                        ApplyMaterialToObject(worldChunk[2][height, widht], SelectMaterial(noiseMap[height, widht][height, widht], texturePack));
                        ApplyMaterialToObject(worldChunk[3][height, widht], SelectMaterial(noiseMap[height, widht][height, widht], texturePack));
                        ApplyMaterialToObject(worldChunk[4][height, widht], SelectMaterial(noiseMap[height, widht][height, widht], texturePack));
                        //worldChunk[2][i, j] = SetupCube(GameObject.Find("World"), posx, posy, i, j, noiseMap[i, j], -1);
                        //worldChunk[3][i, j] = SetupCube(GameObject.Find("World"), posx, posy, i, j, noiseMap[i, j], -3);
                        //worldChunk[4][i, j] = SetupCube(GameObject.Find("World"), posx, posy, i, j, noiseMap[i, j], -5);
                    }
                }
                if (noiseMap[height, widht][height, widht] > texturePack.values[3])
                {
                    //posx = mapSettings.originPosition.x + mapSettings.cellSize * j * 2 - offsetX;
                    //posy = mapSettings.originPosition.y + mapSettings.cellSize * i * 2 - offsetY;
                    if (height % 2 == 0)
                    {
                        worldChunk[1][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f), .866f * (height / 2), -2), 1, 1, height, widht);
                    }
                    else
                    {
                        worldChunk[1][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f) + .75f, .433f * height, -2), 1, 1, height, widht);
                    }
                    ApplyMaterialToObject(worldChunk[1][height, widht], SelectMaterial(texturePack.values[3] + (float)0.1, texturePack));
                    //mapsArray[1][height, widht] = SetupCube(GameObject.Find("World"), posx, posy, i, j, texturePack.values[3] + (float)0.1);
                    if (noiseMap[height, widht][height, widht] > texturePack.values[4])
                    {
                        //posx = mapSettings.originPosition.x + mapSettings.cellSize * j * 2 - offsetX;
                        //posy = mapSettings.originPosition.y + mapSettings.cellSize * i * 2 - offsetY;
                        if (height % 2 == 0)
                        {
                            worldChunk[0][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f), .866f * (height / 2), -4), 1, 1, height, widht);
                        }
                        else
                        {
                            worldChunk[0][height, widht] = BlockGenerator.Generate3DHexagon(new Vector3(widht + (widht * .5f) + .75f, .433f * height, -4), 1, 1, height, widht);
                        }
                        worldChunk[0][height, widht].name = height + " " + widht;
                        ApplyMaterialToObject(worldChunk[0][height, widht], SelectMaterial(noiseMap[height, widht][height, widht], texturePack));
                    }
                }
                if (worldChunk[0][height, widht])
                {
                    worldChunk[0][height, widht].transform.parent = LayerOne.transform;
                }
                if (worldChunk[1][height, widht])
                {
                    worldChunk[1][height, widht].transform.parent = LayerTwo.transform;
                }
                if (worldChunk[2][height, widht])
                {
                    worldChunk[2][height, widht].transform.parent = LayerThree.transform;
                }
                if (worldChunk[3][height, widht])
                {
                    worldChunk[3][height, widht].transform.parent = LayerFour.transform;
                }
                if (worldChunk[4][height, widht])
                {
                    worldChunk[4][height, widht].transform.parent = LayerFive.transform;
                }
            }
        }
        LayerOne.transform.parent = Chunk.transform;
        LayerTwo.transform.parent = Chunk.transform;
        LayerThree.transform.parent = Chunk.transform;
        LayerFour.transform.parent = Chunk.transform;
        LayerFive.transform.parent = Chunk.transform;
        Chunk.transform.parent = GameObject.Find("World").transform;
        return worldChunk;
    }



    public static void GenerateWholeWorld(GameObject[,][][,] world, float[,][,] noiseMap, int mapSize, Grid<BlockInfo> blockGrid, MapSettings mapSettings, BasicTexturePack texturePack, GameObject cubePrefab)
    {
        world = new GameObject[mapSize, mapSize][][,];
        for (int currentHeightChunk = 0; currentHeightChunk < mapSize; currentHeightChunk++)
        {
            for (int currentWidthChunk = 0; currentWidthChunk < mapSize; currentWidthChunk++)
            {
                world[currentHeightChunk, currentWidthChunk] = GenerateWorldChunk(noiseMap, blockGrid, mapSettings, texturePack, cubePrefab);
                AssingChunkParent(world[currentHeightChunk, currentWidthChunk], "Chunk " + currentHeightChunk + " " + currentWidthChunk, GameObject.Find("World"));
            }
        }
    }
    public static GameObject[][,] GenerateWorldChunk( float[,][,] noiseMap, Grid<BlockInfo> blockGrid, MapSettings mapSettings, BasicTexturePack texturePack, GameObject cubePrefab)
    {
        GameObject[][,] worldChunk = null;

        worldChunk = new GameObject[5][,];
        for (int i = 0; i < 5; i++)
        {// 0 vrch hory      1 hora         2 zem       3 podzemie1     4 podzemie2 
            worldChunk[i] = new GameObject[mapSettings.height, mapSettings.width];
        }
        int offsetX = 0;
        int offsetY = 0;

        Vector3 originPos = mapSettings.originPosition;
        originPos.x -= mapSettings.width + (mapSettings.cellSize);
        originPos.y -= mapSettings.height + (mapSettings.cellSize);
        blockGrid = new Grid<BlockInfo>(mapSettings.width, mapSettings.height, mapSettings.cellSize * 2, originPos);

        for (int i = 0; i < mapSettings.height; i++)
        {
            for (int j = 0; j < mapSettings.width; j++)
            {
                float posx;
                float posy;
                if (ShapeCheck(mapSettings, i, j) || noiseMap[0,0][i, j] == -1)
                {
                    GameObject temp = new GameObject();
                    temp.AddComponent<BlockInfo>();
                    temp.GetComponent<BlockInfo>().blockSize = mapSettings.cellSize;
                    temp.GetComponent<BlockInfo>().blockNoiseValue = -1;
                    temp.GetComponent<BlockInfo>().walkable = false;
                    blockGrid.SetValue(j, i, temp.GetComponent<BlockInfo>());
                    GameObject.Destroy(temp);
                    continue;
                }
                if (noiseMap[0, 0][i, j] < texturePack.values[1])
                {
                    posx = mapSettings.originPosition.x + mapSettings.cellSize * j * 2 - offsetX;
                    posy = mapSettings.originPosition.y + mapSettings.cellSize * i * 2 - offsetY;
                    worldChunk[3][i, j] = BlockGenerator.SetupCubePrefab(noiseMap, blockGrid, cubePrefab, mapSettings, texturePack, GameObject.Find("World"), posx, posy, i, j, noiseMap[0, 0][i, j], -3);
                    worldChunk[4][i, j] = BlockGenerator.SetupCubePrefab(noiseMap, blockGrid, cubePrefab, mapSettings, texturePack, GameObject.Find("World"), posx, posy, i, j, noiseMap[0, 0][i, j], -5);
                    continue;
                }
                if (noiseMap[0, 0][i, j] > texturePack.values[1])
                {
                    posx = mapSettings.originPosition.x + mapSettings.cellSize * j * 2 - offsetX;
                    posy = mapSettings.originPosition.y + mapSettings.cellSize * i * 2 - offsetY;

                    if (noiseMap[0, 0][i, j] > texturePack.values[4])
                    {
                        worldChunk[2][i, j] = BlockGenerator.SetupCubePrefab(noiseMap, blockGrid, cubePrefab, mapSettings, texturePack, GameObject.Find("World"), posx, posy, i, j, texturePack.values[3] - (float).01, -1);
                        worldChunk[3][i, j] = BlockGenerator.SetupCubePrefab(noiseMap, blockGrid, cubePrefab, mapSettings, texturePack, GameObject.Find("World"), posx, posy, i, j, texturePack.values[3] - (float).01, -3);
                        worldChunk[4][i, j] = BlockGenerator.SetupCubePrefab(noiseMap, blockGrid, cubePrefab, mapSettings, texturePack, GameObject.Find("World"), posx, posy, i, j, texturePack.values[3] - (float).01, -5);
                    }
                    else
                    {
                        worldChunk[2][i, j] = BlockGenerator.SetupCubePrefab(noiseMap, blockGrid, cubePrefab, mapSettings, texturePack, GameObject.Find("World"), posx, posy, i, j, noiseMap[0, 0][i, j], -1);
                        worldChunk[3][i, j] = BlockGenerator.SetupCubePrefab(noiseMap, blockGrid, cubePrefab, mapSettings, texturePack, GameObject.Find("World"), posx, posy, i, j, noiseMap[0, 0][i, j], -3);
                        worldChunk[4][i, j] = BlockGenerator.SetupCubePrefab(noiseMap, blockGrid, cubePrefab, mapSettings, texturePack, GameObject.Find("World"), posx, posy, i, j, noiseMap[0, 0][i, j], -5);
                    }
                }
                if (noiseMap[0, 0][i, j] > texturePack.values[3])
                {
                    posx = mapSettings.originPosition.x + mapSettings.cellSize * j * 2 - offsetX;
                    posy = mapSettings.originPosition.y + mapSettings.cellSize * i * 2 - offsetY;
                    worldChunk[1][i, j] = BlockGenerator.SetupCubePrefab(noiseMap, blockGrid, cubePrefab, mapSettings, texturePack, GameObject.Find("World"), posx, posy, i, j, texturePack.values[3] + (float)0.1);
                    if (noiseMap[0, 0][i, j] > texturePack.values[4])
                    {
                        posx = mapSettings.originPosition.x + mapSettings.cellSize * j * 2 - offsetX;
                        posy = mapSettings.originPosition.y + mapSettings.cellSize * i * 2 - offsetY;
                        worldChunk[0][i, j] = BlockGenerator.SetupCubePrefab(noiseMap, blockGrid, cubePrefab, mapSettings, texturePack, GameObject.Find("World"), posx, posy, i, j, noiseMap[0, 0][i, j], 3);
                    }
                }
            }
        }
        var position = GameObject.Find("World").transform.position;
        position = new Vector3(-mapSettings.width, -mapSettings.height, 1);
        GameObject.Find("World").transform.position = position;

        return worldChunk;
    }
    public static void AssingChunkParent(GameObject[][,] chunk, string name, GameObject optionalParent = null)
    {
        GameObject parent = new GameObject(name);
        if (optionalParent != null)
        {
            parent.transform.parent = optionalParent.transform;
        }
        foreach (GameObject[,] array in chunk)
        {
            foreach (GameObject obj in array)
            {
                if (obj == null) continue;
                obj.transform.parent = parent.transform;
            }
        }
    }



    public static void ApplyMaterialToObject(GameObject obj, Material material)
    {
        if (!obj)
        {
            Debug.Log("Error: Retarded progremer of coordinates!");
            return;
        }
        if (obj.transform.childCount > 0)
        {
            //Debug.Log("kids hehe");
            foreach (Transform child in obj.transform)
            {
                child.gameObject.GetComponent<MeshRenderer>().material = material;
            }
        }
        else if (obj.GetComponent<MeshRenderer>())
        {
            //Debug.Log("4everlonely");
            obj.GetComponent<MeshRenderer>().material = material;
        }
    }
    public static Material SelectMaterial(float noiseVal, BasicTexturePack texturePack)
    {
        if (noiseVal < texturePack.values[1])
        {
            return texturePack.materials[1];
        }
        else if (noiseVal < texturePack.values[2])
        {
            return texturePack.materials[2];
        }
        else if (noiseVal < texturePack.values[3])
        {
            return texturePack.materials[3];
        }
        else if (noiseVal <= texturePack.values[4])
        {
            return texturePack.materials[4];
        }
        else if (noiseVal >= texturePack.values[5])
        {
            return texturePack.materials[5];
        }
        else
        {
            return texturePack.materials[0];
        }
    }
    public static bool ShapeCheck(MapSettings mapSettings, int yCurr, int xCurr)
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
    public static bool CheckVisibility(GameObject[,][][,] world, int chunkX, int chunkY, int layer, int posY, int posX, bool checkFlag = false)
    {
        //world[,][][,]
        GameObject[][,] currentChunk = world[chunkY, chunkX];
        //Debug.Log(posX + " " + posY);
        if (layer - 1 >= 0 && currentChunk[layer - 1][posX, posY] != null)
        {
            return false;
        }
        else if (checkFlag)
        {
            return true;
        }
        if (posX - 2 >= 0 && currentChunk[layer][posX - 2, posY] != null)
        {   //Top i guess
            SetActiveBlockSide(currentChunk[layer][posX, posY], 1);
        }
        if (posX + 2 < currentChunk[layer].GetLength(0) && currentChunk[layer][posX + 2, posY] != null)
        {   //Bot i guess
            SetActiveBlockSide(currentChunk[layer][posX, posY], -1);
        }
        if (posX % 2 == 0)
        {//parne
            if (posX - 1 >= 0 && currentChunk[layer][posX - 1, posY] != null)
            {   //TopLeft parne 0 neparne + 1
                SetActiveBlockSide(currentChunk[layer][posX, posY], 2);
            }
            if (posX - 1 >= 0 && posY - 1 >= 0 && currentChunk[layer][posX - 1, posY - 1] != null)
            {   //TopRight parne -1 neparne 0
                SetActiveBlockSide(currentChunk[layer][posX, posY], 3);
            }
            if (posX + 1 < currentChunk[layer].GetLength(0) && currentChunk[layer][posX + 1, posY] != null)
            {   //BotLeft parne 0 neparne + 1

                SetActiveBlockSide(currentChunk[layer][posX, posY], -2);
            }
            if (posX + 1 < currentChunk[layer].GetLength(0) && posY - 1 >= 0 && currentChunk[layer][posX + 1, posY - 1] != null)
            {   //BotRight i guess parne - 1 neparne 0
                SetActiveBlockSide(currentChunk[layer][posX, posY], -3);
            }
        }
        else
        {//neparne
            if (posX - 1 >= 0 && posY + 1 < currentChunk[layer].GetLength(1) && currentChunk[layer][posX - 1, posY + 1] != null)
            {   //TopLeft parne 0 neparne + 1
                SetActiveBlockSide(currentChunk[layer][posX, posY], 2);
            }
            if (posX - 1 >= 0 && currentChunk[layer][posX - 1, posY] != null)
            {   //TopRight parne -1 neparne 0
                SetActiveBlockSide(currentChunk[layer][posX, posY], 3);
            }
            if (posX + 1 < currentChunk[layer].GetLength(0) && posY + 1 < currentChunk[layer].GetLength(1) && currentChunk[layer][posX + 1, posY + 1] != null)
            {   //BotLeft parne 0 neparne + 1

                SetActiveBlockSide(currentChunk[layer][posX, posY], -2);
            }
            if (posX + 1 < currentChunk[layer].GetLength(0) && currentChunk[layer][posX + 1, posY] != null)
            {   //BotRight i guess parne - 1 neparne 0
                SetActiveBlockSide(currentChunk[layer][posX, posY], -3);
            }
        }

        return true;
    }
    public static void SetActiveBlockSide(GameObject block, int side, bool activeStatus = false)
    {
        switch (side)
        {
            case 1: // "top"
                block.transform.Find("top").gameObject.SetActive(activeStatus);
                return;
            case 2: // "topLeft"
                block.transform.Find("topLeft").gameObject.SetActive(activeStatus);
                return;
            case 3: // "topRight"
                block.transform.Find("topRight").gameObject.SetActive(activeStatus);
                return;
            case -1: // "bot"
                block.transform.Find("bot").gameObject.SetActive(activeStatus);
                return;
            case -2: // "botLeft"
                block.transform.Find("botLeft").gameObject.SetActive(activeStatus);
                return;
            case -3: // "botRight"
                block.transform.Find("botRight").gameObject.SetActive(activeStatus);
                return;
        }
    }
    public static void ChangeBlock(GameObject[,][][,] map, GameObject block, int chunkX, int chunkY, int layer, int x, int y)
    {
        GameObject.Destroy(map[chunkY, chunkX][layer][x, y]);
        map[chunkY, chunkX][layer][x, y] = block;
    }
    public static void DeleteWorld(Transform map)
    {
        foreach (Transform child in map)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    private static bool CheckForBlockInLayers(GameObject[,][][,] world, int currentLayer, int chunkX, int chunkY, int x, int y)
    {
        for (int i = currentLayer; i > 0; i--)
        {
            if (world[chunkY, chunkX][i][y, x]) return true;
        }
        return false;
    }
    public static void RegenerateNeighbours(GameObject[,][][,] world, Hexagon3Dproperties[,][][,] potentialWorld, int layer, int chunkX, int chunkY, Vector2Int positionInArray)
    {
        
        GameObject[,] map = world[0, 0][layer];
        GameObject[] neighbours = GetNeighbours(world, potentialWorld, layer, chunkX, chunkY, positionInArray);
        if (positionInArray.x + 2 < map.GetLength(0))
            RegenerateBlock(potentialWorld, world, neighbours, chunkY, chunkX, layer, positionInArray.y, positionInArray.x + 2, 0);
        if (positionInArray.x - 2 >= 0)
            RegenerateBlock(potentialWorld, world, neighbours, chunkY, chunkX, layer, positionInArray.y, positionInArray.x - 2, 1);

        if (positionInArray.x + 1 < map.GetLength(0))
            RegenerateBlock(potentialWorld, world, neighbours, chunkY, chunkX, layer, positionInArray.y, positionInArray.x + 1, 2);
        if (positionInArray.x -1 >= 0) 
            RegenerateBlock(potentialWorld, world, neighbours, chunkY, chunkX, layer, positionInArray.y, positionInArray.x - 1, 3);
        if (positionInArray.x % 2 == 0) // even row
        {
            if (positionInArray.x + 1 < map.GetLength(0) && positionInArray.y - 1 > 0)
                RegenerateBlock(potentialWorld, world, neighbours, chunkY, chunkX, layer, positionInArray.y - 1, positionInArray.x + 1, 4);
            if (positionInArray.x - 1 >= 0 && positionInArray.y - 1 > 0) 
                RegenerateBlock(potentialWorld, world, neighbours, chunkY, chunkX, layer, positionInArray.y - 1, positionInArray.x - 1, 5);
        }
        else// odd row
        {
            if (positionInArray.x + 1 < map.GetLength(0) && positionInArray.y + 1 < map.GetLength(1))
                RegenerateBlock(potentialWorld, world, neighbours, chunkY, chunkX, layer, positionInArray.y + 1, positionInArray.x + 1, 4);
            if (positionInArray.x - 1 >= 0 && positionInArray.y + 1 < map.GetLength(1))
                RegenerateBlock(potentialWorld, world, neighbours, chunkY, chunkX, layer, positionInArray.y + 1, positionInArray.x - 1, 5);
        }
    }
    public static void RegenerateBlock(Hexagon3Dproperties[,][][,] potentialWorld, GameObject[,][][,] world, GameObject[] neighbours, int chunkY, int chunkX, int layer, int y, int x, int index)
    {
        Vector3 pos;
        WorldGenerationHandler worldGenerationHandler = GameObject.Find("WorldHandler").GetComponent<WorldGenerationHandler>();

        if (layer < 0 || layer > 4)
        {
            Debug.Log("You are callculating wrong layer you incopetent punk.");
            return;
        }


        if (world[chunkY, chunkX][layer][x, y] == null)
        {//blok neni vytvoreny
            if(potentialWorld[chunkY, chunkX][layer][x, y] == null)
            {
                //Debug.Log("Already minned object");
                return;
            }//Block you looking at is already minned
            //Debug.Log("Creating new obj for walls");

            GameObject block = new GameObject("block " + x + " : " + y);
            GenerateSides(potentialWorld, block, chunkX, chunkY, layer, y, x, true);
            block.transform.position = potentialWorld[chunkY, chunkX][layer][x, y].position;
            ApplyMaterialToObject(block, SelectMaterial(worldGenerationHandler.noiseMap[chunkY, chunkX][x, y], worldGenerationHandler.texturePack));
            world[chunkY, chunkX][layer][x, y] = block;
            
            TrasferBlockToLayer(world, layer, chunkX, chunkY, x, y);

        }
        else
        {//blok je vytvoreny
            Debug.Log("Using already created obj for walls");


            //Vector3 pos;
            //WorldGenerationHandler worldGenerationHandler = GameObject.Find("WorldHandler").GetComponent<WorldGenerationHandler>();
            pos = world[chunkY, chunkX][layer][x, y].transform.position;
            world[chunkY, chunkX][layer][x, y].transform.position = Vector3.zero;
            GenerateSides(potentialWorld, world[chunkY, chunkX][layer][x, y], chunkX, chunkY, layer, y, x, true);
            world[chunkY, chunkX][layer][x, y].transform.position = pos;
            ApplyMaterialToObject(world[chunkY, chunkX][layer][x, y], SelectMaterial(worldGenerationHandler.noiseMap[chunkY, chunkX][x, y], worldGenerationHandler.texturePack));
        }
        //ApplyPotencialBlock(potentialWorld, world, chunkY, chunkX, layer, x, y, true);
        //world[0, 0][layer][x, y].transform.position = potentialWorld[0, 0][layer][x, y].position;


        //pos = neighbours[index].transform.position;
        //neighbours[index].transform.position = Vector3.zero;
        //GenerateSides(potentialWorld, neighbours[index], chunkY, chunkX, layer, y, x, true);
        //neighbours[index].transform.position = pos;
        //ApplyMaterialToObject(world[chunkY, chunkX][layer][x, y], SelectMaterial(worldGenerationHandler.noiseMap[chunkX, chunkY][x, y], worldGenerationHandler.texturePack));
    }
    public static GameObject[] GetNeighbours(GameObject[,][][,] world, Hexagon3Dproperties[,][][,] potentialWorld, int layer, int chunkX, int chunkY, Vector2Int positionInArray)
    {
        GameObject[] neighbours = new GameObject[6];
        GameObject[,] map = world[0, 0][layer];

        if (positionInArray.x + 2 < world[0, 0][layer].GetLength(0))   
            neighbours[0] = BuildWallOfHole(world, potentialWorld, layer, chunkX, chunkY, positionInArray.x + 2, positionInArray.y);
        if (positionInArray.x - 2 >= 0)
            neighbours[1] = BuildWallOfHole(world, potentialWorld, layer, chunkX, chunkY, positionInArray.x - 2, positionInArray.y);
        
        if (positionInArray.x + 1 < map.GetLength(0))
            neighbours[2] = BuildWallOfHole(world, potentialWorld, layer, chunkX, chunkY, positionInArray.x + 1, positionInArray.y); 
        if (positionInArray.x - 1 >= 0)
            neighbours[3] = BuildWallOfHole(world, potentialWorld, layer, chunkX, chunkY, positionInArray.x - 1, positionInArray.y);

        if (positionInArray.x % 2 == 0) // even row
        {
            if (positionInArray.x + 1 < map.GetLength(0) && positionInArray.y - 1 > 0)
                neighbours[4] = BuildWallOfHole(world, potentialWorld, layer, chunkX, chunkY, positionInArray.x + 1, positionInArray.y - 1); 
            if (positionInArray.x - 1 >= 0 && positionInArray.y - 1 > 0)
                neighbours[5] = BuildWallOfHole(world, potentialWorld, layer, chunkX, chunkY, positionInArray.x - 1, positionInArray.y - 1);
        }
        else// odd row
        {
            if (positionInArray.x + 1 < map.GetLength(0) && positionInArray.y + 1 < map.GetLength(1))
                neighbours[4] = BuildWallOfHole(world, potentialWorld, layer, chunkX, chunkY, positionInArray.x + 1, positionInArray.y + 1);
            if (positionInArray.x - 1 >= 0 && positionInArray.y + 1 < map.GetLength(1))
                neighbours[5] = BuildWallOfHole(world, potentialWorld, layer, chunkX, chunkY, positionInArray.x - 1, positionInArray.y + 1);
        }

        return neighbours;
    }
    public static GameObject BuildWallOfHole(GameObject[,][][,] world, Hexagon3Dproperties[,][][,] potentialWorld, int layer, int chunkX, int chunkY, int x, int y)
    {
        int currentLayer = GetLayer(world, chunkX, chunkY, y, x, world[0, 0].Length);
        if (currentLayer - (layer + 1) < -1)
        {
            currentLayer = layer;
        }
        return world[0, 0][currentLayer][x, y];
    }
    public static int GetLayer(GameObject[,][][,] world, int chunkX, int chunkY, int x, int y, int maxLayer)
    {
        for (int i = maxLayer - 1; i > 0; i--)
        {
            if (world[chunkY, chunkX][i][y, x] && world[chunkY, chunkX][i][y, x].transform.Find("HexagonTop")) return i;
        }
        return -1;
    }
    public static void TrasferBlockToLayer(GameObject[,][][,] world, int currentLayer, int chunkX, int chunkY, int x, int y)
    {
        switch (currentLayer)
        {
            case 0:
                world[0, 0][currentLayer][x, y].transform.parent = GameObject.Find("Chunk" + chunkY + " : " + chunkX).transform.Find("LayerOne").transform;
                break;
            case 1:
                world[0, 0][currentLayer][x, y].transform.parent = GameObject.Find("Chunk" + chunkY + " : " + chunkX).transform.Find("LayerTwo").transform;
                break;
            case 2:
                world[0, 0][currentLayer][x, y].transform.parent = GameObject.Find("Chunk" + chunkY + " : " + chunkX).transform.Find("LayerThree").transform;
                break;
            case 3:
                world[0, 0][currentLayer][x, y].transform.parent = GameObject.Find("Chunk" + chunkY + " : " + chunkX).transform.Find("LayerFour").transform;
                break;
            case 4:
                world[0, 0][currentLayer][x, y].transform.parent = GameObject.Find("Chunk" + chunkY + " : " + chunkX).transform.Find("LayerFive").transform;
                break;
        }
    }
}