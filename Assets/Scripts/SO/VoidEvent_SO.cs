using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidEvent_SO", menuName = "ScriptableObjects/VoidEvent")]
public class VoidEvent_SO : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}
