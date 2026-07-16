namespace Security
{
	public class DummyRateLimit : global::Security.RateLimit
	{
		public DummyRateLimit()
			: base(0, 0f)
		{
		}
	}
}
