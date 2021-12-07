using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    #region DATA

    public String _name="Explorator";
    public float _monsterDetectionRange = 5;
    public float _budDetectionRange = 5;
    public float exploringTimeForEachObject = 3;
    public float updatingTimeForEachBud = 5;
    public float batteryCapacity = 100;
    private Vector3 _startingPoint;
    [NonSerialized] public bool needsToRecharge=false; 

    #endregion

    // Start is called before the first frame update
    void Start()
    {
    }
}