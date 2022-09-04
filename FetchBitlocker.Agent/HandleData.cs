using System.Management;
using System.Net.Http.Json;
using FetchBitlocker.Shared;

namespace FetchBitlocker.Agent;

public static class HandleData
{
    internal static async Task SendModelData(EndPointData endPointData, string hostname)
    {
        var client = new HttpClient();
        var response = await client.PostAsJsonAsync($"http://{hostname}:5000/api/BitLocker", endPointData);
        Console.WriteLine($"Server Response: {await response.Content.ReadAsStringAsync()}");
        response.EnsureSuccessStatusCode();
    }

    internal static DataModel GetBitlockerState()
    {
#pragma warning disable CA1416
        var path = new ManagementPath(@"\ROOT\CIMV2\Security\MicrosoftVolumeEncryption")
        {
            ClassName = "Win32_EncryptableVolume",
            Server = Environment.MachineName
        };
        var scope = new ManagementScope(path);
        var objectSearcher = new ManagementClass(scope, path, new ObjectGetOptions());

        foreach (var instance in objectSearcher.GetInstances())
        {
            if (instance["DriveLetter"].ToString() != "C:") continue;
            if (instance["ProtectionStatus"].ToString() == "0" && instance["ConversionStatus"].ToString() == "0")
            {
                return new DataModel {State = BitLockerState.Unprotected, HostName = Environment.MachineName};
            }

            if (instance["ProtectionStatus"].ToString() == "0" && instance["ConversionStatus"].ToString() == "1")
            {
                return new DataModel {State = BitLockerState.Suspended, HostName = Environment.MachineName};
            }

            if (instance["ProtectionStatus"].ToString() == "1" && instance["ConversionStatus"].ToString() == "1")
            {
                return new DataModel {State = BitLockerState.Protected, HostName = Environment.MachineName};
            }
        }

        return new DataModel {State = BitLockerState.Unknown, HostName = Environment.MachineName};
#pragma warning restore CA1416
    }
}
