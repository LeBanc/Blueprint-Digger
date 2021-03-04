using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashController : MonoBehaviour
{
    public float destroyDelay = 0.6f;
    public float randomFactor = 0.3f;

    // At activation, all trash are set to be destroyed after a delay based on destroyDelay and randomFactor
    private void Start()
    {
        float delay = Random.Range(destroyDelay * (1f-randomFactor), destroyDelay * (1+randomFactor));
        Destroy(this.gameObject, delay);
    }
}
