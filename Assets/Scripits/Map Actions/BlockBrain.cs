using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBrain : MonoBehaviour
{
    public float blockSize;
    public float blockNoiseValue;
    public Vector3 originPoint;
    private Vector3 position;

    private void Update()
    {
        position = transform.position;
        position.x = Mathf.Floor((0.01f + transform.position.x) / blockSize) * blockSize;
        position.y = Mathf.Floor((0.01f + transform.position.y) / blockSize) * blockSize;
        transform.position = position;
    }
}
