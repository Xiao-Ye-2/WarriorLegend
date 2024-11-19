using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    public VoidEvent_SO saveGameEvent;
    public SpriteRenderer spriteRenderer;
    public Sprite savePointOn;
    public Sprite savePointOff;
    public GameObject lightObj;
    private bool isDone;

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? savePointOn : savePointOff;
        lightObj.SetActive(isDone);
    }

    public void TriggerAction()
    {
        if (!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = savePointOn;
            GetComponent<Collider2D>().enabled = false;
            lightObj.SetActive(true);
            saveGameEvent.RaiseEvent();
        }
    }
}
