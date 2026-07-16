using Utf8Json;

public readonly struct RemoteAdminResponse : IJsonSerializable
{
    public readonly string Content;

    public readonly bool IsSuccess;

    public readonly bool LogInConsole;

    public readonly string OverrideDisplay;

    [SerializationConstructor]
    public RemoteAdminResponse(string content, bool isSuccess, bool logInConsole, string overrideDisplay)
    {
        Content = content;
        IsSuccess = isSuccess;
        LogInConsole = logInConsole;
        OverrideDisplay = overrideDisplay;
    }
}
