using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefendfrommonsterBT : MonoBehaviour {

    #region variables

    private BehaviourTreeEngine DefendfrommonsterBT_BT;
    

    private SelectorNode Defend;
    private SequenceNode attack;
    private LeafNode flashattack;
    private LeafNode faint;
    private ConditionalDecoratorNode Conditional_flashattack;
    
    //Place your variables here

    #endregion variables

    // Start is called before the first frame update
    private void Start()
    {
        DefendfrommonsterBT_BT = new BehaviourTreeEngine(false);
        

        CreateBehaviourTree();
    }
    
    
    private void CreateBehaviourTree()
    {
        // Nodes
        Defend = DefendfrommonsterBT_BT.CreateSelectorNode("Defend");
        attack = DefendfrommonsterBT_BT.CreateSequenceNode("attack", true);
        flashattack = DefendfrommonsterBT_BT.CreateLeafNode("flash attack", flashattackAction, flashattackSuccessCheck);
        faint = DefendfrommonsterBT_BT.CreateLeafNode("faint", faintAction, faintSuccessCheck);
        Conditional_flashattack = DefendfrommonsterBT_BT.CreateConditionalNode("Conditional_flashattack", flashattack, null /*Change this for a perception*/);
        
        // Child adding
        Defend.AddChild(attack);
        Defend.AddChild(faint);
        
        attack.AddChild(Conditional_flashattack);
        
        // SetRoot
        DefendfrommonsterBT_BT.SetRootNode(Defend);
        
        // ExitPerceptions
        
        // ExitTransitions
        
    }

    // Update is called once per frame
    private void Update()
    {
        DefendfrommonsterBT_BT.Update();
    }

    // Create your desired actions
    
    private void flashattackAction()
    {
        
    }
    
    private ReturnValues flashattackSuccessCheck()
    {
        //Write here the code for the success check for flashattack
        return ReturnValues.Failed;
    }
    
    private void faintAction()
    {
        
    }
    
    private ReturnValues faintSuccessCheck()
    {
        //Write here the code for the success check for faint
        return ReturnValues.Failed;
    }
    
}