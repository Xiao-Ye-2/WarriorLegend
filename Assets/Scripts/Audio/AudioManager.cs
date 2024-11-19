using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioEvent_SO BGMEvent;
    public AudioEvent_SO FXEvent;
    public FloatEvent_SO syncVolumeEvent;
    public FloatEvent_SO volumeEvent;
    public VoidEvent_SO pauseGameEvent;
    public AudioSource BGMSource;
    public AudioSource FXSource;
    public AudioMixer mixer;

    private void OnEnable()
    {
        BGMEvent.OnPlayAudio += PlayBGM;
        FXEvent.OnPlayAudio += PlayFX;
        volumeEvent.OnEventRaised += SetMasterVolume;
        pauseGameEvent.OnEventRaised += OnPauseEvent;
    }

    private void OnPauseEvent()
    {
        float volume;
        mixer.GetFloat("MasterVolume", out volume);
        syncVolumeEvent.RaiseEvent(volume);
    }

    private void OnDisable()
    {
        BGMEvent.OnPlayAudio -= PlayBGM;
        FXEvent.OnPlayAudio -= PlayFX;
        volumeEvent.OnEventRaised -= SetMasterVolume;
        pauseGameEvent.OnEventRaised -= OnPauseEvent;
    }

    private void SetMasterVolume(float arg0)
    {
        mixer.SetFloat("MasterVolume", arg0 * 100 - 80);
    }

    private void PlayFX(AudioClip arg0)
    {
        FXSource.clip = arg0;
        FXSource.Play();
    }

    private void PlayBGM(AudioClip arg0)
    {
        BGMSource.clip = arg0;
        BGMSource.Play();
    }
}
