using CustomRendering;
using DeathAnimations;
using PlayerRoles.PlayableScps.HUDs;
using PlayerRoles.PlayableScps.Scp939.Mimicry;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939Hud : ViewmodelScpHud
	{
		[SerializeField]
		private GameObject _hudRoot;

		[SerializeField]
		private GameObject _mimicryMenuRoot;

		[SerializeField]
		private TextMeshProUGUI _lungeReadyText;

		[SerializeField]
		private AbilityHud _amnesticCloudPlacedIcon;

		[SerializeField]
		private AbilityHud _amnesticCloudBuildupIcon;

		[SerializeField]
		private AbilityHud _mimicryCooldown;

		[SerializeField]
		private ScpWarningHud _warningElement;

		[SerializeField]
		private Volume _postProcessVolume;

		[SerializeField]
		private float _blurAdditive;

		[SerializeField]
		private float _effectsLerpSpeed;

		private EnvironmentalMimicry _envMimicry;

		private MimicPointController _mimicPoint;

		private MimicryRecorder _recorder;

		private Scp939VisibilityController _visController;

		private Scp939LungeAbility _lungeAbility;

		private Scp939AmnesticCloudAbility _cloudAbility;

		private FogEffect _ppFog;

		private BlurEffect _ppBlur;

		private Grayscale _ppGrayscale;

		private const float TextFadeSpeed = 5f;

		private const float WarningDuration = 5.5f;

		private const float ProlongedUpdateTime = 0.4f;

		private const string DefaultLungeFormat = "Press {0} or {1} to Lunge.";

		private void LateUpdate()
		{
		}

		private void UpdateAmnesticCloud(bool buildupHidden)
		{
		}

		private void LerpEffects(float lerp)
		{
		}

		private void ShowWarning(Scp939HudTranslation val)
		{
		}

		private void LerpEffect(DistanceEffect fx, float target, float lerp)
		{
		}

		private string ActionKeyName(ActionName an)
		{
			return null;
		}

		internal override void Init(ReferenceHub hub)
		{
		}

		internal override void OnDied()
		{
		}

		protected override void OnDestroy()
		{
		}
	}
}
