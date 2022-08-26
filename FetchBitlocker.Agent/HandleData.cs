using System.Diagnostics;
using System.Net.Http.Json;
using FetchBitlocker.Shared;

namespace FetchBitlocker.Agent;

public static class HandleData
{
    internal static async Task SendModelData(DataModel dataModel)
    {
        var client = new HttpClient();
        var response = await client.PostAsJsonAsync("http://localhost:5167/BitLocker/BitLocker", dataModel);
        Console.WriteLine($"Response {await response.Content.ReadAsStringAsync()}");
    }

    internal static DataModel GetBitlockerState()
    {
        var p = new Process();

        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.FileName = "manage-bde";
        p.StartInfo.Arguments = " -status";
        p.Start();
        var output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();

        var strArr = output.Split("\n");
        var conversion = string.Empty;
        var protection = string.Empty;
        foreach (var line in strArr)
        {
            if (line.Contains("Conversion Status"))
            {
                conversion = line.Replace(" ", "").Split(":")[1];
            }

            if (line.Contains("Protection Status"))
            {
                protection = line.Replace(" ", "").Split(":")[1];
            }
        }

        BitLockerState state;
        if (conversion.Contains("FullyDecrypted") && protection.Contains("ProtectionOff"))
        {
            state = BitLockerState.Unprotected;
        }
        else if (conversion.Contains("UsedSpaceOnlyEncrypted") && protection.Contains("ProtectionOff"))
        {
            state = BitLockerState.Suspended;
        }
        else if (conversion.Contains("UsedSpaceOnlyEncrypted") && protection.Contains("ProtectionOn"))
        {
            state = BitLockerState.Protected;
        }
        else
        {
            state = BitLockerState.Unknown;
        }


        return new DataModel {State = state};
    }
}
