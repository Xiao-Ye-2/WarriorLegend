using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FloatEvent_SO", menuName = "ScriptableObjects/FloatEvent")]
public class FloatEvent_SO : ScriptableObject
{
    public UnityAction<float> OnEventRaised;
    public void RaiseEvent(float value)
    {
        OnEventRaised?.Invoke(value);
    }
}
