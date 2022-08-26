namespace FetchBitlocker.Shared;

public class DataModel
{
    public string HostName { get; set; }
    public BitLockerState State { get; set; }
}

public enum BitLockerState
{
    Protected,
    Unprotected,
    Suspended,
    Unknown
}
