using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class BlockSelector : MonoBehaviour
{
    public Material material;
    public Sprite materialPrefab;
    private void OnMouseDown()
    {
        GameObject.Find("Building").GetComponent<Building>().SetCurrentTexture(material, materialPrefab);
        Debug.Log(this.transform.name);
        GameObject.Find("Building").GetComponent<Building>().SetBuildingFlag(true);
    }
}
