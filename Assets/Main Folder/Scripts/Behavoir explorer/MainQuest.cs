using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainQuest : MonoBehaviour {

    #region variables

    private StateMachineEngine MainQuest_FSM;
    

    private PushPerception mainquestfinishedPerception;
    private PushPerception quedarinconscientePerception;
    private PushPerception NewTransition2Perception;
    private ValuePerception nobateryPerception;
    private PushPerception helpedPerception;
    private ValuePerception NewTransition5Perception;
    private State mainQuest;
    private State Inconcious;
    private State Finishgame;
    private State Askforhelp;
    
    //Place your variables here

    #endregion variables

    // Start is called before the first frame update
    private void Start()
    {
        MainQuest_FSM = new StateMachineEngine(false);
        

        CreateStateMachine();
    }
    
    
    private void CreateStateMachine()
    {
        // Perceptions
        // Modify or add new Perceptions, see the guide for more
        mainquestfinishedPerception = MainQuest_FSM.CreatePerception<PushPerception>();
        quedarinconscientePerception = MainQuest_FSM.CreatePerception<PushPerception>();
        NewTransition2Perception = MainQuest_FSM.CreatePerception<PushPerception>();
        nobateryPerception = MainQuest_FSM.CreatePerception<ValuePerception>(() => false /*Replace this with a boolean function*/);
        helpedPerception = MainQuest_FSM.CreatePerception<PushPerception>();
        NewTransition5Perception = MainQuest_FSM.CreatePerception<ValuePerception>(() => false /*Replace this with a boolean function*/);
        
        // States
        mainQuest = MainQuest_FSM.CreateEntryState("Main Quest", MainQuestAction);
        Inconcious = MainQuest_FSM.CreateState("Inconcious", InconciousAction);
        Finishgame = MainQuest_FSM.CreateState("Finish game", FinishgameAction);
        Askforhelp = MainQuest_FSM.CreateState("Ask for help", AskforhelpAction);
        
        // Transitions
        MainQuest_FSM.CreateTransition("main quest finished", mainQuest, mainquestfinishedPerception, Finishgame);
        MainQuest_FSM.CreateTransition("quedar inconsciente", mainQuest, quedarinconscientePerception, Inconcious);
        MainQuest_FSM.CreateTransition("New Transition 2", Inconcious, NewTransition2Perception, mainQuest);
        MainQuest_FSM.CreateTransition("no batery", mainQuest, nobateryPerception, Askforhelp);
        MainQuest_FSM.CreateTransition("helped", Askforhelp, helpedPerception, mainQuest);
        MainQuest_FSM.CreateTransition("New Transition 5", Inconcious, NewTransition5Perception, Askforhelp);
        
        // ExitPerceptions
        
        // ExitTransitions
        
    }

    // Update is called once per frame
    private void Update()
    {
        MainQuest_FSM.Update();
    }

    // Create your desired actions
    
    private void MainQuestAction()
    {
        
    }
    
    private void InconciousAction()
    {
        
    }
    
    private void FinishgameAction()
    {
        
    }
    
    private void AskforhelpAction()
    {
        
    }
    
}