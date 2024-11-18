using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidEvent_SO", menuName = "ScriptableObjects/VoidEvent")]
public class VoidEvent_SO : ScriptableObject
{
    public UnityAction voidEvent;

    public void Raise()
    {
        voidEvent?.Invoke();
    }
}
