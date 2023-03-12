using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Controls : MonoBehaviour
{
    float speed = 20f;
    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(transform.up.y, -transform.up.x, 0) * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(transform.up.y, -transform.up.x, 0) * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.up * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, 0, Time.deltaTime * speed, Space.World);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, 0, -Time.deltaTime * speed, Space.World); //GetComponent<ViewSwap>().GetActiveCamera().
        }
    }
}