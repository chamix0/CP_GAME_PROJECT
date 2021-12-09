using System;

using UnityEngine;

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
    private PushPerception waitingfPerception;
    private State lookingforanobject;
    private State objectfound;
    private State takeobjecttotheplace;
    private State leaveobject;
    private State reanimate;
    private State waitforthedoortoopen;
    private State waiting;

    #endregion variables

    //Place your variables here
    private CharacterManager mainCharacter;
    private String transition;
    private bool paused = false;
    private ExplorableObject carryingObject;

    // Start is called before the first frame update


    private void Start()
    {
        MainQuestFSM_FSM = new StateMachineEngine(false);
        mainCharacter = GetComponent<CharacterManager>();
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
        waitingfPerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();

        // States
        lookingforanobject = MainQuestFSM_FSM.CreateEntryState("Looking for an object", lookingforanobjectAction);
        objectfound = MainQuestFSM_FSM.CreateState("Object found", objectfoundAction);
        takeobjecttotheplace = MainQuestFSM_FSM.CreateState("Take object to the place", takeobjecttotheplaceAction);
        leaveobject = MainQuestFSM_FSM.CreateState("Leave object", leaveobjectAction);
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
        MainQuestFSM_FSM.CreateTransition("no more places to look", lookingforanobject, nomoreplacestolookPerception,
            waiting);
        MainQuestFSM_FSM.CreateTransition("more places to look", waiting, MoreplacestolookPerception,
            lookingforanobject);
        MainQuestFSM_FSM.CreateTransition("quest finished", waiting, QuestfinishedPerception, waitforthedoortoopen);
        MainQuestFSM_FSM.CreateTransition("keep exploring", lookingforanobject, keepexploringPerception,
            lookingforanobject);
        MainQuestFSM_FSM.CreateTransition("keep waiting", waiting, waitingfPerception,
            waiting);
        MainQuestFSM_FSM.CreateTransition("item not placed", takeobjecttotheplace, itemnotplacedPerception,
            takeobjecttotheplace);

        // ExitPerceptions

        // ExitTransitions
    }

    // Update is called once per frame
    private void Update()
    {
        if (!paused)
        {
            MainQuestFSM_FSM.Update();
            MainQuestFSM_FSM.Fire(transition);
        }
    }

    // Create your desired actions
    private void lookingforanobjectAction()
    {
        if (mainCharacter.worldManager.getAllItemsFound())
        {
            mainCharacter.PrintLabel("no more places to look");
            //Debug.Log("no more places to look", this);
            transition = "no more places to look";
            if (mainCharacter.playerInfo.hasShovel)
            {
                exploring();
                if (mainCharacter.worldManager.getAllItemsPlaced())
                {
                    transition = "no more places to look";
                }
            }
        }
        else exploring();
    }

    private void objectfoundAction()
    {
        if (mainCharacter.isMyObjectNeeded())
        {
            if (mainCharacter.currentTarget == mainCharacter.worldManager.getShovel())
            {
                mainCharacter.playerInfo.hasShovel = true;
            }

            mainCharacter.PrintLabel("I need this object");
            carryingObject = mainCharacter.worldManager.advanceOnTask();
            mainCharacter.changeToExplored();
            transition = "object needed";
        }
        else
        {
            mainCharacter.PrintLabel("I dont need this object now");
            //Debug.Log("I dont need this object", this);
            transition = "object not needed";
        }
    }


    private void takeobjecttotheplaceAction()
    {
        Debug.Log("taking this object to the platform", this);


        mainCharacter.takeObjectToBombPlatform();
        if (mainCharacter.destinationReached())
        {
            mainCharacter.PrintLabel("Dropping object");
            if (mainCharacter.worldManager.phase == 8)
                mainCharacter.worldManager.advanceOnTask();
            Debug.Log("platform reached", this);

            mainCharacter.stopAction();
            mainCharacter.worldManager.putObjectInPlatform(carryingObject);
            mainCharacter.objectFound.GetComponent<MeshRenderer>().enabled = false;
            carryingObject = null;
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
        //mainCharacter.PrintLabel("I might need this object later");
        //Debug.Log("I might need this object later", this);
        mainCharacter.addObjectTocontainsAnObjectList();
        mainCharacter.changeToExplored();
        transition = "go back to explore";
    }

    private void waitforthedoortoopenAction()
    {
    }

    private void waitingAction()
    {
        if (mainCharacter.worldManager.getExploted())
        {
            transition = "quest finished";
        }
        else if (mainCharacter.findRandomExplorablePlace() && !mainCharacter.worldManager.getAllItemsFound())
        {
            transition = "more places to look";
        }
        else
        {
            mainCharacter.goToWaitingPoint();
            transition = "keep waiting";
        }
    }

    // my methods
    public void pause()
    {
        paused = true;
    }

    public void unPause()
    {
        paused = false;
    }

    private void exploring()
    {
        if (mainCharacter.iKnowWhereThatObjectIs())
        {
            mainCharacter.PrintLabel("I remember that!");
            //Debug.Log("i know where that object is!!!", this);
            mainCharacter.goToObjectIknow();
            if (mainCharacter.destinationReached())
            {
                mainCharacter.stopAction();
                mainCharacter.PrintLabel("Object found");
                //Debug.Log("object found", this);
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
                mainCharacter.PrintLabelBig("Exploring");
                checkArrival();
            }
            else
            {
                mainCharacter.PrintLabel("No more places to look");
                Debug.Log("no more places to look", this);
                transition = "no more places to look";
            }
        }
    }

    public void checkArrival()
    {
        if (mainCharacter.destinationReached())
        {
            mainCharacter.stopAction();
            mainCharacter.PrintLabel("Searching...");
            if (mainCharacter.currentTarget.getContainsObject())
            {
                mainCharacter.PrintLabel("Object found");
                Debug.Log("object found!!!!", this);
                transition = "there is object";
            }
            else
            {
                mainCharacter.PrintLabel("Nothing here");
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
}