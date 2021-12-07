using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
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
    private bool allItemsFound = false;
    private bool exploted = false;

    private ExplorableObject book;
    private ExplorableObject graveYardItem;

    private int phase;

    //patform where the bomb goes
    public GameObject platform;

    public Text tasks;

    //players
    private List<CharacterManager> characters;
    private List<ExplorableObject> neededObjects;

    #endregion

    void Awake()
    {
        UnityEngine.Random.InitState((int) System.DateTime.Now.Ticks);
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
        characters = new List<CharacterManager>();
        setExplorableObjects();
        chooseBookLocation();
        shuffleItems();
        setNeededObjects();
        setPlayers();
        pushInfoToPlayers();
    }

    private void Start()
    {
        printTasks();
    }

    public void advanceOnTask()
    {
        phase++;
        if (phase > 6)
        {
            allItemsFound = true;
        }

        printTasks();
    }

    public bool getAllItemsFound()
    {
        return allItemsFound;
    }

    public ExplorableObject getBook()
    {
        return book;
    }

    public void printTasks()
    {
        string tas;
        tas = "" + getCurrentTask().getType() + "\n";
        tas += "-------\n";
        for (int i = 0; i < neededObjects.Count; i++)
        {
            tas += "\n" + neededObjects[i].getType();
        }

        tasks.text = "\n" + tas;
    }

    public ExplorableObject getCurrentTask()
    {
        return neededObjects[phase % neededObjects.Count];
    }

    public bool getExploted()
    {
        return exploted;
    }

    public void setNeededObjects()
    {
        //shuffle
        neededObjects.Sort((a, b) => 1 - 2 * UnityEngine.Random.Range(0, 5));


        List<ExplorableObject> aux = new List<ExplorableObject>();
        aux.Add(book);
        aux.AddRange(neededObjects);
        aux.Add(graveYardItem);
        neededObjects = aux;
    }

    private void shuffleItems()
    {
        int i = UnityEngine.Random.Range(0, livingRoomObjects.Count - 1);
        livingRoomObjects[i].setContainsObject(true);
        livingRoomObjects[i].setType(ObjectTypes.LIVINGROOM);
        neededObjects.Add(livingRoomObjects[i]);

        i = UnityEngine.Random.Range(0, roomObjects.Count - 1);
        roomObjects[i].setContainsObject(true);
        roomObjects[i].setType(ObjectTypes.ROOM);
        neededObjects.Add(roomObjects[i]);


        i = UnityEngine.Random.Range(0, kitchenObjects.Count - 1);
        kitchenObjects[i].setContainsObject(true);
        kitchenObjects[i].setType(ObjectTypes.KITCHEN);
        neededObjects.Add(kitchenObjects[i]);


        i = UnityEngine.Random.Range(0, libraryObjects.Count - 1);
        libraryObjects[i].setContainsObject(true);
        libraryObjects[i].setType(ObjectTypes.LIBRARY);
        neededObjects.Add(libraryObjects[i]);


        i = UnityEngine.Random.Range(0, bathObjects.Count - 1);
        bathObjects[i].setContainsObject(true);
        bathObjects[i].setType(ObjectTypes.BATH);
        neededObjects.Add(bathObjects[i]);

        i = UnityEngine.Random.Range(0, graveYardObjects.Count - 1);
        graveYardObjects[i].setContainsObject(true);
        graveYardObjects[i].setType(ObjectTypes.GRAVEYARD);
        graveYardItem = graveYardObjects[i];
    }

    private void pushInfoToPlayers()
    {
        foreach (CharacterManager c in characters)
        {
            c.addObjects(allObjects.ToArray());
        }
    }

    private void chooseBookLocation()
    {
        int aux = UnityEngine.Random.Range(0, booksPlaceHolders.Count - 1);
        for (int i = 0; i < booksPlaceHolders.Count; i++)
        {
            if (i == aux)
            {
                book = booksPlaceHolders[i].GetComponent<ExplorableObject>();
                book.setType(ObjectTypes.BOOK);
                book.setContainsObject(true);
                allObjects.Add(book);
            }
            else
            {
                Destroy(booksPlaceHolders[i]);
            }
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
            characters.Add(g.GetComponent<CharacterManager>());
        }
    }

    public Vector3 getPlatformPosition()
    {
        return platform.transform.position;
    }
}