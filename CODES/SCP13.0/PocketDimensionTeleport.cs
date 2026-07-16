using CustomPlayerEffects;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp106;
using PlayerStatsSystem;
using PluginAPI.Events;
using UnityEngine;

public class PocketDimensionTeleport : NetworkBehaviour
{
    public delegate void PlayerEscapePocketDimension(ReferenceHub hub);

    public enum PDTeleportType
    {
        Killer = 0,
        Exit = 1
    }

    public const float DisabledDuration = 10f;

    public static bool DebugBool;

    private const float MinAliveDuration = 1f;

    private PDTeleportType _type;

    public static bool RefreshExit;

    public static event PlayerEscapePocketDimension OnPlayerEscapePocketDimension;

    public void SetType(PDTeleportType t)
    {
        _type = t;
    }

    public PDTeleportType GetTeleportType()
    {
        return _type;
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (!NetworkServer.active)
        {
            return;
        }
        NetworkIdentity component = other.GetComponent<NetworkIdentity>();
        if (component == null || !ReferenceHub.TryGetHubNetID(component.netId, out var hub) || hub.roleManager.CurrentRole.ActiveTime < 1f)
        {
            return;
        }
        if ((_type == PDTeleportType.Killer || AlphaWarheadController.Detonated) && !DebugBool)
        {
            if (EventManager.ExecuteEvent(new PlayerExitPocketDimensionEvent(hub, isSuccessful: false)))
            {
                hub.playerStats.DealDamage(new UniversalDamageHandler(-1f, DeathTranslations.PocketDecay));
            }
        }
        else if ((_type == PDTeleportType.Exit || DebugBool) && hub.roleManager.CurrentRole is IFpcRole fpcRole && EventManager.ExecuteEvent(new PlayerExitPocketDimensionEvent(hub, isSuccessful: true)))
        {
            fpcRole.FpcModule.ServerOverridePosition(Scp106PocketExitFinder.GetBestExitPosition(fpcRole), Vector3.zero);
            hub.playerEffectsController.EnableEffect<Disabled>(10f, addDuration: true);
            hub.playerEffectsController.EnableEffect<Traumatized>();
            hub.playerEffectsController.DisableEffect<PocketCorroding>();
            hub.playerEffectsController.DisableEffect<Corroding>();
            PocketDimensionTeleport.OnPlayerEscapePocketDimension?.Invoke(hub);
            PocketDimensionGenerator.RandomizeTeleports();
        }
    }
}
