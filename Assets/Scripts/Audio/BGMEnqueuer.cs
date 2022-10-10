using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMEnqueuer : MonoBehaviour
{
    [SerializeField] private AudioClip BGMClip;

    void Start()
    {
        AudioSystem audioSystem = FindObjectOfType<AudioSystem>();
        if (BGMClip)
            StartCoroutine(audioSystem.PlayBGM(BGMClip));
        else
            Debug.Log("No BGM clip assigned to BGMEnqueuer.");
    }

}
