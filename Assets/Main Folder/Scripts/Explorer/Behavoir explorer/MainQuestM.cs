using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.AI;

public class MainQuestM : MonoBehaviour
{
    #region variables

    private StateMachineEngine MainQuestFSM_FSM;
    private PushPerception ThereisobjectPerception;
    private PushPerception objectneededPerception;
    private PushPerception objectnotneededPerception;
    private PushPerception gobacktoexplorePerception;
    private PushPerception backtoexplorePerception;
    private PushPerception budonrangePerception;
    private PushPerception budisdeadPerception;
    private PushPerception budisokPerception;
    private PushPerception budreanimatedPerception;
    private PushPerception bactoexplorePerception;
    private PushPerception nomoreplacestolookPerception;
    private PushPerception MoreplacestolookPerception;
    private PushPerception QuestfinishedPerception;
    private PushPerception keepexploringPerception;
    private PushPerception itemnotplacedPerception;
    private State lookingforanobject;
    private State objectfound;
    private State takeobjecttotheplace;
    private State leaveobject;
    private State checkonbud;
    private State reanimate;
    private State updateinfo;
    private State waitforthedoortoopen;
    private State waiting;

    //Place your variables here
    private CharacterController mainCharacter;
    private String transition;

    #endregion variables

    // Start is called before the first frame update


    private void Start()
    {
        MainQuestFSM_FSM = new StateMachineEngine(false);
        mainCharacter = GetComponent<CharacterController>();
        transition = "";
        CreateStateMachine();
    }


    private void CreateStateMachine()
    {
        // Perceptions
        // Modify or add new Perceptions, see the guide for more
        ThereisobjectPerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();
        objectneededPerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();
        objectnotneededPerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();
        gobacktoexplorePerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();
        backtoexplorePerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();
        budonrangePerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();
        budisdeadPerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();
        budisokPerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();
        budreanimatedPerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();
        bactoexplorePerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();
        nomoreplacestolookPerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();
        MoreplacestolookPerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();
        QuestfinishedPerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();
        keepexploringPerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();
        itemnotplacedPerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();

        // States
        lookingforanobject = MainQuestFSM_FSM.CreateEntryState("Looking for an object", lookingforanobjectAction);
        objectfound = MainQuestFSM_FSM.CreateState("Object found", objectfoundAction);
        takeobjecttotheplace = MainQuestFSM_FSM.CreateState("Take object to the place", takeobjecttotheplaceAction);
        leaveobject = MainQuestFSM_FSM.CreateState("Leave object", leaveobjectAction);
        checkonbud = MainQuestFSM_FSM.CreateState("Check on bud", checkonbudAction);
        reanimate = MainQuestFSM_FSM.CreateState("Reanimate", reanimateAction);
        updateinfo = MainQuestFSM_FSM.CreateState("Update info", updateinfoAction);
        waitforthedoortoopen = MainQuestFSM_FSM.CreateState("Wait for the door to open", waitforthedoortoopenAction);
        waiting = MainQuestFSM_FSM.CreateState("Waiting", waitingAction);

        // Transitions
        MainQuestFSM_FSM.CreateTransition("there is object", lookingforanobject, ThereisobjectPerception, objectfound);
        MainQuestFSM_FSM.CreateTransition("object needed", objectfound, objectneededPerception, takeobjecttotheplace);
        MainQuestFSM_FSM.CreateTransition("object not needed", objectfound, objectnotneededPerception, leaveobject);
        MainQuestFSM_FSM.CreateTransition("go back to explore", leaveobject, gobacktoexplorePerception,
            lookingforanobject);
        MainQuestFSM_FSM.CreateTransition("back to explore", takeobjecttotheplace, backtoexplorePerception,
            lookingforanobject);
        MainQuestFSM_FSM.CreateTransition("bud on range", lookingforanobject, budonrangePerception, checkonbud);
        MainQuestFSM_FSM.CreateTransition("bud is dead", checkonbud, budisdeadPerception, reanimate);
        MainQuestFSM_FSM.CreateTransition("bud is ok", checkonbud, budisokPerception, updateinfo);
        MainQuestFSM_FSM.CreateTransition("bud reanimated", reanimate, budreanimatedPerception, updateinfo);
        MainQuestFSM_FSM.CreateTransition("bac to explore", updateinfo, bactoexplorePerception, lookingforanobject);
        MainQuestFSM_FSM.CreateTransition("no more places to look", lookingforanobject, nomoreplacestolookPerception,
            waiting);
        MainQuestFSM_FSM.CreateTransition("More places to look", waiting, MoreplacestolookPerception,
            lookingforanobject);
        MainQuestFSM_FSM.CreateTransition("quest finished", waiting, QuestfinishedPerception, waitforthedoortoopen);
        MainQuestFSM_FSM.CreateTransition("keep exploring", lookingforanobject, keepexploringPerception,
            lookingforanobject);
        MainQuestFSM_FSM.CreateTransition("item not placed", takeobjecttotheplace, itemnotplacedPerception,
            takeobjecttotheplace);

        // ExitPerceptions

        // ExitTransitions
    }

    // Update is called once per frame
    private void Update()
    {
        MainQuestFSM_FSM.Update();
        MainQuestFSM_FSM.Fire(transition);
    }

    // Create your desired actions

    private void lookingforanobjectAction()
    {
        Debug.Log("looking for an object", this);
        if (mainCharacter.iKnowWhereThatObjectIs())
        {
            Debug.Log("i know where that object is!!!", this);
            mainCharacter.goToObjectIknow();
            mainCharacter.actionManagerTextTime();
            if (mainCharacter.destinationReached())
            {
                Debug.Log("object found", this);
                transition = "there is object";
            }
            else
            {
                Debug.Log("destination not reached", this);
                transition = "keep exploring";
            }
        }
        else
        {
            if (mainCharacter.findRandomExplorablePlace())
            {
                mainCharacter.actionManagerTextTime();
                if (mainCharacter.destinationReached())
                {
                    if (mainCharacter.currentTarget.getContainsObject())
                    {
                        Debug.Log("object found!!!!", this);
                        transition = "there is object";
                    }
                    else
                    {
                        Debug.Log("this place doesnt contain anything", this);
                        mainCharacter.changeToExplored();
                        transition = "keep exploring";
                    }
                }
                else
                {
                    Debug.Log("destination not reached", this);
                    transition = "keep exploring";
                }
            }
            else
            {
                Debug.Log("no more places to look", this);
                transition = "no more places to look";
            }
        }
    }

    private void objectfoundAction()
    {
        if (mainCharacter.isMyObjectNeeded())
        {
            Debug.Log("I need this object", this);
            mainCharacter.worldManager.advanceOnTask();
            mainCharacter.changeToExplored();
            transition = "object needed";
        }
        else
        {
            Debug.Log("I dont need this object", this);
            transition = "object not needed";
        }
    }

    private void takeobjecttotheplaceAction()
    {
        Debug.Log("taking this object to the platform", this);

        mainCharacter.takeObjectToBombPlatform();
        if (mainCharacter.destinationReached())
        {
            Debug.Log("platform reached", this);
            mainCharacter.actionManagerTextTime();
            mainCharacter.objectFound.GetComponent<MeshRenderer>().enabled = false;
            transition = "back to explore";
        }
        else
        {
            Debug.Log("platfrom not reached", this);
            transition = "item not placed";
        }
    }

    private void leaveobjectAction()
    {
        Debug.Log("I might need this object later", this);
        mainCharacter.actionManagerTextTime();
        mainCharacter.addObjectTocontainsAnObjectList();
        mainCharacter.changeToExplored();
        transition = "go back to explore";
    }

    private void checkonbudAction()
    {
    }

    private void reanimateAction()
    {
    }

    private void updateinfoAction()
    {
    }

    private void waitforthedoortoopenAction()
    {
    }

    private void waitingAction()
    {
    }
}