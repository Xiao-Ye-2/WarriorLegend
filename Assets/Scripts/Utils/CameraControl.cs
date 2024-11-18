using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public VoidEvent_SO afterSceneLoadedEvent;
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    private void OnEnable()
    {
        EventHandler.CameraShakeEvent += OnCameraShakeEvent;
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadEvent;
    }

    private void OnDisable()
    {
        EventHandler.CameraShakeEvent -= OnCameraShakeEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadEvent;
    }

    private void OnAfterSceneLoadEvent()
    {
        GetNewCameraBounds();
    }

    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

    private void GetNewCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null) return;

        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache();
    }
}
