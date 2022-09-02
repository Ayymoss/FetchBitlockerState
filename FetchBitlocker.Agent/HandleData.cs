using System.Diagnostics;
using System.Management.Automation;
using System.Net.Http.Json;
using System.Text.Json;
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
        var bitLockerSuspendState = PowerShell.Create();
        bitLockerSuspendState.AddScript(@"Get-CimInstance -Namespace 'ROOT/CIMV2/Security/MicrosoftVolumeEncryption' -Class Win32_EncryptableVolume -Filter ""DriveLetter='C:'"" | Invoke-CimMethod -MethodName GetSuspendCount");
        var bitLockerSuspendStateResult = Convert.ToInt32(bitLockerSuspendState.Invoke()[0].Members["SuspendCount"].Value);

        var state = bitLockerSuspendStateResult > 0 ? BitLockerState.Suspended : BitLockerState.Unknown;

        if (state == BitLockerState.Unknown)
        {
            var bitLockerState = PowerShell.Create();
            bitLockerState.AddScript(@"Get-CimInstance -Namespace 'ROOT/CIMV2/Security/MicrosoftVolumeEncryption' -Class Win32_EncryptableVolume -Filter ""DriveLetter='C:'""");
            var bitLockerStateResult = Convert.ToInt32(bitLockerState.Invoke()[0].Members["ProtectionStatus"].Value);
            
            state = bitLockerStateResult > 0 ? BitLockerState.Protected : BitLockerState.Unprotected;
        }

        return new DataModel {State = state, HostName = Environment.MachineName};
    }
}
