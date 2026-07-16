public class MessageUnallowedCharsException : global::System.Exception
{
	public MessageUnallowedCharsException()
	{
	}

	public MessageUnallowedCharsException(string message)
		: base(message)
	{
	}

	public MessageUnallowedCharsException(string message, global::System.Exception inner)
		: base(message, inner)
	{
	}
}
