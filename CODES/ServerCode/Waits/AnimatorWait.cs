namespace Waits
{
	public class AnimatorWait : global::Waits.Wait
	{
		public global::UnityEngine.Animator animator;

		public override global::System.Collections.Generic.IEnumerator<float> _Run()
		{
			yield return global::MEC.Timing.WaitUntilFalse(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);
		}
	}
}
