using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CentralAuth;
using GameCore;
using InventorySystem.Disarming;
using MEC;
using Mirror;
using Mirror.RemoteCalls;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Events;
using Respawning;
using RoundRestarting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.NonAllocLINQ;

public class RoundSummary : NetworkBehaviour
{
    public enum LeadingTeam : byte
    {
        FacilityForces = 0,
        ChaosInsurgency = 1,
        Anomalies = 2,
        Draw = 3
    }

    [Serializable]
    public struct SumInfo_ClassList : IEquatable<SumInfo_ClassList>
    {
        public int class_ds;
        public int scientists;
        public int chaos_insurgents;
        public int mtf_and_guards;
        public int scps_except_zombies;
        public int zombies;
        public int warhead_kills;

        public bool Equals(SumInfo_ClassList other)
        {
            if (class_ds == other.class_ds && scientists == other.scientists && chaos_insurgents == other.chaos_insurgents && mtf_and_guards == other.mtf_and_guards && scps_except_zombies == other.scps_except_zombies && zombies == other.zombies)
            {
                return warhead_kills == other.warhead_kills;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is SumInfo_ClassList other)
            {
                return Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (((((((((((class_ds * 397) ^ scientists) * 397) ^ chaos_insurgents) * 397) ^ mtf_and_guards) * 397) ^ scps_except_zombies) * 397) ^ zombies) * 397) ^ warhead_kills;
        }

        public static bool operator ==(SumInfo_ClassList left, SumInfo_ClassList right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SumInfo_ClassList left, SumInfo_ClassList right)
        {
            return !left.Equals(right);
        }
    }

    private const double RespawnTargetMultiplier = 0.75;

    [SyncVar]
    private int _chaosTargetCount;

    private bool _roundEnded;

    private bool _summaryActive;

    public bool KeepRoundOnOne;

    public SumInfo_ClassList classlistStart;

    public GameObject ui_root;

    public static bool RoundLock;

    public Image fadeOutImage;

    public TextMeshProUGUI ui_text_header;

    public TextMeshProUGUI ui_text_who_won;

    public TextMeshProUGUI ui_text_info;

    public static RoundSummary singleton;

    private static bool _singletonSet;

    public static int roundTime;

    public static bool SummaryActive
    {
        get
        {
            if (_singletonSet)
            {
                return singleton._summaryActive;
            }
            return false;
        }
    }

    public int ChaosTargetCount
    {
        get
        {
            return _chaosTargetCount;
        }
        set
        {
            if (NetworkServer.active)
            {
                _chaosTargetCount = Mathf.Max(value, 0);
            }
        }
    }

    public static int Kills { get; private set; }
    public static int EscapedClassD { get; private set; }
    public static int EscapedScientists { get; private set; }
    public static int SurvivingSCPs { get; private set; }
    public static int KilledBySCPs { get; private set; }
    public static int ChangedIntoZombies { get; private set; }

    private void Start()
    {
        singleton = this;
        _singletonSet = true;
        if (NetworkServer.active)
        {
            roundTime = 0;
            KeepRoundOnOne = !ConfigFile.ServerConfig.GetBool("end_round_on_one_player");
            Timing.RunCoroutine(_ProcessServerSideCode(), Segment.FixedUpdate);
            KilledBySCPs = 0;
            EscapedClassD = 0;
            EscapedScientists = 0;
            ChangedIntoZombies = 0;
            Kills = 0;
            ChaosTargetCount = ReferenceHub.AllHubs.Count((ReferenceHub hub) => hub.GetTeam() == Team.ChaosInsurgency);
            PlayerRoleManager.OnServerRoleSet += OnServerRoleSet;
            RespawnManager.ServerOnRespawned += ServerOnRespawned;
            PlayerStats.OnAnyPlayerDied += OnAnyPlayerDied;
        }
    }

    private void OnDestroy()
    {
        _singletonSet = false;
        PlayerRoleManager.OnServerRoleSet -= OnServerRoleSet;
        RespawnManager.ServerOnRespawned -= ServerOnRespawned;
        PlayerStats.OnAnyPlayerDied -= OnAnyPlayerDied;
    }

    private void OnAnyPlayerDied(ReferenceHub ply, DamageHandlerBase handler)
    {
        Kills++;
        PlayerRoleBase result;
        if (handler is UniversalDamageHandler universalDamageHandler)
        {
            if (universalDamageHandler.TranslationId != DeathTranslations.PocketDecay.Id)
            {
                return;
            }
        }
        else if (handler is not AttackerDamageHandler attackerDamageHandler || !PlayerRoleLoader.TryGetRoleTemplate<PlayerRoleBase>(attackerDamageHandler.Attacker.Role, out result) || result.Team != Team.SCPs)
        {
            return;
        }
        KilledBySCPs++;
    }

    private void OnServerRoleSet(ReferenceHub userHub, RoleTypeId newRole, RoleChangeReason reason)
    {
        switch (reason)
        {
            case RoleChangeReason.RoundStart:
            case RoleChangeReason.LateJoin:
                ModifySpawnedTeam(userHub.GetTeam(), -1);
                ModifySpawnedTeam(newRole.GetTeam(), 1);
                break;
            case RoleChangeReason.Escaped:
                if (!userHub.inventory.IsDisarmed())
                {
                    switch (newRole.GetTeam())
                    {
                        case Team.FoundationForces:
                            EscapedScientists++;
                            break;
                        case Team.ChaosInsurgency:
                            ChaosTargetCount++;
                            EscapedClassD++;
                            break;
                    }
                }
                break;
            case RoleChangeReason.Revived:
                ChangedIntoZombies++;
                classlistStart.zombies++;
                break;
            case RoleChangeReason.RemoteAdmin:
                {
                    bool flag = userHub.roleManager.CurrentRole.Team == Team.ChaosInsurgency;
                    bool flag2 = newRole.GetTeam() == Team.ChaosInsurgency;
                    if (!flag && flag2)
                    {
                        ChaosTargetCount++;
                    }
                    else if (flag && !flag2)
                    {
                        ChaosTargetCount--;
                    }
                    break;
                }
            case RoleChangeReason.Died:
            case RoleChangeReason.Destroyed:
                if (userHub.roleManager.CurrentRole.Team == Team.ChaosInsurgency)
                {
                    ChaosTargetCount--;
                }
                break;
            case RoleChangeReason.Respawn:
                break;
        }
    }

    private void ServerOnRespawned(SpawnableTeamType team, List<ReferenceHub> respawnedPlayers)
    {
        if (team == SpawnableTeamType.ChaosInsurgency)
        {
            ChaosTargetCount += (int)((double)respawnedPlayers.Count * 0.75);
        }
    }

    private void ModifySpawnedTeam(Team t, int modifyAmount)
    {
        switch (t)
        {
            case Team.ChaosInsurgency:
                classlistStart.chaos_insurgents += modifyAmount;
                break;
            case Team.ClassD:
                classlistStart.class_ds += modifyAmount;
                break;
            case Team.FoundationForces:
                classlistStart.mtf_and_guards += modifyAmount;
                break;
            case Team.Scientists:
                classlistStart.scientists += modifyAmount;
                break;
            case Team.SCPs:
                classlistStart.scps_except_zombies += modifyAmount;
                break;
        }
    }

    public void ForceEnd()
    {
        _roundEnded = true;
    }

    public int CountRole(RoleTypeId role)
    {
        return ReferenceHub.AllHubs.Count((ReferenceHub x) => x.GetRoleId() == role);
    }

    public int CountTeam(Team team)
    {
        return ReferenceHub.AllHubs.Count((ReferenceHub x) => x.GetTeam() == team);
    }

    private IEnumerator<float> _ProcessServerSideCode()
    {
        float time = Time.unscaledTime;
        while (this != null)
        {
            yield return Timing.WaitForSeconds(2.5f);
            if (RoundLock || (KeepRoundOnOne && ReferenceHub.AllHubs.Count((ReferenceHub x) => x.authManager.InstanceMode != ClientInstanceMode.DedicatedServer) < 2) || !RoundInProgress() || Time.unscaledTime - time < 15f)
            {
                continue;
            }
            SumInfo_ClassList newList = default(SumInfo_ClassList);
            foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
            {
                switch (allHub.GetTeam())
                {
                    case Team.ClassD:
                        newList.class_ds++;
                        break;
                    case Team.ChaosInsurgency:
                        newList.chaos_insurgents++;
                        break;
                    case Team.FoundationForces:
                        newList.mtf_and_guards++;
                        break;
                    case Team.Scientists:
                        newList.scientists++;
                        break;
                    case Team.SCPs:
                        if (allHub.GetRoleId() == RoleTypeId.Scp0492)
                        {
                            newList.zombies++;
                        }
                        else
                        {
                            newList.scps_except_zombies++;
                        }
                        break;
                }
            }
            yield return float.NegativeInfinity;
            newList.warhead_kills = (AlphaWarheadController.Detonated ? AlphaWarheadController.Singleton.WarheadKills : (-1));
            yield return float.NegativeInfinity;

            int facilityForces = newList.mtf_and_guards + newList.scientists;
            int chaosInsurgency = newList.chaos_insurgents + newList.class_ds;
            int anomalies = newList.scps_except_zombies + newList.zombies;
            int escapedD = newList.class_ds + EscapedClassD;
            int escapedSci = newList.scientists + EscapedScientists;
            SurvivingSCPs = newList.scps_except_zombies;

            float dEscapePercentage = ((classlistStart.class_ds != 0) ? (escapedD / classlistStart.class_ds) : 0);
            float sEscapePercentage = ((classlistStart.scientists == 0) ? 1 : (escapedSci / classlistStart.scientists));

            bool roundEndConditionsMet;
            if (newList.class_ds <= 0 && facilityForces <= 0 && ChaosTargetCount == 0)
            {
                roundEndConditionsMet = true;
            }
            else
            {
                int opposingTeams = 0;
                if (facilityForces > 0) opposingTeams++;
                if (chaosInsurgency > 0) opposingTeams++;
                if (anomalies > 0) opposingTeams++;
                roundEndConditionsMet = opposingTeams <= 1;
            }

            if (!_roundEnded)
            {
                RoundEndConditionsCheckCancellationData.RoundEndConditionsCheckCancellation cancellation = EventManager.ExecuteEvent<RoundEndConditionsCheckCancellationData>(new RoundEndConditionsCheckEvent(roundEndConditionsMet)).Cancellation;
                if (cancellation != RoundEndConditionsCheckCancellationData.RoundEndConditionsCheckCancellation.ConditionsSatisfied)
                {
                    if (cancellation == RoundEndConditionsCheckCancellationData.RoundEndConditionsCheckCancellation.ConditionsNotSatisfied && !_roundEnded)
                    {
                        continue;
                    }
                    if (roundEndConditionsMet)
                    {
                        _roundEnded = true;
                    }
                }
                else
                {
                    _roundEnded = true;
                }
            }

            if (!_roundEnded)
            {
                continue;
            }

            bool ffAlive = facilityForces > 0;
            bool ciAlive = chaosInsurgency > 0;
            bool scpAlive = anomalies > 0;
            LeadingTeam leadingTeam = LeadingTeam.Draw;

            if (ffAlive)
            {
                leadingTeam = ((EscapedScientists < EscapedClassD) ? LeadingTeam.Draw : LeadingTeam.FacilityForces);
            }
            else if (scpAlive || (scpAlive && ciAlive))
            {
                leadingTeam = ((EscapedClassD > SurvivingSCPs) ? LeadingTeam.ChaosInsurgency : ((SurvivingSCPs > EscapedScientists) ? LeadingTeam.Anomalies : LeadingTeam.Draw));
            }
            else if (ciAlive)
            {
                leadingTeam = ((EscapedClassD >= EscapedScientists) ? LeadingTeam.ChaosInsurgency : LeadingTeam.Draw);
            }

            RoundEndCancellationData roundEndCancellationData = EventManager.ExecuteEvent<RoundEndCancellationData>(new RoundEndEvent(leadingTeam));
            while (roundEndCancellationData.IsCancelled)
            {
                if (roundEndCancellationData.Delay <= 0f)
                {
                    yield break;
                }
                yield return Timing.WaitForSeconds(roundEndCancellationData.Delay);
                roundEndCancellationData = EventManager.ExecuteEvent<RoundEndCancellationData>(new RoundEndEvent(leadingTeam));
            }

            if (Statistics.FastestEndedRound.Duration > RoundStart.RoundLength)
            {
                Statistics.FastestEndedRound = new Statistics.FastestRound(leadingTeam, RoundStart.RoundLength, DateTime.Now);
            }

            Statistics.CurrentRound.ClassDAlive = newList.class_ds;
            Statistics.CurrentRound.ScientistsAlive = newList.scientists;
            Statistics.CurrentRound.MtfAndGuardsAlive = newList.mtf_and_guards;
            Statistics.CurrentRound.ChaosInsurgencyAlive = newList.chaos_insurgents;
            Statistics.CurrentRound.ZombiesAlive = newList.zombies;
            Statistics.CurrentRound.ScpsAlive = newList.scps_except_zombies;
            Statistics.CurrentRound.WarheadKills = newList.warhead_kills;
            FriendlyFireConfig.PauseDetector = true;

            string text = "Round finished! Anomalies: " + anomalies + " | Chaos: " + chaosInsurgency + " | Facility Forces: " + facilityForces + " | D escaped percentage: " + dEscapePercentage + " | S escaped percentage: " + sEscapePercentage + ".";
            GameCore.Console.AddLog(text, Color.gray);
            ServerLogs.AddLog(ServerLogs.Modules.Logger, text, ServerLogs.ServerLogType.GameEvent);

            yield return Timing.WaitForSeconds(1.5f);

            int restartTime = Mathf.Clamp(ConfigFile.ServerConfig.GetInt("auto_round_restart_time", 10), 5, 1000);

            if (this != null)
            {
                RpcShowRoundSummary(classlistStart, newList, leadingTeam, EscapedClassD, EscapedScientists, KilledBySCPs, restartTime, (int)RoundStart.RoundLength.TotalSeconds);
            }

            yield return Timing.WaitForSeconds(restartTime - 1);

            RpcDimScreen();
            yield return Timing.WaitForSeconds(1f);

            RoundRestart.InitiateRoundRestart();
        }
    }

    [ClientRpc]
    private void RpcShowRoundSummary(SumInfo_ClassList listStart, SumInfo_ClassList listFinish, LeadingTeam leadingTeam, int eDS, int eSc, int scpKills, int roundCd, int seconds)
    {
        _summaryActive = true;
        Timing.RunCoroutine(_ShowRoundSummary(listStart, listFinish, leadingTeam, eDS, eSc, scpKills, roundCd, seconds));
    }

    private IEnumerator<float> _ShowRoundSummary(SumInfo_ClassList list_start, SumInfo_ClassList list_finish, LeadingTeam leadingTeam, int e_ds, int e_sc, int scp_kills, int round_cd, int seconds)
    {
        string winText = string.Empty;
        if (leadingTeam == LeadingTeam.Draw)
        {
            winText = "<color=#FEFEFE> " + TranslationReader.Get("Summary", 4) + "</color>";
        }
        else if (leadingTeam == LeadingTeam.FacilityForces)
        {
            winText = "<color=red> " + TranslationReader.Get("Summary", 1) + "</color>";
        }
        else if (leadingTeam == LeadingTeam.ChaosInsurgency)
        {
            winText = "<color=#008F1E> " + TranslationReader.Get("Summary", 3) + "</color>";
        }
        else // Anomalies
        {
            winText = "<color=#0096FF> " + TranslationReader.Get("Summary", 2) + "</color>";
        }

        ui_text_who_won.text = winText;

        string timeStr = "<color=red>" + (seconds / 60) + " min " + (seconds % 60) + " sec</color>";
        string info = TranslationReader.GetFormatted("Summary", 11, "", timeStr) + "\n";

        string dStats = "<color=red>" + list_start.class_ds + "</color>/<color=red>" + e_ds + "</color>";
        info += TranslationReader.GetFormatted("Summary", 9, "", dStats) + "\n";

        string scStats = "<color=red>" + list_start.scientists + "</color>/<color=red>" + e_sc + "</color>";
        info += TranslationReader.GetFormatted("Summary", 10, "", scStats) + "\n";

        int newZombies = list_finish.zombies - list_start.zombies;
        string scpStats = "<color=red>" + list_finish.scps_except_zombies + "</color>/<color=red>" + newZombies + "</color>";
        info += TranslationReader.GetFormatted("Summary", 14, "", scpStats) + "\n";

        string warheadKillsStr = "<color=red>" + list_finish.warhead_kills + "</color>";
        string scpKillsStr = "<color=red>" + scp_kills + "</color>";
        info += TranslationReader.GetFormatted("Summary", 13, "", warheadKillsStr, scpKillsStr) + "\n";

        string totalKillsStr = "<color=red>" + Kills + "</color>";
        info += TranslationReader.GetFormatted("Summary", 15, "", totalKillsStr) + "\n";

        string restartCdStr = "<color=red>" + round_cd + "</color>";
        info += TranslationReader.GetFormatted("Summary", 16, "", restartCdStr);

        ui_text_info.text = info;

        ui_root.gameObject.SetActive(true);
        ui_text_who_won.rectTransform.localPosition = Vector3.zero;
        ui_root.GetComponent<RectTransform>().localPosition = Vector3.zero;

        float currentAlpha = 0f;
        while (currentAlpha < 1f)
        {
            currentAlpha += Time.unscaledDeltaTime;
            ui_text_header.alpha = currentAlpha;
            ui_text_info.alpha = currentAlpha;
            ui_text_who_won.alpha = currentAlpha;
            yield return 0f;
        }
    }

    [ClientRpc]
    private void RpcDimScreen()
    {
        Timing.RunCoroutine(_FadeScreenOut());
    }

    private IEnumerator<float> _FadeScreenOut()
    {
        float sTime = 0f;
        while (sTime < 1f)
        {
            sTime += Time.deltaTime;
            if (fadeOutImage != null)
            {
                fadeOutImage.color = new Color(0f, 0f, 0f, sTime);
            }
            yield return 0f;
        }
        if (fadeOutImage != null) fadeOutImage.color = Color.black;
    }

    public static bool RoundInProgress()
    {
        if (ReferenceHub.LocalHub.characterClassManager.RoundStarted)
        {
            return !singleton._roundEnded;
        }
        return false;
    }
}