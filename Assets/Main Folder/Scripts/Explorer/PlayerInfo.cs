using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    #region DATA
    
    public String _name = "Explorator";
    [Range(1,10)] public float _budDetectionRange = 5;
    [Range(1,100)] public float _budResurrectionRange = 5;
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
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _budDetectionRange);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _budResurrectionRange);
    }
}