using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorableObject : MonoBehaviour
{
    public Material mat;
    private bool explored = false; 

    
    public Vector3 getPosition()
    {
        return transform.position;
    }

    public void setExplored()
    {
        explored = true;
        GetComponentInChildren<MeshRenderer>().material = mat;
    }
}
