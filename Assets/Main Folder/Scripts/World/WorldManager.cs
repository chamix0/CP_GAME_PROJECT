using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class WorldManager : MonoBehaviour
{
    public enum ObjectTypes
    {
        BOOK = 6,
        KITCHEN = 0,
        GRAVEYARD = 1,
        LIVINGROOM = 2,
        ROOM = 3,
        BATH = 4,
        LIBRARY = 5
    };

    #region Data

    //Explorable objects
    private List<GameObject> booksPlaceHolders;
    private List<ExplorableObject> allObjects;
    private List<ExplorableObject> livingRoomObjects;
    private List<ExplorableObject> roomObjects;
    private List<ExplorableObject> graveYardObjects;
    private List<ExplorableObject> kitchenObjects;
    private List<ExplorableObject> libraryObjects;
    private List<ExplorableObject> bathObjects;

    private ExplorableObject book;
    private ExplorableObject graveYardItem;

    private int phase;

    //patform where the bomb goes
    public GameObject platform;

    //players
    private List<CharacterController> characters;
    private List<ExplorableObject> neededObjects;

    #endregion

    void Awake()
    {
        phase = 0;
        allObjects = new List<ExplorableObject>();
        livingRoomObjects = new List<ExplorableObject>();
        roomObjects = new List<ExplorableObject>();
        graveYardObjects = new List<ExplorableObject>();
        kitchenObjects = new List<ExplorableObject>();
        libraryObjects = new List<ExplorableObject>();
        bathObjects = new List<ExplorableObject>();
        booksPlaceHolders = new List<GameObject>();
        neededObjects = new List<ExplorableObject>();
        characters = new List<CharacterController>();
        setExplorableObjects();
        chooseBookLocation();
        shuffleItems();
        setNeededObjects();
        setPlayers();
        pushInfoToPlayers();
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void advanceOnTask()
    {
        phase++;
    }

    public ExplorableObject getCurrentTask()
    {
        if (phase == 0)
        {
            return book;
        }
        else if (phase == 4)
        {
            return graveYardItem;
        }
        else
        {
            return neededObjects[phase];
        }
    }


    public void setNeededObjects()
    {
        System.Random rn = new System.Random();

        //shuffle
        neededObjects.OrderBy(a => rn.Next());

        for (int i = 0; i < 2; i++)
        {
            neededObjects.RemoveAt(rn.Next(neededObjects.Count - 1));
        }
    }

    private void shuffleItems()
    {
        System.Random rn = new System.Random();
        int i = rn.Next(livingRoomObjects.Count - 1);
        livingRoomObjects[i].setContainsObject(true);
        livingRoomObjects[i].setType(ObjectTypes.LIVINGROOM);
        neededObjects.Add(livingRoomObjects[i]);

        i = rn.Next(roomObjects.Count - 1);
        roomObjects[i].setContainsObject(true);
        roomObjects[i].setType(ObjectTypes.ROOM);
        neededObjects.Add(roomObjects[i]);


        i = rn.Next(kitchenObjects.Count - 1);
        kitchenObjects[i].setContainsObject(true);
        kitchenObjects[i].setType(ObjectTypes.KITCHEN);
        neededObjects.Add(kitchenObjects[i]);


        i = rn.Next(libraryObjects.Count - 1);
        libraryObjects[i].setContainsObject(true);
        libraryObjects[i].setType(ObjectTypes.LIBRARY);
        neededObjects.Add(libraryObjects[i]);


        i = rn.Next(bathObjects.Count - 1);
        bathObjects[i].setContainsObject(true);
        bathObjects[i].setType(ObjectTypes.BATH);
        neededObjects.Add(bathObjects[i]);

        i = rn.Next(graveYardObjects.Count - 1);
        graveYardObjects[i].setContainsObject(true);
        graveYardObjects[i].setType(ObjectTypes.GRAVEYARD);
        graveYardItem = graveYardObjects[i];
    }

    private void pushInfoToPlayers()
    {
        foreach (CharacterController c in characters)
        {
            c.addObjects(allObjects.ToArray());
            c.platformLocation = getPlatformPosition();
        }
    }

    private void chooseBookLocation()
    {
        Random rn = new Random();
        int aux = rn.Next(booksPlaceHolders.Count - 1);
        for (int i = 0; i < booksPlaceHolders.Count; i++)
        {
            if (i == aux)
            {
                book = booksPlaceHolders[i].GetComponent<ExplorableObject>();
                book.setType(ObjectTypes.BOOK);
                book.setContainsObject(true);
                allObjects.Add(book);
                break;
            }
            Destroy(booksPlaceHolders[i]);
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

        //Graveyard objects
        aux = GameObject.FindGameObjectsWithTag("book");
        foreach (GameObject g in aux)
        {
            booksPlaceHolders.Add(g);
        }
    }

    private void setPlayers()
    {
        GameObject[] aux;
        aux = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject g in aux)
        {
            characters.Add(g.GetComponent<CharacterController>());
        }
    }

    public Vector3 getPlatformPosition()
    {
        return platform.transform.position;
    }
}