namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079GeneratorNotification : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079SimpleNotification
	{
		public static readonly global::System.Collections.Generic.HashSet<global::MapGeneration.Distributors.Scp079Generator> TrackedGens = new global::System.Collections.Generic.HashSet<global::MapGeneration.Distributors.Scp079Generator>();

		private readonly global::MapGeneration.Distributors.Scp079Generator _generator;

		private readonly global::System.Text.StringBuilder _emptyBuilder;

		private readonly global::System.Diagnostics.Stopwatch _activeStopwatch;

		private const string UnknownRoom = "UNKNOWN";

		private const string Format = "<color=red>0m 00s - {0}</color>";

		private const int MinuteDigit = 11;

		private const int TensDigit = 14;

		private const int SecsDigit = 15;

		private const int CharOffset = 48;

		private const int BlinkRate = 9;

		private const float BlinkDuration = 2.5f;

		private float _opacity;

		private bool IsActivating
		{
			get
			{
				if (_generator != null)
				{
					return _generator.Activating;
				}
				return false;
			}
		}

		public override float Opacity
		{
			get
			{
				float num = (IsActivating ? 1f : (-1f));
				return _opacity = global::UnityEngine.Mathf.Min(1f, _opacity + num * global::UnityEngine.Time.deltaTime / 0.18f);
			}
		}

		public override bool Delete
		{
			get
			{
				if (base.Delete)
				{
					TrackedGens.Remove(_generator);
					return true;
				}
				return false;
			}
		}

		protected override global::System.Text.StringBuilder WrittenText
		{
			get
			{
				float num = (float)_activeStopwatch.Elapsed.TotalSeconds;
				if (num < 2.5f && global::UnityEngine.Mathf.RoundToInt(num * 9f) % 2 != 0)
				{
					return _emptyBuilder;
				}
				global::System.Text.StringBuilder writtenText = base.WrittenText;
				if (!IsActivating)
				{
					return writtenText;
				}
				int remainingTime = _generator.RemainingTime;
				OverrideStringBuilder(writtenText, 11, remainingTime / 60 + 48);
				OverrideStringBuilder(writtenText, 14, remainingTime % 60 / 10 + 48);
				OverrideStringBuilder(writtenText, 15, remainingTime % 60 % 10 + 48);
				return writtenText;
			}
		}

		public Scp079GeneratorNotification(global::MapGeneration.Distributors.Scp079Generator generator, bool skipAnimation)
			: base($"<color=red>0m 00s - {GetGeneratorCamera(generator)}</color>")
		{
			_activeStopwatch = (skipAnimation ? new global::System.Diagnostics.Stopwatch() : global::System.Diagnostics.Stopwatch.StartNew());
			_emptyBuilder = new global::System.Text.StringBuilder();
			_generator = generator;
			_opacity = 1f;
		}

		private static void OverrideStringBuilder(global::System.Text.StringBuilder sb, int place, int character)
		{
			if (sb.Length > place)
			{
				sb[place] = (char)character;
			}
		}

		private static string GetGeneratorCamera(global::MapGeneration.Distributors.Scp079Generator gen)
		{
			global::UnityEngine.Vector3 position = gen.transform.position;
			global::MapGeneration.RoomIdentifier roomIdentifier = global::MapGeneration.RoomIdUtils.RoomAtPositionRaycasts(position);
			if (roomIdentifier == null)
			{
				return "UNKNOWN";
			}
			bool flag = false;
			float num = 0f;
			global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera scp079Camera = null;
			global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera scp079Camera2 = null;
			foreach (global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase allInstance in global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase.AllInstances)
			{
				if (allInstance is global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera scp079Camera3 && scp079Camera3.Room == roomIdentifier)
				{
					float sqrMagnitude = (scp079Camera3.Position - position).sqrMagnitude;
					if (scp079Camera3.IsMain)
					{
						scp079Camera2 = scp079Camera3;
					}
					if ((!flag || !(sqrMagnitude > num)) && (!global::UnityEngine.Physics.Linecast(scp079Camera3.Position, position, out var hitInfo, 1) || !(hitInfo.collider.GetComponentInParent<global::MapGeneration.Distributors.Scp079Generator>() != gen)))
					{
						scp079Camera = scp079Camera3;
						flag = true;
						num = sqrMagnitude;
					}
				}
			}
			if (!flag)
			{
				if (!(scp079Camera2 != null))
				{
					return "UNKNOWN";
				}
				return scp079Camera2.Label;
			}
			return scp079Camera.Label;
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += delegate(ReferenceHub x, global::PlayerRoles.PlayerRoleBase y, global::PlayerRoles.PlayerRoleBase z)
			{
				if (x.isLocalPlayer)
				{
					TrackedGens.Clear();
				}
			};
		}
	}
}
