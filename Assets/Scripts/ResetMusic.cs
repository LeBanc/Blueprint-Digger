using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        MusicAudioSource musicSource = FindObjectOfType<MusicAudioSource>();
        if(musicSource != null) Destroy(musicSource.gameObject);
        Destroy(gameObject);
    }
}
