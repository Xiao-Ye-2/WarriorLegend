using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CharacterEvent_SO", menuName = "ScriptableObjects/CharacterEvent")]
public class CharacterEvent_SO : ScriptableObject
{
    public UnityAction<Character> OnEventRaised;
    public void RaiseEvent(Character character)
    {
        OnEventRaised?.Invoke(character);
    }
}
