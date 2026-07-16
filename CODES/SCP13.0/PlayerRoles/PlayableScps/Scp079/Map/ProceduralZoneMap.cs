using System;
using System.Collections.Generic;
using MapGeneration;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using PlayerRoles.Subroutines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class ProceduralZoneMap : MonoBehaviour, IZoneMap
	{
		public class RoomNode
		{
			public readonly Image Icon;

			public readonly List<RoomNode> SubNodes;

			public readonly RoomIdentifier Room;

			public readonly Scp079Camera CameraOverride;

			public readonly RectTransform Transform;

			public readonly TextMeshProUGUI Label;

			private readonly ProceduralZoneMap _map;

			private readonly Vector2 _halfBounds;

			private readonly RoomNode _parentNode;

			public bool Highlighted => false;

			public RoomNode(RoomIdentifier room, ProceduralZoneMap map)
			{
			}

			private RoomNode(RoomNode parent, ProceduralZoneMap map, Scp079Camera targetCam, Sprite icon, int index)
			{
			}

			private List<RoomNode> GenerateElevatorFastTravelSubnodes()
			{
				return null;
			}
		}

		[Serializable]
		private struct IconDefinition
		{
			public RoomName Room;

			public RoomShape Shape;

			public string Name;

			public Vector3 TextOffset;

			public Bounds IndicatorLimits;

			public Vector2 SubNodeFirstOffset;

			public Vector2 SubNodeNextOffset;
		}

		public static readonly Dictionary<FacilityZone, Scp079HudTranslation> ZoneTranslations;

		public static readonly Color CurrentZoneColor;

		public readonly Dictionary<RoomIdentifier, RoomNode> NodesByRoom;

		public readonly List<RoomNode> AllNodes;

		[SerializeField]
		protected TextMeshProUGUI ZoneLabel;

		[SerializeField]
		private Image _iconTemplate;

		[SerializeField]
		private Image _subNodeTemplate;

		[SerializeField]
		private Sprite _iconElevatorUp;

		[SerializeField]
		private Sprite _iconElevatorDown;

		[SerializeField]
		private IconDefinition[] _roomIcons;

		[SerializeField]
		private float _positionScale;

		[SerializeField]
		private RectTransform _rectParent;

		private const float NonExactCooldown = 1.5f;

		private Scp079InteractableBase _highlightedCamera;

		private readonly AbilityCooldown _nonExactCooldown;

		private readonly HashSet<TextMeshProUGUI> _spawnedTexts;

		private readonly List<RectTransform> _transformsToUpright;

		private readonly Queue<ProceduralZoneMap> _queuedPostProcessing;

		public static Color OtherZoneColor => default(Color);

		public static Color CurrentRoomColor => default(Color);

		public static Color HighlightedColor => default(Color);

		public bool Ready { get; private set; }

		public Bounds RectBounds { get; protected set; }

		[field: SerializeField]
		public FacilityZone Zone { get; protected set; }

		private void Update()
		{
		}

		private bool TryGetIcon(RoomIdentifier room, out IconDefinition result)
		{
			result = default(IconDefinition);
			return false;
		}

		private bool TryGetCamOfRoom(Scp079Camera curCam, RoomIdentifier room, out Scp079Camera result)
		{
			result = null;
			return false;
		}

		private void UpdateNode(Scp079Camera curCam, RoomNode node)
		{
		}

		protected virtual void PlaceRooms()
		{
		}

		protected virtual void PostProcessRooms()
		{
		}

		public void Generate()
		{
		}

		public virtual void UpdateOpened(Scp079Camera curCam)
		{
		}

		public bool TryGetCamera(out Scp079Camera target)
		{
			target = null;
			return false;
		}

		public bool TryGetCenterTransform(Scp079Camera curCam, out Vector3 center)
		{
			center = default(Vector3);
			return false;
		}

		public bool TrySetPlayerIndicator(ReferenceHub ply, RectTransform indicator, bool exact)
		{
			return false;
		}
	}
}
