using System;
using Utf8Json;

public readonly struct QueryRaReply : IEquatable<QueryRaReply>, IJsonSerializable
{
    public readonly string Text;

    public readonly bool Success;

    public readonly bool LogToConsole;

    public readonly string OverrideDisplay;

    [SerializationConstructor]
    public QueryRaReply(string text, bool success, bool logToConsole, string overrideDisplay)
    {
        Text = text;
        Success = success;
        LogToConsole = logToConsole;
        OverrideDisplay = overrideDisplay;
    }

    public bool Equals(QueryRaReply other)
    {
        if (string.Equals(Text, other.Text) && Success == other.Success && LogToConsole == other.LogToConsole)
        {
            return string.Equals(OverrideDisplay, other.OverrideDisplay);
        }
        return false;
    }

    public override bool Equals(object obj)
    {
        if (obj is QueryRaReply other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        int num = ((Text != null) ? Text.GetHashCode() : 0) * 397;
        bool success = Success;
        int num2 = (num ^ success.GetHashCode()) * 397;
        success = LogToConsole;
        return ((num2 ^ success.GetHashCode()) * 397) ^ ((OverrideDisplay != null) ? OverrideDisplay.GetHashCode() : 0);
    }

    public static bool operator ==(QueryRaReply left, QueryRaReply right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(QueryRaReply left, QueryRaReply right)
    {
        return !left.Equals(right);
    }
}
