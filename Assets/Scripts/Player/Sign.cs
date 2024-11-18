using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class Sign : MonoBehaviour
{
    public GameObject signSprite;
    public Transform playerTrans;
    private bool canPress;
    private Animator anim;
    private PlayerInputControl playerInputControl;
    private IInteractable target;

    private void Awake()
    {
        // anim = GetComponentInChildren<Animator>(); Broken if disabled
        anim = signSprite.GetComponent<Animator>();
        playerInputControl = new PlayerInputControl();
        playerInputControl.Enable();
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        playerInputControl.Gameplay.Confirm.started += OnConfirm;
    }

    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (canPress)
        {
            target?.TriggerAction();
        }
    }

    private void OnActionChange(object arg1, InputActionChange change)
    {
        if (change == InputActionChange.ActionStarted)
        {
            var d = ((InputAction)arg1).activeControl.device;
            switch (d)
            {
                case Keyboard:
                    anim.Play("keyBoard");
                    break;
                case DualShockGamepad:
                    anim.Play("ps");
                    break;
            }
        }
    }

    private void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        signSprite.transform.localScale = playerTrans.localScale;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            canPress = true;
            target = collision.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canPress = false;
    }
}
