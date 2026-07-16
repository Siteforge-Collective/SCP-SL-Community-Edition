using System.Diagnostics;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079GeneratorWarningGui : Scp079GuiElementBase
	{
		[SerializeField]
		private AudioClip _warningSound;

		[SerializeField]
		private float _headerCooldown;

		private int _lastAmount;

		private string _headerText;

		private readonly Stopwatch _headerStopwatchTimer;

		private const string HeaderFormat = "<color=red>{0}</color>";

		private void Awake()
		{
		}

		private void Update()
		{
		}
	}
}
