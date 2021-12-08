using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;

/// <summary>
/// this class function is to look for random places to explore and update info with other explorers
/// </summary>
[DefaultExecutionOrder(-1)]
public class CharacterManager : MonoBehaviour
{
    #region VARIABLES

    //public
    public PlayerInfo PlayerInfo;
    public Text characterLabel;
    public MyTimer exploreObjectTimer;
    public GameObject objectFound;
    public float speed = 3;

    //private
    // private float updatingCooldown = 20;
    private List<ExplorableObject> explorablePlaces;
    private List<ExplorableObject> exploredPlaces;
    private List<ExplorableObject> containsAnObjectPlaces;
    private List<CharacterManager> budsList;
    private Vector3 _currentDestination;
    private Vector3 waitingPosition;
    [NonSerialized] public ExplorableObject currentTarget;
    private NavMeshAgent agent;
    private Vector3 destinationBuffer;
    [SerializeField] private GameObject waitingPositionObject;
    [SerializeField] public WorldManager worldManager;
    [SerializeField] public LightManager lightManager;
    

    #endregion

    private void Awake()
    {
        UnityEngine.Random.InitState((int) System.DateTime.Now.Ticks);
        budsList = new List<CharacterManager>();
        explorablePlaces = new List<ExplorableObject>();
        exploredPlaces = new List<ExplorableObject>();
        containsAnObjectPlaces = new List<ExplorableObject>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        _currentDestination = transform.position;
        waitingPosition = waitingPositionObject.transform.position;
        currentTarget = null;
        characterLabel.text = PlayerInfo._name;


        //object found disable
        objectFound.GetComponent<MeshRenderer>().enabled = false;

        exploreObjectTimer.setTimer(PlayerInfo.exploringTimeForEachObject);
        setPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        canContinue();
        destinationStillIsUnexplored();
    }

    #region ACTIONS

    public void goToWaitingPoint()
    {
        setDestination(waitingPosition, null);
    }

    public chargingPoint goToRechargePoint()
    {
        destinationBuffer = _currentDestination;
        chargingPoint aux = worldManager.getNearestChargingPoint(this.transform.position);
        setDestination(aux.transform.position);
        return aux;
    }

    public void restoreDestination()
    {
        setDestination(destinationBuffer);
        destinationBuffer = waitingPosition;
    }

    public void rechargeBattery()
    {
        lightManager.chargeBattery();
    }

    public bool isMyObjectNeeded()
    {
        return Equals(currentTarget, worldManager.getCurrentTask());
    }

    public bool findRandomExplorablePlace()
    {
        if (!currentTarget)
        {
            if (explorablePlaces.Contains(worldManager.getBook()))
            {
                setDestination(worldManager.getBook().getPosition(), worldManager.getBook());
                return true;
            }

            if (explorablePlaces.Contains(worldManager.getShovel()))
            {
                setDestination(worldManager.getShovel().getPosition(), worldManager.getShovel());
                return true;
            }

            if (explorablePlaces.Count > 0)
            {
                ExplorableObject aux = explorablePlaces[UnityEngine.Random.Range(0, explorablePlaces.Count - 1)];
                setDestination(aux.getPosition(), aux);
                return true;
            }

            currentTarget = null;
            setDestination(waitingPosition, null);
            return false;
        }

        return true;
    }

    public void forgetAboutGraveyard()
    {
        explorablePlaces.Remove(worldManager.getGraveyardItem());
        exploredPlaces.Add(worldManager.getGraveyardItem());
        containsAnObjectPlaces.Remove(worldManager.getGraveyardItem());
    }

    public void stopAction()
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
        if (exploreObjectTimer.pausedTimer())
        {
            characterLabel.text = "Going to my target...";
            agent.speed = speed;
            return true;
        }

        characterLabel.text = "doing something...";
        agent.speed = 0;
        return false;
    }

    public void setDestination(Vector3 pos, ExplorableObject exp)
    {
        currentTarget = exp;
        _currentDestination = pos;
        agent.speed = speed;
        agent.SetDestination(_currentDestination);
    }

    public void setDestination(Vector3 pos)
    {
        _currentDestination = pos;
        agent.speed = speed;
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

    public bool checkOnBud()
    {
        foreach (CharacterManager c in budsList)
        {
            if (Vector3.Distance(this.transform.position, c.transform.position) <
                PlayerInfo._budDetectionRange)
            {
                foreach (ExplorableObject explored in c.exploredPlaces)
                {
                    if (!exploredPlaces.Contains(explored))
                    {
                        explorablePlaces.Remove(explored);
                        containsAnObjectPlaces.Remove(explored);
                        exploredPlaces.Add(explored);
                    }
                }

                foreach (var knownPlace in c.containsAnObjectPlaces)
                {
                    if (!containsAnObjectPlaces.Contains(knownPlace))
                    {
                        containsAnObjectPlaces.Add(knownPlace);
                    }
                }

                if (c.PlayerInfo.isDead)
                {
                    StartCoroutine(ResurrectBud(c.PlayerInfo));
                    c.PlayerInfo.Resurrect();
                    return true;
                }
            }
        }

        return false;
    }


    public void addObjects(ExplorableObject[] obj)
    {
        explorablePlaces.AddRange(obj);
    }

    public void addObject(ExplorableObject obj)
    {
        explorablePlaces.Add(obj);
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

        foreach (var c in exploredPlaces)
        {
            if (containsAnObjectPlaces.Contains(c))
            {
                if (containsAnObjectPlaces.Remove(c))
                {
                    agent.speed = speed;
                    setDestination(transform.position, null);
                }
            }
        }
    }

    public bool destinationReached()
    {
        if (distanceToCurrentDestination() < 0.6)
        {
            if (currentTarget == worldManager.getBook() || currentTarget == worldManager.getShovel())
            {
                currentTarget.turnOffMesh();
            }

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
            if (g.GetComponent<CharacterManager>() != this)
            {
                budsList.Add(g.GetComponent<CharacterManager>());
            }
        }
    }

    #endregion

    public void SetAgentSpeed(float speed)
    {
        agent.speed = speed;
    }
    
    private IEnumerator ResurrectBud(PlayerInfo playerInfo)
    {
        agent.speed = 0.0f;
        yield return new WaitForSeconds(playerInfo._resurrectDuration);
        playerInfo.Resurrect();
        agent.speed = speed;
    }
}