using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    private bool openDoor =false;
    public float degrees=90;

    // Update is called once per frame
    void Update()
    {
        if (openDoor)
        {
            Vector3 to = new Vector3(degrees, 0, 0);
            transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, Time.deltaTime);
        }
    }

    public void openTheDoor()
    {
        openDoor = true;
    }
}