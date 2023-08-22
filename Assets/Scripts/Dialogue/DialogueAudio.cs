using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAudio : MonoBehaviour
{
    [SerializeField] string directory;
    private AudioClip[] sounds;
    private AudioSource source;
    private AudioClip clip;
    private int index;

    // Resources is a folder in Assets that's easy to access in code. To use the directory, just put the 
    // path to the folder of sounds you want to use from inside the resources folder.
    void Start() {
        source = GetComponent<AudioSource>();
        sounds = Resources.LoadAll<AudioClip>(directory);
        Random.seed = (int)System.DateTime.Now.Ticks;
    }
    public void PlayDialogueSFX() {
        if(!source.isPlaying) {
            index = Random.Range(0, sounds.Length);
            clip = sounds[index];
            source.PlayOneShot(clip);
        }   
    }
    public void Stop() {
        source.Stop();
    }
}
