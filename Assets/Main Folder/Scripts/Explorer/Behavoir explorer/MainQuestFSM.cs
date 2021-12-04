using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainQuestFSM : MonoBehaviour
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
    private ValuePerception QuestfinishedPerception;
    private State lookingforanobject;
    private State objectfound;
    private State takeobjecttotheplace;
    private State leaveobject;
    private State checkonbud;
    private State reanimate;
    private State updateinfo;
    private State waitforthedoortoopen;

    //Place your variables here
    private CharacterController mainCharacter;

    #endregion variables

    // Start is called before the first frame update
    private void Start()
    {
        MainQuestFSM_FSM = new StateMachineEngine(false);
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
        //bactoexplorePerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();
        //bactoexplorePerception = MainQuestFSM_FSM.CreatePerception<PushPerception>();
        QuestfinishedPerception =
            MainQuestFSM_FSM.CreatePerception<ValuePerception>(() => false /*Replace this with a boolean function*/);

        // States
        lookingforanobject = MainQuestFSM_FSM.CreateEntryState("looking for an object", lookingforanobjectAction);
        objectfound = MainQuestFSM_FSM.CreateState("object found", objectfoundAction);
        takeobjecttotheplace = MainQuestFSM_FSM.CreateState("take object to the place", takeobjecttotheplaceAction);
        leaveobject = MainQuestFSM_FSM.CreateState("leave object", leaveobjectAction);
        checkonbud = MainQuestFSM_FSM.CreateState("check on bud", checkonbudAction);
        reanimate = MainQuestFSM_FSM.CreateState("reanimate", reanimateAction);
        updateinfo = MainQuestFSM_FSM.CreateState("update info", updateinfoAction);
        waitforthedoortoopen = MainQuestFSM_FSM.CreateState("wait for the door to open", waitforthedoortoopenAction);

        // Transitions
        MainQuestFSM_FSM.CreateTransition("There is object", lookingforanobject, ThereisobjectPerception, objectfound);
        MainQuestFSM_FSM.CreateTransition("object needed", objectfound, objectneededPerception, takeobjecttotheplace);
        MainQuestFSM_FSM.CreateTransition("object not needed", objectfound, objectnotneededPerception, leaveobject);
        MainQuestFSM_FSM.CreateTransition("go back to explore", leaveobject, gobacktoexplorePerception,
            lookingforanobject);
        MainQuestFSM_FSM.CreateTransition("back to explore", takeobjecttotheplace, gobacktoexplorePerception,
            lookingforanobject);
        MainQuestFSM_FSM.CreateTransition("bud on range", lookingforanobject, budonrangePerception, checkonbud);
        MainQuestFSM_FSM.CreateTransition("bud is dead", checkonbud, budisdeadPerception, reanimate);
        MainQuestFSM_FSM.CreateTransition("bud is ok", checkonbud, budisokPerception, updateinfo);
        MainQuestFSM_FSM.CreateTransition("bud reanimated", reanimate, budreanimatedPerception, updateinfo);
        MainQuestFSM_FSM.CreateTransition("bac to explore", updateinfo, gobacktoexplorePerception, lookingforanobject);
        MainQuestFSM_FSM.CreateTransition("keep Exploring", lookingforanobject, gobacktoexplorePerception,
            waitforthedoortoopen);
        MainQuestFSM_FSM.CreateTransition("Quest finished", lookingforanobject, QuestfinishedPerception,
            waitforthedoortoopen);
        // ExitPerceptions
        // ExitTransitions
    }

    // Update is called once per frame
    private void Update()
    {
        MainQuestFSM_FSM.Update();
    }

    // Create your desired actions

    private void lookingforanobjectAction()
    {
        if (mainCharacter.findRandomExplorablePlace())
        {
            if (mainCharacter.destinationReached())
            {
                mainCharacter.stopToExploreAnObject();
                if (mainCharacter.currentTarget.getContainsObject())
                {
                    MainQuestFSM_FSM.Fire("There is object");
                }
                else
                {
                    mainCharacter.changeToExplored();
                    MainQuestFSM_FSM.Fire("keep Exploring");
                }
            }
            else
            {
                MainQuestFSM_FSM.Fire("keep Exploring");
            }
        }
        else
        {
            MainQuestFSM_FSM.Fire("Quest finished");
        }
    }

    private void objectfoundAction()
    {
    }

    private void takeobjecttotheplaceAction()
    {
    }

    private void leaveobjectAction()
    {
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
}