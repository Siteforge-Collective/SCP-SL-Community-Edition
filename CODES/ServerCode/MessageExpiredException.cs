public class MessageExpiredException : global::System.Exception
{
	public MessageExpiredException()
	{
	}

	public MessageExpiredException(string message)
		: base(message)
	{
	}

	public MessageExpiredException(string message, global::System.Exception inner)
		: base(message, inner)
	{
	}
}
