using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// this class function is to look for random places to explore and update info with other explorers
/// </summary>
public class CharacterController : MonoBehaviour
{
    #region VARIABLES

    //public
    public PlayerInfo PlayerInfo;
    public Text characterLabel;

    public MyTimer exploreObjectTimer, updateWithBudTimer /*, 
        updateAnimationCooldownTimer*/;

    public float speed = 3;
    public bool enabledMainQuest = true;

    //private
    // private float updatingCooldown = 20;
    private List<ExplorableObject> explorablePlaces;
    private List<ExplorableObject> exploredPlaces;
    private List<CharacterController> budsList;
    private Vector3 _currentDestination;
    private Vector3 _startingPosition;
    [NonSerialized] public ExplorableObject currentTarget;
    private NavMeshAgent agent;

    #endregion

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _currentDestination = transform.position;
        _startingPosition = _currentDestination;
        currentTarget = null;
        characterLabel.text = PlayerInfo._name;

        budsList = new List<CharacterController>();
        exploredPlaces = new List<ExplorableObject>();


        exploreObjectTimer.setTimer(PlayerInfo.exploringTimeForEachObject);
        updateWithBudTimer.setTimer(PlayerInfo.updatingTimeForEachBud);
        //updateAnimationCooldownTimer.setTimer(updatingCooldown);

        setPlayers();
        //setExplorableObjects();
        findRandomExplorablePlace();
    }

    // Update is called once per frame
    void Update()
    {
        canContinue();
    }

    #region ACTIONS

    private void mainQuest()
    {
        //Debug.Log(distanceToCurrentDestination() + " " + explorablePlaces.Count);
        if (canContinue())
        {
            budOnRange();
            destinationStillIsUnexplored();
            if (placeExplored())
            {
                changeToExplored();
                currentTarget = null;
                findRandomExplorablePlace();
            }
        }
    }

    public bool findRandomExplorablePlace()
    {
        System.Random rn = new System.Random();
        if (!currentTarget)
        {
            if (explorablePlaces.Count > 0)
            {
                ExplorableObject aux = explorablePlaces[rn.Next(explorablePlaces.Count - 1)];
                setDestination(aux.getPosition());
                currentTarget = aux;
                return true;
            }
            else
            {
                currentTarget = null;
                setDestination(_startingPosition);
                return false;
            }
        }

        return true;
    }


    public void stopToExploreAnObject()
    {
        if (currentTarget)
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
    }

    #endregion

    #region Methods

    private bool canContinue()
    {
        if (exploreObjectTimer.pausedTimer() /*|| updateWithBudTimer.pausedTimer()*/)
        {
            agent.speed = speed;
            return true;
        }
        else
        {
            agent.speed = 0;
            return false;
        }
    }

    public void setDestination(Vector3 pos)
    {
        _currentDestination = pos;
        agent.SetDestination(_currentDestination);
    }

    public void changeToExplored()
    {
        if (currentTarget)
        {
            currentTarget.setExplored();
            if (explorablePlaces.Remove(currentTarget))
            {
                exploredPlaces.Add(currentTarget);
            }
        }
    }

    public float distanceToCurrentDestination()
    {
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
                //Debug.Log(Vector3.Distance(this.transform.position, c.transform.position));
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
            setDestination(transform.position);
            currentTarget = null;
        }
    }

    public bool destinationReached()
    {
        bool aux = distanceToCurrentDestination() < 1;
        return aux;
    }

    private bool placeExplored()
    {
        if (destinationReached())
        {
            characterLabel.text = "Exploring...";
            canContinue();
            stopToExploreAnObject();
            return true;
        }
        else
        {
            characterLabel.text = "Going to my target...";
            return false;
        }
    }

    /*public void setExplorableObjects()
    {
        GameObject[] aux;
        aux = GameObject.FindGameObjectsWithTag("Explorable");
        foreach (GameObject g in aux)
        {
            explorablePlaces.Add(g.GetComponent<ExplorableObject>());
        }
    }*/

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