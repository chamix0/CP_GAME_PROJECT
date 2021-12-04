using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostFSM : MonoBehaviour {

    #region variables

    private StateMachineEngine GhostFSM_FSM;
    

    private CustomNamePerception HumaninrangePerception;
    private PushPerception HumandoesntdefenditselfPerception;
    private PushPerception HumandefendsitselfPerception;
    private PushPerception HumandiedPerception;
    private PushPerception GhosthasescapedPerception;
    private State Wanderaround;
    private State Chasehuman;
    private State Attack;
    private State Runaway;
    
    //Place your variables here

    #endregion variables

    // Start is called before the first frame update
    private void Start()
    {
        GhostFSM_FSM = new StateMachineEngine(false);
        

        CreateStateMachine();
    }
    
    
    private void CreateStateMachine()
    {
        // Perceptions
        // Modify or add new Perceptions, see the guide for more
        HumaninrangePerception = GhostFSM_FSM.CreatePerception<CustomNamePerception>(new CustomNamePerception());
        HumandoesntdefenditselfPerception = GhostFSM_FSM.CreatePerception<PushPerception>();
        HumandefendsitselfPerception = GhostFSM_FSM.CreatePerception<PushPerception>();
        HumandiedPerception = GhostFSM_FSM.CreatePerception<PushPerception>();
        GhosthasescapedPerception = GhostFSM_FSM.CreatePerception<PushPerception>();
        
        // States
        Wanderaround = GhostFSM_FSM.CreateEntryState("Wander around", WanderAroundAction);
        Chasehuman = GhostFSM_FSM.CreateState("Chase human", ChaseHumanAction);
        Attack = GhostFSM_FSM.CreateState("Attack", AttackAction);
        Runaway = GhostFSM_FSM.CreateState("Run away", RunAwayAction);
        
        // Transitions
        GhostFSM_FSM.CreateTransition("Human in range", Wanderaround, HumaninrangePerception, Chasehuman);
        GhostFSM_FSM.CreateTransition("Human doesn't defend itself", Chasehuman, HumandoesntdefenditselfPerception, Attack);
        GhostFSM_FSM.CreateTransition("Human defends itself", Chasehuman, HumandefendsitselfPerception, Runaway);
        GhostFSM_FSM.CreateTransition("Human died", Attack, HumandiedPerception, Wanderaround);
        GhostFSM_FSM.CreateTransition("Ghost has escaped", Runaway, GhosthasescapedPerception, Wanderaround);
        
        // ExitPerceptions
        
        // ExitTransitions
        
    }

    // Update is called once per frame
    private void Update()
    {
        GhostFSM_FSM.Update();
    }

    // Create your desired actions
    
    private void WanderAroundAction()
    {
        
    }
    
    private void ChaseHumanAction()
    {
        
    }
    
    private void AttackAction()
    {
        
    }
    
    private void RunAwayAction()
    {
        
    }
    
}