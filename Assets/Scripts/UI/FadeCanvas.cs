using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FadeCanvas : MonoBehaviour
{
    public FadeEvent_SO fadeEvent;
    public Image fadeImage;

    private void OnEnable()
    {
        fadeEvent.OnEventRaised += OnFadeEvent;
    }

    private void OnDisable()
    {
        fadeEvent.OnEventRaised -= OnFadeEvent;
    }

    private void OnFadeEvent(Color color, float duration, bool _)
    {
        fadeImage.DOBlendableColor(color, duration);
    }

}
