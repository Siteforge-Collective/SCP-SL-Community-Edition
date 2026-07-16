using System;
using System.Collections.Generic;
using PlayerRoles.FirstPersonControl.Thirdperson;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114FakeModelManager : StandardSubroutine<Scp3114Role>
	{
		[Serializable]
		private class MaterialAnimation
		{
			public float ProgressSpeed;

			public AnimationCurve FadeOverProgress;
		}

		private struct MappedBone
		{
			public Transform Original;

			public Transform Tracked;

			public Vector3 PrevPos;

			public Quaternion PrevRot;
		}

		[Serializable]
		public class MaterialPair
		{
			public Material Original;

			public Material Disguise;

			public Material Reveal;

			public Material FromType(VariantType type)
			{
				return null;
			}
		}

		public enum VariantType
		{
			Original = 0,
			Disguise = 1,
			Reveal = 2
		}

		[SerializeField]
		private MaterialAnimation _disguiseAnimation;

		[SerializeField]
		private MaterialAnimation _revealAnimation;

		[SerializeField]
		private MaterialPair[] _materialPairs;

		[SerializeField]
		private float _teammateProgressMultiplier;

		private static Dictionary<Material, MaterialPair> _dictionarizedPairs;

		private Scp3114Identity.DisguiseStatus _lastActiveStatus;

		private VariantType _lastMaterialType;

		private float _animProgress;

		private bool _ownModelMapped;

		private bool _trackBones;

		private HumanCharacterModel _lastModel;

		private Scp3114Model _ownModel;

		private bool _wasTeammatePerpsective;

		private const string RigPrefix = "mixamorig:";

		private static readonly int ProgressHash;

		private readonly Dictionary<Material, Material> _materialInstances;

		private readonly Dictionary<GameObject, HumanCharacterModel> _modelInstances;

		private readonly Dictionary<HumanCharacterModel, List<MappedBone>> _mappedBones;

		private readonly Dictionary<string, Transform> _ownModelBones;

		private readonly List<Material> _materialsInUse;

		private float AnimProgress
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		private bool TeammatePerspective => false;

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		private void UpdateModelMaterials()
		{
		}

		private void OnIdentityChanged()
		{
		}

		private void UpdatePerspective()
		{
		}

		private void UpdateVisibilityAll()
		{
		}

		private void Update()
		{
		}

		private void LateUpdate()
		{
		}

		private bool TryTrackBones()
		{
			return false;
		}

		private bool TrySetModel(RoleTypeId role, VariantType variant)
		{
			return false;
		}

		private bool TryCreateModel(GameObject template, out HumanCharacterModel model)
		{
			model = null;
			return false;
		}

		private void RestoreBones(HumanCharacterModel model)
		{
		}

		private void SetModelMaterials(HumanCharacterModel model, VariantType matType)
		{
		}

		public static Material GetVariant(Material original, VariantType matType)
		{
			return null;
		}
	}
}
