﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.right, Time.deltaTime * GameObject.Find("Watch").GetComponent<TimeClock>().GetTimeSpeed());
        transform.LookAt(Vector3.zero);
        
    }
}
