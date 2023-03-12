using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockNodes
{
    private static float FLAT_NODE_DISTANCE = .35f;
    private static float POINTY_NODE_DISTANCE = .35f;
    private Node[] nodes;
    public Vector2Int positionInArray;
    public BlockNodes(Vector2Int positionInArray)
    {
        this.positionInArray = positionInArray;
    }
    public void GenerateFlatOnlyNodes(Vector3 position, bool walkable = false, bool buildable = false)
    {
        nodes = new Node[6];
        for (int i = 0; i < 6; i++)
        {
            nodes[i] = new Node(HexFlatTopCorner(position, FLAT_NODE_DISTANCE, i + 1), i, this, walkable, buildable);
        }
    }
    public void GeneratePointyOnlyNodes(Vector3 position, bool walkable = false, bool buildable = false)
    {
        nodes = new Node[6];
        for (int i = 0; i < 6; i++)
        {
            nodes[i] = new Node(HexPointyTopCorner(position, POINTY_NODE_DISTANCE, i + 1), i, this, walkable, buildable);
        }
    }
    public void GenerateAllNodeTypes(Vector3 position, bool walkable = false, bool buildable = false)
    {
        nodes = new Node[13]; //12 nod + stred
        nodes[12] = new Node(position, 12, this, walkable, buildable);
        for (int i = 0; i < 6; i++)
        {
            nodes[i * 2] = new Node(HexFlatTopCorner(position, FLAT_NODE_DISTANCE, 3 - i), i * 2, this, walkable, buildable); // rohy
            nodes[i * 2 + 1] = new Node(HexPointyTopCorner(position, POINTY_NODE_DISTANCE, 3 - i), i * 2 + 1, this, walkable, buildable); // stredy // + 6 kvoli tomu ze sa pouzivanato iny algoritmus
        }
    }



    public Vector3 HexFlatTopCorner(Vector3 center, float size, int i)
    {
        float angleDeg = 60 * i;
        float angleRad = Mathf.PI / 180 * angleDeg;

        return new Vector3(center.x + size * Mathf.Cos(angleRad),
                            center.y + size * Mathf.Sin(angleRad));
    }
    public Vector3 HexPointyTopCorner(Vector3 center, float size, int i)
    {
        float angleDeg = 60 * i - 30;
        float angleRad = Mathf.PI / 180 * angleDeg;

        return new Vector3(center.x + size * Mathf.Cos(angleRad),
                            center.y + size * Mathf.Sin(angleRad));
    }


    public Node[] GetNodes()
    {
        return nodes;
    }
    public Node GetNode(int index)
    {
        if (nodes[index] != null) return nodes[index];
        else return null;
    }
}
