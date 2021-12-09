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
    [NonSerialized] public PlayerInfo playerInfo;
    public Text characterLabel;
    public MyTimer exploreObjectTimer;
    public GameObject objectFound;
    public float speed = 3;
    [NonSerialized] public bool isResurrectingBuddy;
    [NonSerialized] public PlayerInfo deadBuddy;

    //private
    // private float updatingCooldown = 20;
    private Queue<string> _labelQueue;
    private List<ExplorableObject> explorablePlaces;
    private List<ExplorableObject> exploredPlaces;
    private List<ExplorableObject> containsAnObjectPlaces;
    [NonSerialized] public List<CharacterManager> budsList;
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
        playerInfo = GetComponent<PlayerInfo>();
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        _labelQueue = new Queue<string>();
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


        //object found disable
        objectFound.GetComponent<MeshRenderer>().enabled = false;

        exploreObjectTimer.setTimer(playerInfo.exploringTimeForEachObject);
        setPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        canContinue();
        destinationStillIsUnexplored();
    }

    private void FixedUpdate()
    {
        // if (Time.frameCount % 60 == 0)
        //     characterLabel.text = _labelQueue.Dequeue();

        // if (Time.fixedTime % 1f == 0.0f)
        //     characterLabel.text = _labelQueue.Dequeue();
    }

    #region ACTIONS

    public void goToWaitingPoint()
    {
        PrintLabelBig("Waiting");
        setDestination(waitingPosition, null);
    }

    public chargingPoint goToRechargePoint()
    {
        PrintLabel("I need to recharge");
        destinationBuffer = _currentDestination;
        chargingPoint aux = worldManager.getNearestChargingPoint(this.transform.position);
        setDestination(aux.transform.position);
        return aux;
    }

    public PlayerInfo GoToDeadBuddy()
    {
        PrintLabel("I need to save my bud");
        destinationBuffer = _currentDestination;
        setDestination(deadBuddy.transform.position);
        return deadBuddy;
    }

    public void restoreDestination()
    {
        setDestination(destinationBuffer);
        destinationBuffer = waitingPosition;
    }

    public void rechargeBattery()
    {
        PrintLabel("Recharging");
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
                PrintLabel("Searching recipe book");
                setDestination(worldManager.getBook().getPosition(), worldManager.getBook());
                return true;
            }

            if (explorablePlaces.Contains(worldManager.getShovel()))
            {
                PrintLabel("Searching shovel");
                setDestination(worldManager.getShovel().getPosition(), worldManager.getShovel());
                return true;
            }

            if (explorablePlaces.Count > 0)
            {
                PrintLabel("Where should I look...");
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
        PrintLabelBig("Taking object to front door");
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
            //characterLabel.text = "aaaaaaaaaaa";
            agent.speed = speed;
            return true;
        }

        //characterLabel.text = "doing something...";
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

    public void checkOnBud()
    {
        foreach (CharacterManager c in budsList)
        {
            if (Vector3.Distance(this.transform.position, c.transform.position) <
                playerInfo._budResurrectionRange)
            {
                CheckDeadBud(c);
            }
            
            if (Vector3.Distance(this.transform.position, c.transform.position) <
                playerInfo._budDetectionRange)
            {

                bool aux = false;
                foreach (ExplorableObject explored in c.exploredPlaces)
                {
                    if (!exploredPlaces.Contains(explored))
                    {
                        aux = true;
                        explorablePlaces.Remove(explored);
                        containsAnObjectPlaces.Remove(explored);
                        exploredPlaces.Add(explored);
                    }
                }

                foreach (var knownPlace in c.containsAnObjectPlaces)
                {
                    if (!containsAnObjectPlaces.Contains(knownPlace))
                    {
                        aux = true;
                        containsAnObjectPlaces.Add(knownPlace);
                    }
                }

                if (aux)
                    PrintLabel("Sharing info");
            }
        }
    }

    private void CheckDeadBud(CharacterManager c)
    {
        if (c.playerInfo.isDead)
        {
            PrintLabel("Resurrecting buddy");
            isResurrectingBuddy = true;
            deadBuddy = c.playerInfo;
        }
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

    public void PrintLabel(string label)
    {
        if (_labelQueue.Contains(label)) return;
        _labelQueue.Enqueue(label);
        if (isPrinting) return;
        StartCoroutine(PrintLabelCoroutine());
    }

    private bool isPrinting;

    public void PrintLabelBig(string label)
    {
        if (isPrinting) return;
        if (characterLabel.text.Equals(label)) return;
        characterLabel.text = label;
    }

    private IEnumerator PrintLabelCoroutine()
    {
        isPrinting = true;

        while (_labelQueue.Count != 0)
        {
            characterLabel.text = _labelQueue.Dequeue();
            yield return new WaitForSeconds(1f);
        }

        isPrinting = false;
    }
}