using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public float smoothTime = 0.3F;

    public Vector3 cameraOffset = new Vector3(0, 10, 0);
    private Vector3 velocity = Vector3.zero;
    private List<Transform> targets;
    private Transform currentTarget;
    private Camera camera;
    private int index = 0;

    private void Start()
    {
        targets = new List<Transform>();
        foreach (var bud in GameObject.FindGameObjectsWithTag("Player"))
        {
            targets.Add(bud.transform);
        }

        camera = GetComponent<Camera>();
        currentTarget = targets[index];
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            index = index - 1 < 0 ? targets.Count - 1 : index - 1;
            currentTarget = targets[index % targets.Count];
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            index++;
            currentTarget = targets[index % targets.Count];
        }

        // Define a target position above and behind the target transform
        Vector3 targetPosition = currentTarget.TransformPoint(cameraOffset);
        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        transform.LookAt(currentTarget);
    }
    
}