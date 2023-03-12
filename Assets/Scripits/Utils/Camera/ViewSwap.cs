using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ViewSwap : MonoBehaviour
{
    public GameObject cam3D;
    public GameObject cam2D;
    public GameObject camsObject;

    public void ResetCameraRotation()
    {
        var rot = camsObject.transform.rotation;
        rot.x = 0;
        rot.y = 0;
        rot.z = 0;
        camsObject.transform.rotation = rot;
    }
    public void SwapActiveCam()
    {
        /*
    if (hokusoPokuso.transform.localRotation == new Quaternion(60, 0, 0, 0))
    {
        hokusoPokuso.transform.localRotation = new Quaternion(0, 0, 0, 0);
    }
    else
    {
        Quaternion quaternion = hokusoPokuso.transform.rotation;
        quaternion.eulerAngles = new Vector3(30, 0, 0);
        hokusoPokuso.transform.rotation = quaternion;
        float smooth = 10.0f;
        float tiltAngle = -20.0f;
        // Smoothly tilts a transform towards a target rotation
        float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(tiltAroundX, 0, 0);
        hokusoPokuso.transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }*/
        cam2D.SetActive(!cam2D.activeSelf);
        cam3D.SetActive(!cam3D.activeSelf);
    }
}
