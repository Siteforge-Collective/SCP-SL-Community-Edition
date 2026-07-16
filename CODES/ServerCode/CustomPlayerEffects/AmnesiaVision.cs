namespace CustomPlayerEffects
{
	public class AmnesiaVision : global::CustomPlayerEffects.StatusEffectBase, global::CustomPlayerEffects.ISoundtrackMutingEffect
	{
		private float _lastTime;

		public bool MuteSoundtrack => false;

		public float LastActive => global::UnityEngine.Time.timeSinceLevelLoad - _lastTime;

		protected override void Enabled()
		{
			base.Enabled();
			_lastTime = global::UnityEngine.Time.timeSinceLevelLoad;
		}
	}
}
