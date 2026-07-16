using System;

public class BanDetails
{
    public string OriginalName { get; set; }
    public string Id { get; set; }
    public long Expires { get; set; }
    public string Reason { get; set; }
    public string Issuer { get; set; }
    public long IssuanceTime { get; set; }
    public override string ToString()
    {
        return $"{OriginalName};{Id};{Expires};{Reason};{Issuer};{IssuanceTime}";
    }
}