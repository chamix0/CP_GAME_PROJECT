using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class RootIAManagerFSM : MonoBehaviour
{
    #region variables

    private StateMachineEngine RootIAManagerFSM_FSM;


    private PushPerception characterdiesPerception;
    private PushPerception characterresurrectPerception;
    private PushPerception keepdoingthingsPerception;
    private PushPerception stilldeadPerception;
    private PushPerception gamefinishedPerception;
    private State UtilitySystem;
    private State Dead;
    private State Finishgame;

    #endregion variables

    //Place your variables here
    private PlayerInfo _currentDeadBuddy;

    private bool _ghostWin;
    
    private MainQuestM mainQuest;
    private CharacterManager mainCharacter;
    private String transition;
    private chargingPoint currentChargingPoint;

    // Start is called before the first frame update
    private void Start()
    {
        _ghostWin = false;
        
        RootIAManagerFSM_FSM = new StateMachineEngine(false);

        mainQuest = GetComponent<MainQuestM>();
        mainCharacter = GetComponent<CharacterManager>();
        transition = "";
        CreateStateMachine();
    }


    private void CreateStateMachine()
    {
        // Perceptions
        // Modify or add new Perceptions, see the guide for more
        characterdiesPerception = RootIAManagerFSM_FSM.CreatePerception<PushPerception>();
        characterresurrectPerception = RootIAManagerFSM_FSM.CreatePerception<PushPerception>();
        keepdoingthingsPerception = RootIAManagerFSM_FSM.CreatePerception<PushPerception>();
        stilldeadPerception = RootIAManagerFSM_FSM.CreatePerception<PushPerception>();
        gamefinishedPerception = RootIAManagerFSM_FSM.CreatePerception<PushPerception>();

        // States
        UtilitySystem = RootIAManagerFSM_FSM.CreateEntryState("UtilitySystem", UtilitySystemAction);
        Dead = RootIAManagerFSM_FSM.CreateState("Dead", DeadAction);
        Finishgame = RootIAManagerFSM_FSM.CreateState("Finish game", FinishgameAction);

        // Transitions
        RootIAManagerFSM_FSM.CreateTransition("character dies", UtilitySystem, characterdiesPerception, Dead);
        RootIAManagerFSM_FSM.CreateTransition("character resurrect", Dead, characterresurrectPerception, UtilitySystem);
        RootIAManagerFSM_FSM.CreateTransition("keep doing things", UtilitySystem, keepdoingthingsPerception,
            UtilitySystem);
        RootIAManagerFSM_FSM.CreateTransition("still dead", Dead, stilldeadPerception, Dead);
        RootIAManagerFSM_FSM.CreateTransition("game finished", UtilitySystem, gamefinishedPerception, Finishgame);

        // ExitPerceptions

        // ExitTransitions
    }

    // Update is called once per frame
    private void Update()
    {
        RootIAManagerFSM_FSM.Update();
        
        RootIAManagerFSM_FSM.Fire(transition);
    }

    // Create your desired actions

    private void UtilitySystemAction()
    {
        
        
        //manage defense against the monster
        if (mainCharacter.playerInfo.isDead)
        {
            mainQuest.pause();

            var deaths = 1 + mainCharacter.budsList.Count(bud => bud.playerInfo.isDead);

            if (deaths == mainCharacter.budsList.Count)
            {
                transition = "game finished";
                _ghostWin = true;
            }

            transition = "character dies";
        }
        //manage batery
        else if (mainCharacter.playerInfo.needsToRecharge)
        {
            mainQuest.pause();
            if (!currentChargingPoint)
            {
                currentChargingPoint = mainCharacter.goToRechargePoint();
                currentChargingPoint.use();
            }

            if (mainCharacter.destinationReached())
            {
                mainCharacter.stopAction();
                mainCharacter.rechargeBattery();
                mainCharacter.restoreDestination();
                currentChargingPoint.stopUsing();
                currentChargingPoint = null;
            }

            transition = "keep doing things";
        }
        else if (mainCharacter.isResurrectingBuddy)
        {
            mainQuest.pause();
            if (!_currentDeadBuddy)
                _currentDeadBuddy = mainCharacter.GoToDeadBuddy();

            if (mainCharacter.destinationReached())
            {
                mainCharacter.stopAction();
                _currentDeadBuddy.Resurrect();
                mainCharacter.restoreDestination();
                
                mainCharacter.isResurrectingBuddy = false;
                mainCharacter.deadBuddy = null;
                _currentDeadBuddy = null;
                mainCharacter.PrintLabel("No problem!");
            }
                
            transition = "keep doing things";
        }
        else if (mainCharacter.worldManager.getAllItemsPlaced())
        {
            transition = "game finished";
        }

        //main quest
        else
        {
            mainCharacter.checkOnBud();
            mainQuest.unPause();
            transition = "keep doing things";
        }
    }

    private void DeadAction()
    {
        mainCharacter.PrintLabelBig("I'm Dead");
        if (mainCharacter.playerInfo.isDead)
        {
            mainCharacter.SetAgentSpeed(0.0f);
            //mainQuest.pause();
            transition = "still dead";
        }
        else
        {
            mainCharacter.SetAgentSpeed(mainCharacter.speed);
            //mainQuest.unPause();
            mainCharacter.PrintLabel("Thanks! Back to explore");
            transition = "character resurrect";
        }
    }

    private void FinishgameAction()
    {
        if (_ghostWin)
            SceneManager.LoadScene("Lose Scene");
        SceneManager.LoadScene("Win Scene");
    }

    
}