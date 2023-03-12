using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCameraHandler : MonoBehaviour
{
    private Vector3 ORIGIN_POSITION = new Vector3(75, 48, -100);

    private new Camera camera;
    public Slider slider;
    public Slider axisX;
    public Slider axisY;
    public int mapSize = 1;
    private void Awake()
    {
        if (GameObject.Find("MapObject"))
        {
            ORIGIN_POSITION = new Vector3(610, 360, -800);
        }
        camera = GetComponent<Camera>();
        SetupMapCamera();
    }
    public void SetupMapCamera()
    {
        if (GameObject.Find("NewGame"))
        {
            mapSize = GameObject.Find("TerrainGenerator").GetComponent<TerrainGenerator>().mapSize;
            camera.transform.position = ORIGIN_POSITION * mapSize;
        }
        else if (GameObject.Find("WorldHandler"))
        {
            mapSize = GameObject.Find("WorldHandler").GetComponent<WorldGenerationHandler>().mapSize;
        }
        axisX.maxValue = ORIGIN_POSITION.x * (mapSize + 2);
        axisY.maxValue = ORIGIN_POSITION.y * (mapSize + 2);
        camera = GetComponent<Camera>();
    }
    private void Update()
    {
        if (slider != null)
        {
            if (slider != null && slider.value != camera.fieldOfView)
            {
                slider.value = camera.fieldOfView;
            }
            if (axisX != null && axisX.value != camera.transform.position.x)
            {
                axisX.value = camera.transform.position.x;
            }
            if (axisY != null && axisY.value != camera.transform.position.y)
            {
                axisY.value = camera.transform.position.y;
            }
        }
        //if (GameObject.Find("NewGame"))
        //{
        //}
    }
    public void ChangeFov(int value)
    {
        if (value < 0 && camera.fieldOfView <= 5)
        {
            return;
        }
        else if (value > 0 && camera.fieldOfView >= 55)
        {
            return;
        }
        camera.fieldOfView += value;
    }
    public void ChangeFovSlider()
    {
        camera.fieldOfView = slider.value;
    }
    public void MoveAxisX()
    {
        Vector3 position = camera.transform.position;
        position.x = axisX.value;
        camera.transform.position = position;
    }
    public void MoveAxisY()
    {
        Vector3 position = camera.transform.position;
        position.y = axisY.value;
        camera.transform.position = position;
    }
}
