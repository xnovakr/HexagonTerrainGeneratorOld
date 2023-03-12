using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Hunting : MonoBehaviour
{
    private bool huntingFlag = false;

    [SerializeField] private Transform selectionAreaTransform = null;

    private Vector3 startPosition;
    public bool unitControlFlag;

    // Update is called once per frame
    void Update()
    {
        if (!huntingFlag) return;

        if (Input.GetMouseButtonDown(0) && !GameObject.Find("GameHandler").GetComponent<Utils>().IsMouseOverUI())
        {
            // Left Mouse Button Pressed
            selectionAreaTransform.gameObject.SetActive(true);
            startPosition = UtilsClass.GetMouseWorldPosition3D(Mathf.Abs(Camera.main.transform.position.z));
        }

        if (Input.GetMouseButton(0) && !GameObject.Find("GameHandler").GetComponent<Utils>().IsMouseOverUI())
        {
            // Left Mouse Button Held Down
            Vector3 currentMousePosition = UtilsClass.GetMouseWorldPosition3D(Mathf.Abs(Camera.main.transform.position.z));
            Vector3 lowerLeft = new Vector3(
                Mathf.Min(startPosition.x, currentMousePosition.x),
                Mathf.Min(startPosition.y, currentMousePosition.y)
            );
            Vector3 upperRight = new Vector3(
                Mathf.Max(startPosition.x, currentMousePosition.x),
                Mathf.Max(startPosition.y, currentMousePosition.y)
            );
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;
        }

        if (Input.GetMouseButtonUp(0) && !GameObject.Find("GameHandler").GetComponent<Utils>().IsMouseOverUI())
        {
            // Left Mouse Button Released
            selectionAreaTransform.gameObject.SetActive(false);
            //thats it pobably///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, UtilsClass.GetMouseWorldPosition3D(Mathf.Abs(Camera.main.transform.position.z)));


            // Select Units within Selection Area
            foreach (Collider2D collider2D in collider2DArray)
            {
                UnitRTS unitRTS = collider2D.GetComponent<UnitRTS>();
                if (unitRTS != null)
                {
                    unitRTS.SetSelectedVisible(true);
                    Destroy(unitRTS.gameObject);
                }
            }
        }
    }

    public void SetHuntingFlag(bool value)
    {
        if (value == true)
        {
            GameObject.Find("GameRTSControllerObject").GetComponent<GameRTSController>().SetUnitControlFlag(false);
            GameObject.Find("Examining").GetComponent<Examining>().SetExamineFlag(false);
            GameObject.Find("Building").GetComponent<Building>().SetBuildingFlag(false);
            GameObject.Find("Mining").GetComponent<Mining>().SetMiningFlag(false);
        }
        huntingFlag = value;
    }
}
