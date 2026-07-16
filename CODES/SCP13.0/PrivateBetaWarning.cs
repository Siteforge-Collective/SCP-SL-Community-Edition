using TMPro;
using UnityEngine;
using GameCore;

public class PrivateBetaWarning : MonoBehaviour
{
    public TextMeshProUGUI text;

    private const string ContentPrivateBeta = "<size=32>PRIVATE BETA - restricted access</size>";
    private const string DevelopmentBuild = "<size=32>DEVELOPMENT BUILD - restricted access</size>";
    private const string NightlyBuild = "<size=32>NIGHTLY BUILD - restricted access</size>";
    private const string ContentPublicBeta = "<size=32>PUBLIC BETA - all content is subject to change!</size>";

    private const string StreamingAllowedSuffix = "Sharing footage (recording or streaming) is allowed";
    private const string StreamingDisallowedSuffix = "CONFIDENTIAL - sharing or publishing any information about this version (including recording or streaming) is strictly prohibited";
    private const string DoNotShareSuffix = "Do not share or distribute any game files";

    private void Start()
    {
        if (Version.BuildType == (Version.VersionType)19 || Version.BuildType == (Version.VersionType)26)
        {
            return;
        }

        if (Version.PrivateBeta)
        {
            string header = ContentPrivateBeta;

            if (Version.BuildType == Version.VersionType.Development)
            {
                header = DevelopmentBuild;
            }
            else if (Version.BuildType == Version.VersionType.Nightly)
            {
                header = NightlyBuild;
            }

            string streamingText = Version.StreamingAllowed ? StreamingAllowedSuffix : StreamingDisallowedSuffix;

            text.text = $"{header}\n{streamingText}\n{DoNotShareSuffix}\nGame version: {Version.VersionString}";
        }
        else if (Version.PublicBeta)
        {
            text.text = $"{ContentPublicBeta}\nGame version: {Version.VersionString}";
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}