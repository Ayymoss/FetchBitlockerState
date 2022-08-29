using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FetchBitlocker.Shared;

public class EndPointData
{
    public string Key { get; set; }
    public DataModel DataModel { get; set; }
}
public class DataModel
{
    public string HostName { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BitLockerState State { get; set; }
}

public enum BitLockerState
{
    Protected,
    Unprotected,
    Suspended,
    Unknown
}
