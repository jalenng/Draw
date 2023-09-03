using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAudio : MonoBehaviour
{
    private string currDirectory;
    private AudioClip[] sounds;
    private AudioSource source;
    private AudioClip clip;
    private int index;

    // Resources is a folder in Assets that's easy to access in code. To use the directory, just put the 
    // path to the folder of sounds you want to use from inside the resources folder.
    void Start()
    {
        source = GetComponent<AudioSource>();
        UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks;
    }

    public void SetSFXDirectory(string directory)
    {
        if (directory != currDirectory)
        {
            try
            {
                currDirectory = directory;
                sounds = Resources.LoadAll<AudioClip>(currDirectory);
            }
            catch (Exception e)
            {
                Debug.LogError($"[DialogueAudio] Failed to load resources from {directory}: {e}");
            }
        }
    }

    public void Speak()
    {
        if (!source.isPlaying && sounds.Length > 0)
        {
            index = UnityEngine.Random.Range(0, sounds.Length);
            clip = sounds[index];
            source.PlayOneShot(clip);
        }
    }
}
