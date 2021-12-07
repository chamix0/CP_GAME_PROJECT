using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chargingPoint : MonoBehaviour
{
    private bool beingUsed = false;

    public bool isBeingUsed()
    {
        return beingUsed;
    }

    public void use()
    {
        beingUsed = true;
    }

    public void stopUsing()
    {
        beingUsed = false;
    }

}
