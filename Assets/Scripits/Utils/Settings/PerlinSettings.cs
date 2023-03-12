using UnityEngine;
using System;


[Serializable]
public class PerlinSettings
{
    public bool active = true;

    public int seed;

    [Range(-2f, 2f)]
    public float lacunarity = 2;

    [Range(1, 10)]
    public int octaves = 10;


    public float weightMultiplier = .8f;
    public float persistance = .5f;
    public float strenght = 1;
    public float frequency = 1;
    public float offsetX = 100;
    public float offsetY = 100;
    public float scale = 10;
}
