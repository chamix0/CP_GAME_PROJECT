using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bookColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.color=Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }
    
}
