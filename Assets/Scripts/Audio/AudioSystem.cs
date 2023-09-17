using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// AudioSystem will be used to play non-repeating miscellaneous sound effects. Sound effects that loop use a true/false ifPlaying
// to stop overlap, and will be played through another sound source.
[RequireComponent(typeof(Singleton))]
public class AudioSystem : MonoBehaviour
{
    public static AudioSystem audioPlayer;
    // Audio mixer
    [Header("Audio Mixer")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioMixerParamsConfig mixerParams;

    // Audio source
    [Header("Audio Source")]
    [SerializeField] AudioSource BGMSource;

    void Start()
    {
        // Apply player prefs to audio mixer parameters
        LoadAudioMixerParams();

        SetupSaveOnQuit();
        audioPlayer = this;
    }

    // Ensures that the audio mixer parameters are saved when the game is quit.
    void SetupSaveOnQuit()
    {
        Application.quitting += SaveAudioMixerParams;
    }

    void LoadAudioMixerParams()
    {
        // Get master volume
        string masterVolumeParam = mixerParams.masterVolume;
        audioMixer.SetFloat(masterVolumeParam, PlayerPrefs.GetFloat(masterVolumeParam, 0f));

        // Get BGM volume
        string BGMVolumeParam = mixerParams.BGMVolume;
        audioMixer.SetFloat(BGMVolumeParam, PlayerPrefs.GetFloat(BGMVolumeParam, 0f));

        // Get SFX volume
        string SFXVolumeParam = mixerParams.SFXVolume;
        audioMixer.SetFloat(SFXVolumeParam, PlayerPrefs.GetFloat(SFXVolumeParam, 0f));
    }

    void SaveAudioMixerParams()
    {
        // Save master volume
        string masterVolumeParam = mixerParams.masterVolume;
        float masterVolume;
        audioMixer.GetFloat(masterVolumeParam, out masterVolume);
        PlayerPrefs.SetFloat(masterVolumeParam, masterVolume);

        // Save BGM volume
        string BGMVolumeParam = mixerParams.BGMVolume;
        float BGMVolume;
        audioMixer.GetFloat(BGMVolumeParam, out BGMVolume);
        PlayerPrefs.SetFloat(BGMVolumeParam, BGMVolume);

        // Save SFX volume
        string SFXVolumeParam = mixerParams.SFXVolume;
        float SFXVolume;
        audioMixer.GetFloat(SFXVolumeParam, out SFXVolume);
        PlayerPrefs.SetFloat(SFXVolumeParam, SFXVolume);
    }

    // Fade out the current BGM, then fade in a new BGM
    public IEnumerator PlayBGM(AudioClip clip, float pitch = 1f, float fadeDuration = 1f, float delay = 0f)
    {
        // Do not replay the clip if it is already playing at the given pitch
        if (BGMSource.clip != clip || BGMSource.pitch != pitch)
        {
            // If currently playing a clip, fade out first
            if (BGMSource.clip != null)
            {
                yield return FadeBGMVolume(0f, fadeDuration);
            }
            BGMSource.clip = clip;
            BGMSource.pitch = pitch;
            BGMSource.PlayDelayed(delay);
            yield return FadeBGMVolume(1f, fadeDuration);
        }
    }

    // Fade out the current BGM to silence
    public IEnumerator StopBGM(float fadeDuration = 1f)
    {
        if (BGMSource.clip != null)
        {
            yield return FadeBGMVolume(0f, fadeDuration);
            BGMSource.clip = null;
        }
    }

    // Helper function to facilitate fading
    IEnumerator FadeBGMVolume(float targetVolume, float time)
    {
        float startVolume = BGMSource.volume;
        float startTime = Time.time;

        // Adjust the volume towards the end volume
        while (Time.time < startTime + time)
        {
            BGMSource.volume = Mathf.Lerp(startVolume, targetVolume, (Time.time - startTime) / time);
            yield return null;
        }

        BGMSource.volume = targetVolume;
    }
}