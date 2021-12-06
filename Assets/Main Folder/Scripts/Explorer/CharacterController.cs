using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// this class function is to look for random places to explore and update info with other explorers
/// </summary>
[DefaultExecutionOrder(-1)]
public class CharacterController : MonoBehaviour
{
    #region VARIABLES

    //public
    public PlayerInfo PlayerInfo;
    public Text characterLabel;
    public MyTimer exploreObjectTimer, updateWithBudTimer;
    public GameObject objectFound;
    public float speed = 3;

    //private
    // private float updatingCooldown = 20;
    private List<ExplorableObject> explorablePlaces;
    private List<ExplorableObject> exploredPlaces;
    private List<ExplorableObject> containsAnObjectPlaces;

    private List<CharacterController> budsList;
    private Vector3 _currentDestination;
    private Vector3 _startingPosition;
    [NonSerialized] public ExplorableObject currentTarget;
    private NavMeshAgent agent;
    [SerializeField] public WorldManager worldManager;

    #endregion

    private void Awake()
    {
        UnityEngine.Random.InitState((int) System.DateTime.Now.Ticks);
        budsList = new List<CharacterController>();
        explorablePlaces = new List<ExplorableObject>();
        exploredPlaces = new List<ExplorableObject>();
        containsAnObjectPlaces = new List<ExplorableObject>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        _currentDestination = transform.position;
        _startingPosition = _currentDestination;
        currentTarget = null;
        characterLabel.text = PlayerInfo._name;


        //object found disable
        objectFound.GetComponent<MeshRenderer>().enabled = false;

        exploreObjectTimer.setTimer(PlayerInfo.exploringTimeForEachObject);
        updateWithBudTimer.setTimer(PlayerInfo.updatingTimeForEachBud);
        //updateAnimationCooldownTimer.setTimer(updatingCooldown);
        setPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        canContinue();
    }

    #region ACTIONS

    public bool isMyObjectNeeded()
    {
        return Equals(currentTarget, worldManager.getCurrentTask());
    }

    public bool findRandomExplorablePlace()
    {
        System.Random rn = new System.Random();
        if (!currentTarget)
        {
            if (explorablePlaces.Contains(worldManager.getBook()))
            {
                setDestination(worldManager.getBook().getPosition(), worldManager.getBook());
                return true;
            }

            if (explorablePlaces.Count > 0)
            {
                ExplorableObject aux = explorablePlaces[rn.Next(explorablePlaces.Count - 1)];
                setDestination(aux.getPosition(), aux);
                return true;
            }

            currentTarget = null;
            setDestination(_startingPosition, null);
            return false;
        }

        return true;
    }


    public void stopToExploreAnObject()
    {
        if (exploreObjectTimer.hasBeenUsed())
        {
            exploreObjectTimer.resetTimer();
            exploreObjectTimer.start();
        }
        else
        {
            exploreObjectTimer.start();
        }
    }

    public void takeObjectToBombPlatform()
    {
        setDestination(worldManager.getPlatformPosition());
        objectFound.GetComponent<MeshRenderer>().enabled = true;
    }

    #endregion

    #region Methods

    public void goToObjectIknow()
    {
        Vector3 location = worldManager.getCurrentTask().transform.position;
        setDestination(location, worldManager.getCurrentTask());
    }

    public bool iKnowWhereThatObjectIs()
    {
        return containsAnObjectPlaces.Contains(worldManager.getCurrentTask());
    }

    private bool canContinue()
    {
        if (exploreObjectTimer.pausedTimer() /*|| updateWithBudTimer.pausedTimer()*/)
        {
            characterLabel.text = "Going to my target...";
            agent.speed = speed;
            return true;
        }
        else
        {
            characterLabel.text = "Exploring...";
            agent.speed = 0;
            return false;
        }
    }

    public void setDestination(Vector3 pos, ExplorableObject exp)
    {
        currentTarget = exp;
        _currentDestination = pos;
        agent.SetDestination(_currentDestination);
    }

    public void setDestination(Vector3 pos)
    {
        _currentDestination = pos;
        agent.SetDestination(_currentDestination);
    }

    public void addObjectTocontainsAnObjectList()
    {
        containsAnObjectPlaces.Add(currentTarget);
        explorablePlaces.Remove(currentTarget);
        currentTarget = null;
    }

    public void changeToExplored()
    {
        if (currentTarget)
        {
            currentTarget.setExplored();
            explorablePlaces.Remove(currentTarget);
            exploredPlaces.Add(currentTarget);
            currentTarget = null;
        }
    }

    public float distanceToCurrentDestination()
    {
        //Debug.Log(Vector3.Distance(transform.position, _currentDestination));
        return Vector3.Distance(transform.position, _currentDestination);
    }

    public void budOnRange()
    {
        foreach (CharacterController c in budsList)
        {
            if (this != c && Vector3.Distance(this.transform.position, c.transform.position) <
                PlayerInfo._budDetectionRange)
            {
                //stopToShareInfo();
                foreach (var explored in c.exploredPlaces)
                {
                    if (!exploredPlaces.Contains(explored))
                    {
                        explorablePlaces.Remove(explored);
                        exploredPlaces.Add(explored);
                    }
                }
            }
        }
    }

    public void addObjects(ExplorableObject[] obj)
    {
        explorablePlaces.AddRange(obj);
    }

    /// <summary>
    /// if this the current target has been updated to explored then it forgets it exists
    /// </summary>
    public void destinationStillIsUnexplored()
    {
        if (exploredPlaces.Contains(currentTarget))
        {
            agent.speed = speed;
            setDestination(transform.position, null);
        }
    }

    public bool destinationReached()
    {
        return distanceToCurrentDestination() < 1;
    }

    public bool actionManagerTextTime()
    {
        if (destinationReached())
        {
            stopToExploreAnObject();
            canContinue();
            return true;
        }

        return false;
    }

    public void setPlayers()
    {
        GameObject[] aux;
        aux = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject g in aux)
        {
            if (g != this)
            {
                budsList.Add(g.GetComponent<CharacterController>());
            }
        }
    }

    #endregion
}