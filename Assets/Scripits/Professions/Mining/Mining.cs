using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Unity.Mathematics;

public class Mining : MonoBehaviour
{

    private bool miningFlag = false;
    private WorldGenerationHandler worldGenerationHandler; // world is list of map
    private void Awake()
    {
        worldGenerationHandler = GameObject.Find("WorldHandler").GetComponent<WorldGenerationHandler>();
    }
    private void Update()
    {
        if (!miningFlag) return;
        if (Input.GetMouseButtonDown(0) && !GameObject.Find("GameHandler").GetComponent<Utils>().IsMouseOverUI())
        {
            Vector3 miningPosition = UtilsClass.GetMouseWorldPosition3D(0f);
            int layer = worldGenerationHandler.GetLayerFromPosition(miningPosition);
            Vector2Int positionInArray = worldGenerationHandler.GetNodeFromPosition(miningPosition).nodesParent.positionInArray;

            if (layer == 4) return;// bedrock

            GameObject blockToMine = worldGenerationHandler.world[0, 0][layer][positionInArray.x, positionInArray.y];

            Debug.Log(worldGenerationHandler.GetNodeFromPosition(miningPosition).nodesParent.positionInArray);
            Debug.Log(worldGenerationHandler.GetLayerFromPosition(miningPosition) + " " + miningPosition);
            if (Vector3.Distance(Vector3.zero, miningPosition) > 0)
            {
                Mine(blockToMine, layer, positionInArray);
            }
        }
    }
    public void SetMiningFlag(bool value)
    {
        if (value == true)
        {
            GameObject.Find("GameRTSControllerObject").GetComponent<GameRTSController>().SetUnitControlFlag(false);
            GameObject.Find("Examining").GetComponent<Examining>().SetExamineFlag(false);
            GameObject.Find("Building").GetComponent<Building>().SetBuildingFlag(false);
            GameObject.Find("Hunting").GetComponent<Hunting>().SetHuntingFlag(false);
        }
        miningFlag = value;
    }
    public void Mine(GameObject objectToMine, int layer, Vector2Int positionInArray)
    {
        Debug.Log("===================NEW BLOCK===================");
        int chunkX = 0;
        int chunkY = 0;
        if (!objectToMine)
        {
            Debug.Log("Not set to an instance of an object.");
            return;
        }
        worldGenerationHandler = GameObject.Find("WorldHandler").GetComponent<WorldGenerationHandler>();
        foreach (Transform child in objectToMine.transform)
        {
            Destroy(child.gameObject);
        }
        //Destroy(objectToMine);
        worldGenerationHandler.potentialWorld[0, 0][layer][positionInArray.x, positionInArray.y] = null;
        worldGenerationHandler.world[0, 0][layer][positionInArray.x, positionInArray.y] = null;

        //generate sides around new block
        WorldGenerator.RegenerateNeighbours(worldGenerationHandler.world, worldGenerationHandler.potentialWorld, layer, chunkX, chunkY, positionInArray);

        //generate new block
        layer++;

        WorldGenerator.ApplyPotencialBlock(worldGenerationHandler.potentialWorld, worldGenerationHandler.world, 0, 0, layer, positionInArray.x, positionInArray.y);
        worldGenerationHandler.world[0, 0][layer][positionInArray.x, positionInArray.y].transform.position = worldGenerationHandler.potentialWorld[0, 0][layer][positionInArray.x, positionInArray.y].position;
        WorldGenerator.ApplyMaterialToObject(worldGenerationHandler.world[0, 0][layer][positionInArray.x, positionInArray.y], WorldGenerator.SelectMaterial(worldGenerationHandler.noiseMap[0,0][positionInArray.x, positionInArray.y], worldGenerationHandler.texturePack));

        WorldGenerator.TrasferBlockToLayer(worldGenerationHandler.world, layer, chunkX, chunkY, positionInArray.x, positionInArray.y);
        //switch (layer)
        //{
        //    case 0:
        //        worldGenerationHandler.world[0, 0][layer][positionInArray.x, positionInArray.y].transform.parent = GameObject.Find("Chunk" + chunkY + " : " + chunkX).transform.Find("LayerOne").transform;
        //        break;
        //    case 1:
        //        worldGenerationHandler.world[0, 0][layer][positionInArray.x, positionInArray.y].transform.parent = GameObject.Find("Chunk" + chunkY + " : " + chunkX).transform.Find("LayerTwo").transform;
        //        break;
        //    case 2:
        //        worldGenerationHandler.world[0, 0][layer][positionInArray.x, positionInArray.y].transform.parent = GameObject.Find("Chunk" + chunkY + " : " + chunkX).transform.Find("LayerThree").transform;
        //        break;
        //    case 3:
        //        worldGenerationHandler.world[0, 0][layer][positionInArray.x, positionInArray.y].transform.parent = GameObject.Find("Chunk" + chunkY + " : " + chunkX).transform.Find("LayerFour").transform;
        //        break;
        //    case 4:
        //        worldGenerationHandler.world[0, 0][layer][positionInArray.x, positionInArray.y].transform.parent = GameObject.Find("Chunk" + chunkY + " : " + chunkX).transform.Find("LayerFive").transform;
        //        break;
        //}
    }
}
