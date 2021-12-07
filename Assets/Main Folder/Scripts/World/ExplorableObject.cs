using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorableObject : MonoBehaviour
{
    public Material matC, matNoC;
    private bool containsOnject;
    private WorldManager.ObjectTypes type;


    public Vector3 getPosition()
    {
        return transform.position;
    }

    public bool getContainsObject()
    {
        return containsOnject;
    }

    public void setContainsObject(bool contains)
    {
        containsOnject = contains;
    }

    public void setType(WorldManager.ObjectTypes type)
    {
        this.type = type;
    }

    public WorldManager.ObjectTypes getType()
    {
        return type;
    }

    public void setExplored()
    {
        if (containsOnject)
        {
            GetComponentInChildren<MeshRenderer>().material = matC;
            containsOnject = false;
        }
        else
        {
            GetComponentInChildren<MeshRenderer>().material = matNoC;
        }
    }
}