using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    private Light luz;
    [SerializeField] private MyTimer timer;
    public PlayerInfo playerInfo;
    private float initialIntensity;

    // Start is called before the first frame update
    void Start()
    {
        luz = GetComponent<Light>();
        timer.setTimer(playerInfo.batteryCapacity);
        timer.start();
        initialIntensity = luz.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        bajarLuz();
    }

    /*
     * cuando el contador vaya bajando ira tomando diferentes colores
     */
    void bajarLuz()
    {
        if (!timer.finishedTimer())
        {
            if (timer.getTimeToFinish() <= 0)
            {
                luz.intensity = initialIntensity * 0.1f;
                playerInfo.needsToRecharge = true;
            }
            else if (timer.getTimeToFinish() <= playerInfo.batteryCapacity * 0.25)
            {
                luz.intensity *= 0.99f;
                //algo para que vaya a por bateria
                playerInfo.needsToRecharge=true;
            }
            else
            {
                luz.intensity = initialIntensity;
                playerInfo.needsToRecharge = false;
            }
        }
    }
    
    
    //flashear
    public void flashTorch()
    {
        timer.takeTime(10);
    }

    //reestablecer tiempo
    public void chargeBattery()
    {
        timer.resetTimer();
        timer.start();
    }

    public bool CanDefend()
    {
        return timer.getTimeToFinish() > 10;
    }
}