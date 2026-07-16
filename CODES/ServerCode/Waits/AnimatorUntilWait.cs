namespace Waits
{
	public class AnimatorUntilWait : global::Waits.UntilWait
	{
		public global::UnityEngine.Animator animator;

		protected override bool Predicate()
		{
			return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
		}
	}
}
