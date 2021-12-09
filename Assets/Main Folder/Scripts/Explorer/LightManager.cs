using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private CharacterManager mainCharacter;
    private Light luz;
    [SerializeField] private MyTimer timer;
    public PlayerInfo playerInfo;
    private float initialIntensity;
    private Color initialColor;
    private bool isFlashing;

    // Start is called before the first frame update
    void Start()
    {

        luz = GetComponent<Light>();
        timer.setTimer(playerInfo.batteryCapacity);
        timer.start();
        initialIntensity = luz.intensity;
        initialColor = luz.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFlashing)
        {
            bajarLuz();
        }
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
                playerInfo.needsToRecharge = true;
            }
            else
            {
                luz.intensity = initialIntensity;
                playerInfo.needsToRecharge = false;
            }
        }
    }

    //reestablecer tiempo
    public void chargeBattery()
    {
        timer.resetTimer();
        timer.start();
    }

    public bool CanDefend(Vector3 ghostPosition)
    {
        
        timer.takeTime(30);
        if (!(timer.getTimeToFinish() > 0)) return false;
        StartCoroutine(Flash(ghostPosition));
        return true;
    }

    private IEnumerator Flash(Vector3 ghostPosition)
    {
        isFlashing = true;
        luz.intensity = 120.0f;
        luz.color = Color.yellow;
        var aux = mainCharacter.characterLabel.text;
        mainCharacter.characterLabel.text = "FLASH!";
        var oldRotation = transform.rotation;
        transform.LookAt(ghostPosition);
        yield return new WaitForSeconds(0.5f);
        transform.rotation = oldRotation;
        mainCharacter.characterLabel.text = aux;
        luz.intensity = initialIntensity;
        luz.color = initialColor;
        isFlashing = false;
    }
}