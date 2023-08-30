using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMEnqueuer : MonoBehaviour
{
    [SerializeField] private AudioClip BGMClip;
    [Range(-2, 2)]
    [SerializeField] private float pitch = 1f;

    void Start()
    {
        // Get audio system
        AudioSystem audioSystem = FindObjectOfType<AudioSystem>();
        if (audioSystem == null) {
            Debug.LogError("[BGMEnqueuer] Could not find audio system to enqueue BGM");
            return;
        }

        // Enqueue the BGM
        if (BGMClip) {
            StartCoroutine(audioSystem.PlayBGM(BGMClip, pitch));
        } else {
            Debug.LogError("[BGMEnqueuer] No BGM clip assigned");
        }
    }

}
