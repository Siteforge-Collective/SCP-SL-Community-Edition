namespace Hints
{
	public abstract class FormattableHint<THint> : global::Hints.Hint where THint : global::Hints.FormattableHint<THint>
	{
		protected FormattableHint(global::Hints.HintParameter[] parameters, global::Hints.HintEffect[] effects, float durationScalar = 1f)
			: base(parameters, effects, durationScalar)
		{
		}
	}
}
