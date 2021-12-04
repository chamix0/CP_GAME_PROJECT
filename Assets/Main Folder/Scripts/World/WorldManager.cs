using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    #region Data

    //Explorable objects
    private List<ExplorableObject> allObjects;
    private List<ExplorableObject> livingRoomObjects;
    private List<ExplorableObject> roomObjects;
    private List<ExplorableObject> graveYardObjects;
    private List<ExplorableObject> kitchenObjects;
    private List<ExplorableObject> libraryObjects;
    private List<ExplorableObject> bathObjects;

    //players
    private List<CharacterController> characters;

    #endregion

    void Awake()
    {
        allObjects = new List<ExplorableObject>();
        livingRoomObjects=new List<ExplorableObject>();
        roomObjects=new List<ExplorableObject>();
        graveYardObjects=new List<ExplorableObject>();
        kitchenObjects=new List<ExplorableObject>();
        libraryObjects=new List<ExplorableObject>();
        bathObjects=new List<ExplorableObject>();
        setPlayers();
        setExplorableObjects();
    }

    private void Start()
    {
        pushObjectsToPlayers();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void shuffleItems()
    {
        System.Random rn = new System.Random();

        livingRoomObjects[rn.Next(livingRoomObjects.Count-1)].setContainsObject(true); 
        roomObjects[rn.Next(roomObjects.Count-1)].setContainsObject(true); 
        graveYardObjects[rn.Next(graveYardObjects.Count-1)].setContainsObject(true); 
        kitchenObjects[rn.Next(kitchenObjects.Count-1)].setContainsObject(true); 
        libraryObjects[rn.Next(libraryObjects.Count-1)].setContainsObject(true); 
        bathObjects[rn.Next(bathObjects.Count-1)].setContainsObject(true); 

        
        
    }
    private void pushObjectsToPlayers()
    {
        foreach (CharacterController c in characters)
        {
            c.addObjects(allObjects.ToArray());
        }
    }

    private void setExplorableObjects()
    {
        //Bath objects
        GameObject[] aux;
        aux = GameObject.FindGameObjectsWithTag("BathExplorable");
        foreach (GameObject g in aux)
        {
            bathObjects.Add(g.GetComponent<ExplorableObject>());
            allObjects.Add(g.GetComponent<ExplorableObject>());
        }

        //Room objects
        aux = GameObject.FindGameObjectsWithTag("RoomExplorable");
        foreach (GameObject g in aux)
        {
            roomObjects.Add(g.GetComponent<ExplorableObject>());
            allObjects.Add(g.GetComponent<ExplorableObject>());
        }

        //Library objects
        aux = GameObject.FindGameObjectsWithTag("LibraryExplorable");
        foreach (GameObject g in aux)
        {
            libraryObjects.Add(g.GetComponent<ExplorableObject>());
            allObjects.Add(g.GetComponent<ExplorableObject>());
        }

        //Livingroom objects
        aux = GameObject.FindGameObjectsWithTag("LivingRoomExplorable");
        foreach (GameObject g in aux)
        {
            livingRoomObjects.Add(g.GetComponent<ExplorableObject>());
            allObjects.Add(g.GetComponent<ExplorableObject>());
        }

        //Kitchen objects
        aux = GameObject.FindGameObjectsWithTag("KitchenExplorable");
        foreach (GameObject g in aux)
        {
            kitchenObjects.Add(g.GetComponent<ExplorableObject>());
            allObjects.Add(g.GetComponent<ExplorableObject>());
        }

        //Graveyard objects
        aux = GameObject.FindGameObjectsWithTag("GraveyardExplorable");
        foreach (GameObject g in aux)
        {
            graveYardObjects.Add(g.GetComponent<ExplorableObject>());
            allObjects.Add(g.GetComponent<ExplorableObject>());
        }
    }

    private void setPlayers()
    {
        GameObject[] aux;
        aux = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject g in aux)
        {
            if (g != this)
            {
                characters.Add(g.GetComponent<CharacterController>());
            }
        }
    }
}