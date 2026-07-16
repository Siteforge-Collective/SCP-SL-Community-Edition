using TMPro;
using UnityEngine;
using CentralAuth; 

public class AuthStatus : MonoBehaviour
{
    public GameObject LoadingCircle;
    public TextMeshProUGUI Description;
    public NewServerBrowser ServerBrowser;

    private AuthStatusType _lastStatus = (AuthStatusType)255;

    private void Awake()
    {
        if (Description != null)
        {
            string initialText = TranslationReader.Get("NewMainMenu", 68, "Connecting to central servers...");
            Description.SetText(initialText);
        }
    }

    private void Update()
    {
        AuthStatusType currentStatus = CentralAuthManager.AuthStatusType;

        if (currentStatus != _lastStatus)
        {
            _lastStatus = currentStatus;
            UpdateStatusUI(currentStatus);
        }
    }

    private void UpdateStatusUI(AuthStatusType status)
    {
        if (Description == null || LoadingCircle == null) return;

        switch (status)
        {
            case AuthStatusType.Connecting:
                LoadingCircle.SetActive(true);
                Description.SetText(TranslationReader.Get("NewMainMenu", 68, "Connecting to central servers..."));
                break;

            case AuthStatusType.Success:
                LoadingCircle.SetActive(false);
                Description.SetText(TranslationReader.Get("NewMainMenu", 69, "<color=green>Connection established</color>"));

                if (ServerBrowser != null)
                {
                    ServerBrowser.AuthCompleted();
                }
                break;

            case AuthStatusType.Failure:
                LoadingCircle.SetActive(false);
                Description.SetText(TranslationReader.Get("NewMainMenu", 72, "<color=red>Authentication failure (Check console for details)</color>"));
                break;

            case AuthStatusType.PlatformAuthFailure:
                LoadingCircle.SetActive(false);
                Description.SetText(TranslationReader.Get("NewMainMenu", 71, "<color=red>Platform authentication failed</color>"));
                break;
        }
    }
}