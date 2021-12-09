using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour
{
  
    private Camera currentCamera;

    private int index;

    public List<Camera> cams;

    private void Start()
    {
        index = 0;
        currentCamera = cams[index];
        currentCamera.enabled = true;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            index = index - 1 < 0 ? cams.Count - 1 : index - 1;
            currentCamera = cams[index % cams.Count];
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            index++;
            currentCamera = cams[index % cams.Count];
        }

        foreach (var c in cams)
        {
            if (c != currentCamera)
            {
                c.enabled = false;
            }
            else
            {
                c.enabled = true;
            }
        }
    }
}