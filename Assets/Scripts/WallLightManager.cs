using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLightManager : MonoBehaviour
{
    public bool lightOn;
    public GameObject[] lightObjects;

    private void Start()
    {
        if (lightOn) ActivateLight(); // for startBuildings which lights are already active at game start
    }

    // ActivateLight function activate randomly one of the light GameObject of the building, if one exists
    public void ActivateLight()
    {
        DeactivateLight();
        if (lightObjects.Length > 0)
        {
            lightOn = true;
            int _lightNb = Random.Range(0, lightObjects.Length);
            lightObjects[_lightNb].SetActive(true);
        }
    }

    // DeactivateLight function deactivate all light GameObjects of the building
    public void DeactivateLight()
    {
        lightOn = false;
        if (lightObjects.Length > 0)
        {
            for (int i = 0; i < lightObjects.Length; i++)
            {
                lightObjects[i].SetActive(false);
            }
        }
    }

    // GetLightStatus return the lightOn boolean
    public bool GetLightStatus()
    {
        return lightOn;
    }



}
