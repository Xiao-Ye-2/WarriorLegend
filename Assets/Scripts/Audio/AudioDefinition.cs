using UnityEngine;

public class AudioDefinition : MonoBehaviour
{
    public AudioEvent_SO playAudioEvent;
    public AudioClip audioClip;
    public bool playOnEnable;

    private void OnEnable()
    {
        if (playOnEnable)
        {
            PlayAudioClip();
        }
    }

    public void PlayAudioClip()
    {
        playAudioEvent.PlayAudio(audioClip);
    }

}
