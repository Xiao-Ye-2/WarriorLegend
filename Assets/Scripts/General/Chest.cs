using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public Sprite openChestSprite;
    public Sprite closedChestSprite;
    public bool isOpen = false;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = isOpen ? openChestSprite : closedChestSprite;
        GetComponent<Collider2D>().enabled = !isOpen;
    }

    public void TriggerAction()
    {
        if (!isOpen)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        isOpen = true;
        spriteRenderer.sprite = openChestSprite;
        GetComponent<AudioDefinition>()?.PlayAudioClip();
        GetComponent<Collider2D>().enabled = false;
    }
}
