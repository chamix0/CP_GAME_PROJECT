using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer : MonoBehaviour
{
    private float timer;
    private float timeContdown = 30;
    private float end;
    private bool paused;
    private bool finished = false;
    private bool used;

    private void Start()
    {
        timer = timeContdown;
        paused = true;
        used = false;
    }

    private void FixedUpdate()
    {
        if (!paused)
        {
            if (timer <= 0 || finished)
            {
                paused = true;
                finished = true;
            }
            else
            {
                timer -= UnityEngine.Time.deltaTime;
            }
        }
    }

    public void setTimer(float limit)
    {
        timeContdown = limit;
    }

    public void start()
    {
        timer = timeContdown;
        used = true;
        paused = false;
        finished = false;
    }

    public bool hasBeenUsed()
    {
        return used;
    }

    public void finish()
    {
        paused = true;
        finished = true;
    }

    public bool finishedTimer()
    {
        return finished;
    }

    public bool pausedTimer()
    {
        return paused;
    }

    public float getElapsedTime()
    {
        return timeContdown - timer;
    }

    public float getTimeToFinish()
    {
        return timer;
    }

    public void resetTimer()
    {
        timer = timeContdown;
        paused = true;
        finished = false;
    }

    public void pauseTimer()
    {
        paused = true;
    }

    public void continueTimer()
    {
        paused = false;
    }
}