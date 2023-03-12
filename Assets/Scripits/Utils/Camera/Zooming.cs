using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Zooming : MonoBehaviour
{
    public CinemachineVirtualCamera cam3D;
    public CinemachineVirtualCamera cam2D;
    private float maxZoom3D = 20;
    private float minZoom3D = 50;
    private float maxZoom = 2;
    private float minZoom = 10;

    private void Update()
    {
        if (Input.mouseScrollDelta.y > 0) //if mousewheel is scrolling forwords function ZoomIn is active
        {
            ZoomIn();
        }
        if (Input.mouseScrollDelta.y < 0) //if mousewheel is scrolling backwords function ZoomOut is active
        {
            ZoomOut();
        }
    }
    /*
     Funtions ZoomIn and ZoomOut are changin FOV of currnet camera to zoom in and out.      
     */
     public void ZoomIn()
    {
        if (cam2D.isActiveAndEnabled)
        {
            ZoomInFov(cam2D, maxZoom);
        }
        else
        {
            ZoomInFov(cam3D, maxZoom3D);
        }
    }
    public void ZoomOut()
    {
        if (cam2D.isActiveAndEnabled)
        {
            ZoomOutFov(cam2D, minZoom);
        }
        else
        {
            ZoomOutFov(cam3D,minZoom3D);
        }
    }
    public void ZoomInFov(CinemachineVirtualCamera currCam, float maxZoom)
    {
        if (currCam.m_Lens.FieldOfView <= maxZoom) return;// if we already reached max zoom this will stop function
        currCam.m_Lens.FieldOfView--;//this is changing FOV of currnet camera to zoomin
    }
    public void ZoomOutFov(CinemachineVirtualCamera currCam, float minZoom)
    {
        if (currCam.m_Lens.FieldOfView >= minZoom) return;// if we already reached min zoom this will stop function
        currCam.m_Lens.FieldOfView++;//this is changing FOV of currnet camera to zoomout
    }
}

/*Camera shitters
 * 2Dcamera settings
 * max z -4000 min -1000 FOV = 1 pre 2D kameru
 * minFov 3 maxFov 10
 * defaultZ-400
 * 
 * 3D kamera default settings
 * rotation x -45 zooming by changing FOV (default 40) max 50 min 20
 * z -30 default
 */
