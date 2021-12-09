using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flame : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera m_camera;
    void Start()
    {
        m_camera=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(m_camera.transform,Vector3.up);
        
    }
}
