using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "AudioEvent_SO", menuName = "ScriptableObjects/AudioEvent", order = 1)]
public class AudioEvent_SO : ScriptableObject
{
    public event UnityAction<AudioClip> OnPlayAudio;
    public void PlayAudio(AudioClip clip)
    {
        OnPlayAudio?.Invoke(clip);
    }
}
