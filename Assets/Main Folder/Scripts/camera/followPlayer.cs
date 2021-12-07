using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public float smoothTime = 0.3F;
    public Vector3 cameraOffset=new Vector3(0, 10, 0);
    private Vector3 velocity = Vector3.zero;
    private Transform target;
    private Camera camera;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.TransformPoint(cameraOffset);
        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        transform.LookAt(target);
    }
}