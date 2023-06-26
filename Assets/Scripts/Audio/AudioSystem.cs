using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Singleton))]
public class AudioSystem : MonoBehaviour
{
    // Audio mixer
    [Header("Audio Mixer")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioMixerParamsConfig mixerParams;

    // Audio source
    [Header("Audio Sources")]
    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource SFXSource;

    void Start()
    {
        // Apply player prefs to audio mixer parameters
        LoadAudioMixerParams();

        SetupSaveOnQuit();
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
    
    // Plays a sound effect audio clip
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    // Fades out the background music
    public IEnumerator PlayBGM(AudioClip clip, float fadeDuration = 1f, float delay = 0f)
    {
        yield return FadeBGMVolume(0f, fadeDuration); 
        BGMSource.clip = clip;
        BGMSource.PlayDelayed(delay);
        yield return FadeBGMVolume(1f, fadeDuration);
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