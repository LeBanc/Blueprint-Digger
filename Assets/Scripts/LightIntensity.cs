using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensity : MonoBehaviour
{
    public float minIntensity=0.05f;
    public float maxIntensity=0.3f;
    public float refreshTime = 5f;

    private Light currentLight;
    private float intensity;

    // Start is called before the first frame update
    void Start()
    {
        currentLight = GetComponent<Light>();
        intensity = maxIntensity;
        currentLight.intensity = intensity;
        StartCoroutine(LightIntensityUpdate()); // Launch LightIntensityUpdate coroutine at start
    }

    // Coroutine LightIntensityUpdate makes the light blinks every refresh time with 1 second random delay
    IEnumerator LightIntensityUpdate()
    {
        while (true)
        {
            currentLight.intensity = maxIntensity + Random.Range(-0.05f,0.05f);
            yield return new WaitForSeconds(refreshTime + Random.Range(0f, 1f));
            intensity = Random.Range(minIntensity, maxIntensity);
            currentLight.intensity = intensity;
            yield return new WaitForSeconds(0.1f);
            intensity = Random.Range(minIntensity, maxIntensity);
            currentLight.intensity = intensity;
            yield return new WaitForSeconds(0.1f);
            intensity = Random.Range(minIntensity, maxIntensity);
            currentLight.intensity = intensity;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
