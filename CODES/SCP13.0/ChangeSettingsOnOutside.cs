using CustomRendering;
using UnityEngine;

public class ChangeSettingsOnOutside : MonoBehaviour
{
    public bool changeClearFlags;

    private Camera _myCamera;

    public ChangeSettingsOnOutside()
    {
        changeClearFlags = true;
    }

    private void Update()
    {
        Transform root = transform.root;
        if (!root.TryGetComponent(out ReferenceHub hub) || !hub.isLocalPlayer)
        {
            Destroy(this);
            return;
        }

        Outside.OnSetOutside += Singleton_OnSetOutside;

        _myCamera = GetComponent<Camera>();
        if (_myCamera == null)
        {
            Destroy(this);
            return;
        }

        if (Outside.Singleton != null)
        {
            bool currentState = Outside.Singleton.LocalHostIsOutside;

            if (FogController.Singleton != null)
            {
                var fogEffect = FogController.Singleton.FogEffect;
                if (fogEffect != null)
                    fogEffect.CoverSkybox = currentState ? 0f : 1f;
            }

            if (changeClearFlags)
                _myCamera.clearFlags = currentState
                    ? CameraClearFlags.Skybox
                    : CameraClearFlags.SolidColor;
        }

        enabled = false;
    }

    private void OnDestroy()
    {
        if (!enabled)
            Outside.OnSetOutside -= Singleton_OnSetOutside;
    }

    private void Singleton_OnSetOutside(bool isOutside)
    {
        if (_myCamera == null)
        {
            Destroy(this);
            return;
        }

        if (FogController.Singleton != null)
        {
            var fogEffect = FogController.Singleton.FogEffect;
            if (fogEffect != null)
                fogEffect.CoverSkybox = isOutside ? 0f : 1f;
        }

        if (changeClearFlags)
        {
            _myCamera.clearFlags = isOutside
                ? CameraClearFlags.Skybox
                : CameraClearFlags.SolidColor;
        }
    }
}