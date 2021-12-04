using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomNamePerception : Perception {

    //Evaluates wether it should fire this perception or not
    public override bool Check()
    {
        return false;
    }

    //Called when the transition launches to restore any variables (if you need to)
    public override void Reset()
    {
        return;
    }
}