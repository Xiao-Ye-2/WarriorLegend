using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FadeEvent_SO", menuName = "ScriptableObjects/FadeEvent")]
public class FadeEvent_SO : ScriptableObject
{
    public event UnityAction<Color, float, bool> OnEventRaised;
    public void FadeIn(float duration)
    {
        RaiseEvent(Color.black, duration, true);
    }

    public void FadeOut(float duration)
    {
        RaiseEvent(Color.clear, duration, false);
    }

    private void RaiseEvent(Color color, float duration, bool isFadeIn)
    {
        OnEventRaised?.Invoke(color, duration, isFadeIn);
    }
}
