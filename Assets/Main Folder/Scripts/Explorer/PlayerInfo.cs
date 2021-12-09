using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    #region DATA
    
    public String _name = "Explorator";
    public float _budDetectionRange = 5;
    public float exploringTimeForEachObject = 3;
    [Range(1.0f, 5.0f)] [SerializeField] public int _resurrectDuration;
    public float batteryCapacity = 100;
    private Vector3 _startingPoint;
    [NonSerialized] public bool needsToRecharge = false;
    [NonSerialized]public bool hasShovel = false;
    [NonSerialized]public bool isDead = false;

    #endregion

    public void Die()
    {
        isDead = true;
    }

    public void Resurrect()
    {
        isDead = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _budDetectionRange);
    }
}