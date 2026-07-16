using System.Runtime.InteropServices;
using Mirror;
using PlayerRoles.Subroutines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieConfirmationBox : MonoBehaviour
	{
		[StructLayout((LayoutKind)0, Size = 1)]
		public struct ScpReviveBlockMessage : NetworkMessage
		{
		}

		private const float ManualDeleteDelay = 2f;

		private const float AutomaticDeleteDelay = 15f;

		private const string TranslationKey = "SCP049_HUD";

		[SerializeField]
		private Image _progressBar;

		[SerializeField]
		private GameObject _root;

		[SerializeField]
		private TMP_Text _text;

		private readonly AbilityCooldown _cooldown;

		private KeyCode TargetKey => default(KeyCode);

		private void Confirm()
		{
		}

		private void Start()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private static void ServerReceiveMessage(NetworkConnection conn)
		{
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}
	}
}
