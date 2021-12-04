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
    private Vector3 _startingPoint;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
    }
}