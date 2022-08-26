using System.Security.Principal;

namespace FetchBitlocker.Agent;

public static class TestSendAgent
{
    public static async Task Main(string[] args)
    {
        args = args.Append("---EXIT").ToArray();
        var exit = false;

        if (args[0] != "--hostname")
        {
            Console.WriteLine("Argument Required: --hostname <hostname/ip>");
            exit = true;
        }

        if (!IsAdministrator())
        {
            Console.WriteLine("Elevation Required: You must run this as an administrator");
            exit = true;
        }

        if (exit)
        {
            Console.ReadKey();
            Environment.Exit(1);
        }

        await HandleData.SendModelData(HandleData.GetBitlockerState(), args[1]);
    }

    private static bool IsAdministrator() =>
        new WindowsPrincipal(WindowsIdentity.GetCurrent())
            .IsInRole(WindowsBuiltInRole.Administrator);
}
