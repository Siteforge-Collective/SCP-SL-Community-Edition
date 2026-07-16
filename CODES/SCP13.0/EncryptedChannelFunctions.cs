using Mirror;

public static class EncryptedChannelFunctions
{
    public static void SerializeEncryptedMessageOutside(this NetworkWriter writer, EncryptedChannelManager.EncryptedMessageOutside value)
    {
        writer.WriteByte((byte)value.Level);
        writer.WriteArray(value.Data);
    }

    public static EncryptedChannelManager.EncryptedMessageOutside DeserializeEncryptedMessageOutside(this NetworkReader reader)
    {
        return new EncryptedChannelManager.EncryptedMessageOutside((EncryptedChannelManager.SecurityLevel)reader.ReadByte(), reader.ReadArray<byte>());
    }
}
