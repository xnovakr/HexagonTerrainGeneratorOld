using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroapHolder : MonoBehaviour
{
    private List<UnitRTS> selectedUnitRTSList;
    private void Awake()
    {
        selectedUnitRTSList = new List<UnitRTS>();
    }
    public void SaveGroap()
    {
        selectedUnitRTSList.Clear();
        foreach (UnitRTS unit in GameObject.Find("GameRTSControllerObject").GetComponent<GameRTSController>().selectedUnitRTSList)
        {
            selectedUnitRTSList.Add(unit);
        }
        //selectedUnitRTSList = GameObject.Find("GameRTSControllerObject").GetComponent<GameRTSController>().selectedUnitRTSList;
    }
    public void DeleteGroap()
    {
        foreach (UnitRTS unit in selectedUnitRTSList)
        {
            unit.SetSelectedVisible(false);
        }
        selectedUnitRTSList = new List<UnitRTS>();
        GameObject.Find("GameRTSControllerObject").GetComponent<GameRTSController>().selectedUnitRTSList.Clear();
    }
    public void LoadGroap()
    {
        foreach (UnitRTS unit in selectedUnitRTSList)
        {
            unit.SetSelectedVisible(true);
            GameObject.Find("GameRTSControllerObject").GetComponent<GameRTSController>().selectedUnitRTSList.Add(unit);
        }
        //GameObject.Find("GameRTSControllerObject").GetComponent<GameRTSController>().selectedUnitRTSList = selectedUnitRTSList;
    }
}
