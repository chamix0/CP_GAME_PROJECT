using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;


    private void Start()
    {
        cam1.enabled = true;
        cam2.enabled = false;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            cam1.enabled = false;
            cam2.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            cam1.enabled = true;
            cam2.enabled = false;
        }
    }
}