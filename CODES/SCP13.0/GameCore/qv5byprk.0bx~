using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Mirror;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils.NonAllocLINQ;

namespace GameCore
{
    public class RoundStart : NetworkBehaviour
    {
        public GameObject window;
        public GameObject forceButton;
        public TextMeshProUGUI playersNumber;
        public TextMeshProUGUI startsIn;
        public Image loadingbar;

        [SyncVar]
        public ushort players = 1488;

        [SerializeField]
        private Image _background;

        private string _roundStartText;
        private string _roundStartTextPaused;
        private bool _loaded;
        private bool _hideBackground;

        public static RoundStart singleton;
        public static bool LobbyLock;

        [SyncVar]
        public short Timer = -2;

        internal static readonly Stopwatch RoundStartTimer;

        public static bool RoundStarted => singleton != null && singleton.Timer == -2;

        public static TimeSpan RoundLength => RoundStartTimer.Elapsed;

        public short NetworkTimer
        {
            get => Timer;
            set
            {
                if (NetworkServer.active)
                {
                    Timer = value;
                }
            }
        }

        static RoundStart()
        {
            RoundStartTimer = new Stopwatch();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            RoundStartTimer.Reset();
        }

        private void Start()
        {
            GetComponent<RectTransform>().localPosition = Vector3.zero;
        }

        private void Awake()
        {
            if (singleton != null && singleton != this)
            {
                DestroyImmediate(gameObject);
                return;
            }
            singleton = this;
            PlayerRoleManager.OnRoleChanged += OnRoleChanged;
        }

        private void OnDestroy()
        {
            PlayerRoleManager.OnRoleChanged -= OnRoleChanged;
            singleton = null;
        }

        private void Update()
        {
            if (window == null)
                return;

            window.SetActive(Timer != -2);

            if (window.activeSelf)
            {
                if (_background != null)
                {
                    Color color = _background.color;
                    color.a = _hideBackground ? 0f : 1f;
                    _background.color = color;
                }

                if (!_loaded)
                {
                    _loaded = true;
                    _roundStartText = TranslationReader.Get("Facility", 38, "ROUND STARTS IN: <color=yellow>{0}</color>s");
                    _roundStartTextPaused = TranslationReader.Get("Facility", 39, "ROUND START IS <color=#FF5050>PAUSED</color>");
                }

                if (loadingbar != null)
                {
                    if (Timer >= 0)
                    {
                        loadingbar.fillAmount = Timer / 20f;
                    }
                    else
                    {
                        loadingbar.fillAmount = 1f;
                    }
                }

                if (startsIn != null)
                {
                    if (Timer >= 0)
                    {
                        startsIn.text = _roundStartText.Replace("{0}", Timer.ToString());
                    }
                    else
                    {
                        startsIn.text = _roundStartTextPaused;
                    }
                }

                int count = ReferenceHub.AllHubs.Count(x => x.Mode == CentralAuth.ClientInstanceMode.ReadyClient || x.Mode == CentralAuth.ClientInstanceMode.Host);
                if (players != count)
                {
                    players = (ushort)count;
                    playersNumber.text = players.ToString();
                }
            }
        }

        public void ShowButton()
        {
            forceButton.SetActive(true);
        }

        public void UseButton()
        {
            forceButton.SetActive(false);
            CharacterClassManager.ForceRoundStart();
        }

        private void OnRoleChanged(ReferenceHub userHub, PlayerRoleBase prevRole, PlayerRoleBase newRole)
        {
            if (userHub.isLocalPlayer)
            {
                if (newRole != null)
                {
                    _hideBackground = newRole is FpcStandardRoleBase;
                }
            }
        }
    }
}