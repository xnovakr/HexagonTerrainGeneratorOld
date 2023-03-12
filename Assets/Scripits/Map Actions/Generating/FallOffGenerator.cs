using System.Collections;
using UnityEngine;

public class FallOffGenerator
{
    public static float[,] GenerateFallOffMap(int sizeX, int sizeY, int perspective = -1, int side = -1, int corner = -1)
    {//perspective 0 = x 1 = y else = both  //side 0 = left/bot 1 = right/top else == both // corner 0 = bot+left 1 = bot+right 2 = top+left 3 = top+right else = all
        float[,] map = new float[sizeY, sizeX];

        for(int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                float x = i / (float)sizeY * 2 - 1;
                float y = j / (float)sizeX * 2 - 1;

                if (perspective == 0)
                {
                    x = 0;

                    if (j < sizeX / 2 && side == 1) continue;
                    else if (j > sizeX / 2 && side == 0) continue;
                }
                else if (perspective == 1)
                {
                    y = 0;

                    if (i < sizeY / 2 && side == 1) continue;
                    else if (i > sizeY / 2 && side == 0) continue;
                }

                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                map[i, j] = Evaluate(value);
            }
        }

        if (corner == 0)
        {
            map = CombineMaps(GenerateFallOffMap(sizeX, sizeY, 0, 0), GenerateFallOffMap(sizeX, sizeY, 1, 0));
        }
        else if (corner == 1)
        {
            map = CombineMaps(GenerateFallOffMap(sizeX, sizeY, 0, 0), GenerateFallOffMap(sizeX, sizeY, 1, 1));
        }
        else if (corner == 2)
        {
            map = CombineMaps(GenerateFallOffMap(sizeX, sizeY, 0, 1), GenerateFallOffMap(sizeX, sizeY, 1, 0));
        }
        else if (corner == 3)
        {
            map = CombineMaps(GenerateFallOffMap(sizeX, sizeY, 0, 1), GenerateFallOffMap(sizeX, sizeY, 1, 1));
        }
        return map;
    }
    static float Evaluate(float value)
    {
        float a = 3f;//3f;
        float b = 2.2f;//2.2f;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
    static float[,] CombineMaps(float[,] map1, float[,] map2)
    {
        float[,] map = new float[map1.GetLength(0), map1.GetLength(1)];

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                float value = Mathf.Max(Mathf.Abs(map1[i, j]), Mathf.Abs(map2[i, j]));
                map[i, j] = Evaluate(value);
            }
        }

        return map;
    }
}
