using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Examining : MonoBehaviour
{
    private bool examineFlag = false;
    private WorldGenerationHandler worldGenerationHandler;
    private void Awake()
    {
        worldGenerationHandler = GameObject.Find("WorldHandler").GetComponent<WorldGenerationHandler>();
    }
    private void Update()
    {
        if (!examineFlag) return;
        if (Input.GetMouseButtonDown(0) && !GameObject.Find("GameHandler").GetComponent<Utils>().IsMouseOverUI())
        {
            //RaycastHit hit;
            Vector3 position = UtilsClass.GetMouseWorldPosition3D(Mathf.Abs(Camera.main.transform.position.z));
            position.x += worldGenerationHandler.mapSettings.width;
            position.y += worldGenerationHandler.mapSettings.height;
            Debug.Log(position);
            Examine(position);
        }
    }
    public void SetExamineFlag(bool value)
    {
        if (value == true)
        {
            GameObject.Find("GameRTSControllerObject").GetComponent<GameRTSController>().SetUnitControlFlag(false);
            GameObject.Find("Building").GetComponent<Building>().SetBuildingFlag(false);
            GameObject.Find("Mining").GetComponent<Mining>().SetMiningFlag(false);
            GameObject.Find("Hunting").GetComponent<Hunting>().SetHuntingFlag(false);
        }
        examineFlag = value;
    }
    public void Examine(Vector3 position)
    {
        GameObject[][,] mapsArray = worldGenerationHandler.mapsArray;
        int x, y;
        x = (int)(position.x / 2) + (int)position.x % 2;
        y = (int)(position.y / 2) + (int)position.y % 2;
        Debug.Log(mapsArray[0].GetLength(0) + " " + mapsArray[0].GetLength(1) + " " + x + " " + y);
        if (mapsArray[0][x, y] != null)
        {
            Debug.Log(mapsArray[0][x, y].transform.name);
        }
        else if (mapsArray[1][x, y] != null)
        {
            Debug.Log(mapsArray[1][x, y].transform.name);
        }
        else if (mapsArray[2][x, y] != null)
        {
            Debug.Log(mapsArray[2][x, y].transform.name);
        }
        else if (mapsArray[3][x, y] != null)
        {
            Debug.Log(mapsArray[3][x, y].transform.name);
        }
        else if (mapsArray[4][x, y] != null)
        {
            Debug.Log(mapsArray[4][x, y].transform.name);
        }
        else
        {
            Debug.Log("none blocks of the above wasnt found");
        }
    }
}
