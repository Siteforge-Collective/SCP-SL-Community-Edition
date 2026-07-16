using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AudioPooling;
using GameCore;
using MapGeneration;
using Mirror;
using Mirror.RemoteCalls;
using PluginAPI.Events;
using UnityEngine;
using Utils.NonAllocLINQ;
using VoiceChat;

namespace PlayerRoles.Voice
{
    public class Intercom : NetworkBehaviour
    {
        private static Intercom _singleton;

        private static bool _singletonSet;

        private readonly Stopwatch _sustain = new Stopwatch();

        private readonly Stopwatch _clipSw = Stopwatch.StartNew();

        private readonly HashSet<ReferenceHub> _adminOverrides = new HashSet<ReferenceHub>();

        private ReferenceHub _curSpeaker;

        private float _cooldownTime;

        private float _speechTime;

        private float _rangeSqr;

        private Vector3 _worldPos;

        [SerializeField]
        private float _range;

        [SerializeField]
        private float _wakeupTime;

        [SerializeField]
        private float _sustainTime;

        [SerializeField]
        private AudioClip _startClip;

        [SerializeField]
        private AudioClip _endClip;

        [SerializeField]
        private float _clipCooldown;

        [SyncVar]
        private byte _state;

        [SyncVar]
        private double _nextTime;

        public static IntercomState State
        {
            get
            {
                if (_singletonSet)
                {
                    return (IntercomState)_singleton._state;
                }
                return IntercomState.NotFound;
            }
            set
            {
                if (!_singletonSet || !NetworkServer.active)
                {
                    return;
                }
                Intercom singleton = _singleton;
                switch (value)
                {
                    case IntercomState.InUse:
                        singleton._nextTime = NetworkTime.time + (double)singleton._speechTime;
                        break;
                    case IntercomState.Starting:
                        singleton._nextTime = NetworkTime.time + (double)singleton._wakeupTime;
                        Intercom.OnServerBeginUsage?.Invoke(singleton._curSpeaker);
                        singleton.RpcPlayClip(state: true);
                        break;
                    case IntercomState.Cooldown:
                        singleton.RpcPlayClip(state: false);
                        if (singleton._curSpeaker != null && singleton._curSpeaker.serverRoles.BypassMode)
                        {
                            singleton._nextTime = 0.0;
                        }
                        else
                        {
                            singleton._nextTime = NetworkTime.time + (double)singleton._cooldownTime;
                        }
                        break;
                }
                singleton._state = (byte)value;
            }
        }

        public float RemainingTime => Mathf.Max((float)(_nextTime - NetworkTime.time), 0f);

        public bool BypassMode
        {
            get
            {
                if (State == IntercomState.InUse)
                {
                    return _nextTime == 0.0;
                }
                return false;
            }
        }

        public static event Action<ReferenceHub> OnServerBeginUsage;

        private void Awake()
        {
            if (_singletonSet)
            {
                throw new InvalidOperationException("Multiple instances of Intercom detected. Last name: '" + base.name + "'");
            }
            _singleton = this;
            _singletonSet = true;
            ConfigFile.OnConfigReloaded = (Action)Delegate.Combine(ConfigFile.OnConfigReloaded, new Action(ReloadConfigs));
            SeedSynchronizer.OnMapGenerated += SetupPos;
            ReloadConfigs();
            if (SeedSynchronizer.MapGenerated)
            {
                SetupPos();
            }
        }

        private void Update()
        {
            if (!NetworkServer.active)
            {
                return;
            }
            switch (State)
            {
                case IntercomState.Ready:
                    {
                        if (ReferenceHub.AllHubs.TryGetFirst(CheckPlayer, out var first))
                        {
                            _curSpeaker = first;
                            State = IntercomState.Starting;
                        }
                        break;
                    }
                case IntercomState.Starting:
                    if (!(_nextTime > NetworkTime.time))
                    {
                        _sustain.Restart();
                        State = IntercomState.InUse;
                    }
                    break;
                case IntercomState.InUse:
                    {
                        bool flag;
                        if (_curSpeaker != null && CheckPlayer(_curSpeaker))
                        {
                            flag = true;
                            _sustain.Restart();
                        }
                        else
                        {
                            flag = _sustain.Elapsed.TotalSeconds < (double)_sustainTime;
                        }
                        if (!flag || (!(_nextTime > NetworkTime.time) && _nextTime != 0.0))
                        {
                            State = IntercomState.Cooldown;
                        }
                        break;
                    }
                case IntercomState.Cooldown:
                    if (!(_nextTime > NetworkTime.time))
                    {
                        State = IntercomState.Ready;
                    }
                    break;
            }
        }

        private void OnDestroy()
        {
            ConfigFile.OnConfigReloaded = (Action)Delegate.Remove(ConfigFile.OnConfigReloaded, new Action(ReloadConfigs));
            SeedSynchronizer.OnMapGenerated -= SetupPos;
            _singletonSet = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(base.transform.position, _range);
        }

        private void SetupPos()
        {
            _worldPos = base.transform.position;
            _rangeSqr = _range * _range;
        }

        private void ReloadConfigs()
        {
            _cooldownTime = ConfigFile.ServerConfig.GetFloat("intercom_cooldown", 120f);
            _speechTime = ConfigFile.ServerConfig.GetFloat("intercom_max_speech_time", 20f);
        }

        private bool CheckRange(ReferenceHub hub)
        {
            if (hub.roleManager.CurrentRole is HumanRole humanRole)
            {
                return (humanRole.FpcModule.Position - _worldPos).sqrMagnitude < _rangeSqr;
            }
            return false;
        }

        private bool CheckPlayer(ReferenceHub hub)
        {
            PlayerRoleBase currentRole = hub.roleManager.CurrentRole;
            if (!CheckRange(hub) || !(currentRole as HumanRole).VoiceModule.ServerIsSending || VoiceChatMutes.IsMuted(hub, checkIntercom: true))
            {
                return false;
            }
            return EventManager.ExecuteEvent(new PlayerUsingIntercomEvent(hub, State));
        }

        [ClientRpc]
        private void RpcPlayClip(bool state)
        {
            if (!(_clipSw.Elapsed.TotalSeconds < (double)_clipCooldown))
            {
                AudioSourcePoolManager.PlaySound(state ? _startClip : _endClip, null, 1f, 1f, FalloffType.Exponential, AudioMixerChannelType.VoiceChat, 0f);
                _clipSw.Restart();
            }
        }

        public static bool CheckPerms(ReferenceHub hub)
        {
            if (!_singletonSet)
            {
                return false;
            }
            if (VoiceChatMutes.IsMuted(hub, checkIntercom: true))
            {
                return false;
            }
            bool flag = State == IntercomState.InUse;
            if (!HasOverride(hub))
            {
                if (flag && _singleton.CheckRange(hub))
                {
                    return _singleton._curSpeaker == hub;
                }
                return false;
            }
            return true;
        }

        public static bool HasOverride(ReferenceHub hub)
        {
            return _singleton._adminOverrides.Contains(hub);
        }

        public static bool TrySetOverride(ReferenceHub ply, bool newState)
        {
            if (!_singletonSet || ply == null)
            {
                return false;
            }
            HashSet<ReferenceHub> adminOverrides = _singleton._adminOverrides;
            if (!newState)
            {
                return adminOverrides.Remove(ply);
            }
            adminOverrides.Add(ply);
            return true;
        }
    }
}
