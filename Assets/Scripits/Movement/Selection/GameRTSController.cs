using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GameRTSController : MonoBehaviour {

    [SerializeField] private Transform selectionAreaTransform = null;

    private Vector3 startPosition;
    public List<UnitRTS> selectedUnitRTSList = null;
    public bool unitControlFlag;
    public int selectionAreaHeight = 3;

    private void Awake() {
        selectedUnitRTSList = new List<UnitRTS>();
        selectionAreaTransform.gameObject.SetActive(false);
        unitControlFlag = true;
    }

    private void Update() {
        if (!unitControlFlag) return;
        if (Input.GetMouseButtonDown(0) && !GameObject.Find("GameHandler").GetComponent<Utils>().IsMouseOverUI()) {
            // Left Mouse Button Pressed
            selectionAreaTransform.gameObject.SetActive(true);
            startPosition = UtilsClass.GetMouseWorldPosition3D(Mathf.Abs(Camera.main.transform.position.z));
        }

        if (Input.GetMouseButton(0)/* && !GameObject.Find("GameHandler").GetComponent<Utils>().IsMouseOverUI()*/) {
            // Left Mouse Button Held Down
            Vector3 currentMousePosition = UtilsClass.GetMouseWorldPosition3D(Mathf.Abs(Camera.main.transform.position.z));
            Vector3 lowerLeft = new Vector3(
                Mathf.Min(startPosition.x, currentMousePosition.x),
                Mathf.Min(startPosition.y, currentMousePosition.y),
                -2
            );
            Vector3 upperRight = new Vector3(
                Mathf.Max(startPosition.x, currentMousePosition.x),
                Mathf.Max(startPosition.y, currentMousePosition.y),
                lowerLeft.z + selectionAreaHeight
            );
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;
        }

        if ((Input.GetMouseButtonUp(0) && !GameObject.Find("GameHandler").GetComponent<Utils>().IsMouseOverUI() && selectionAreaTransform.gameObject.activeSelf) ||
            (Input.GetMouseButtonUp(0) && selectionAreaTransform.gameObject.activeSelf)) {
            // Left Mouse Button Released
            selectionAreaTransform.gameObject.SetActive(false);

            //Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, UtilsClass.GetMouseWorldPosition(Mathf.Abs(Camera.main.transform.position.z)));
            Vector3 halfExtent = (selectionAreaTransform.localScale) / 2;
            
            Collider[] colliderArray = Physics.OverlapBox(selectionAreaTransform.position + halfExtent, halfExtent);

            // Deselect all Units
            foreach (UnitRTS unitRTS in selectedUnitRTSList) {
                unitRTS.SetSelectedVisible(false);
            }
            selectedUnitRTSList.Clear();

            //Debug.Log(colliderArray.Length + " " + halfExtent);
            // Select Units within Selection Area
            foreach (Collider collider in colliderArray) {
                UnitRTS unitRTS = collider.GetComponent<UnitRTS>();
                if (unitRTS != null) {
                    unitRTS.SetSelectedVisible(true);
                    selectedUnitRTSList.Add(unitRTS);
                }
                else
                {
                    unitRTS = collider.gameObject.transform.parent.gameObject.GetComponent<UnitRTS>();
                    if (unitRTS != null)
                    {
                        unitRTS.SetSelectedVisible(true);
                        selectedUnitRTSList.Add(unitRTS);
                    }
                }
            }
            //Debug.Log(selectedUnitRTSList.Count);
        }

        if (Input.GetMouseButtonDown(1) && !GameObject.Find("GameHandler").GetComponent<Utils>().IsMouseOverUI())
        {
            // Right Mouse Button Pressed
            Vector3 moveToPosition = UtilsClass.GetMouseWorldPosition3D(Mathf.Abs(Camera.main.transform.position.z));

            List<Vector3> targetPositionList = GetPositionListAround(moveToPosition, new float[] { 10f, 20f, 30f }, new int[] { 5, 10, 20 });

            int targetPositionListIndex = 0;
            //Debug.Log("Moving to position: " + moveToPosition.y);
            float calculatedX = moveToPosition.x / 1.5f;
            calculatedX = calculatedX % 1 < .5f ? calculatedX : calculatedX - .5f;
            calculatedX = Mathf.RoundToInt(calculatedX);

            float calculatedY = moveToPosition.y / .866f;
            calculatedY = Mathf.RoundToInt(calculatedY);
            //float calculatedZ = 0;
            //Debug.Log("Calculated position is X: " + calculatedX +
            //    " Y: " + calculatedY +
            //    " Z: " + calculatedZ);
            foreach (UnitRTS unitRTS in selectedUnitRTSList)
            {
                unitRTS.MoveTo(moveToPosition);//targetPositionList[targetPositionListIndex]);
                targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
            }
            GameObject.Find("WorldHandler").GetComponent<WorldGenerationHandler>().DrawNode(GameObject.Find("WorldHandler").GetComponent<WorldGenerationHandler>().GetNodeFromPosition(moveToPosition));
        }
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray) {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray.Length; i++) {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount) {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++) {
            float angle = i * (360f / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }

    private Vector3 ApplyRotationToVector(Vector3 vec, float angle) {
        return Quaternion.Euler(0, 0, angle) * vec;
    }

    public void ReverseUnitControlFlag()
    {
        unitControlFlag = !unitControlFlag;
        GameObject.Find("Building").GetComponent<Building>().SetBuildingFlag(false);
        GameObject.Find("Examining").GetComponent<Examining>().SetExamineFlag(false);
        GameObject.Find("Mining").GetComponent<Mining>().SetMiningFlag(false);
        GameObject.Find("Hunting").GetComponent<Hunting>().SetHuntingFlag(false);
    }
    public void SetUnitControlFlag(bool value)
    {
        unitControlFlag = value;
    }
    public Transform GetSelection()
    {
        return selectionAreaTransform;
    }
}
