using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioEvent_SO BGMEvent;
    public AudioEvent_SO FXEvent;
    public AudioSource BGMSource;
    public AudioSource FXSource;

    private void OnEnable()
    {
        BGMEvent.OnPlayAudio += PlayBGM;
        FXEvent.OnPlayAudio += PlayFX;
    }
    private void OnDisable()
    {
        BGMEvent.OnPlayAudio -= PlayBGM;
        FXEvent.OnPlayAudio -= PlayFX;
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
