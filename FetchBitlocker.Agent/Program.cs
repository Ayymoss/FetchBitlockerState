using System.Security.Principal;

namespace FetchBitlocker.Agent;

public static class TestSendAgent
{
    public static async Task Main(string[] args)
    {
        args = args.Append("---EXIT").ToArray();
        
        if (args[0] == "---EXIT")
        {
            Console.WriteLine("Argument Required: --hostname <hostname/ip>");
            Console.ReadKey();
            Environment.Exit(1);
        }

        var exit = false;
        if (args[0] != "--hostname")
        {
            Console.WriteLine("Argument Required: --hostname <hostname/ip>");
            exit = true;
        }

        if (!IsAdministrator())
        {
            Console.WriteLine("You must run this as an administrator.");
            exit = true;
        }
        else
        {
            await HandleData.SendModelData(HandleData.GetBitlockerState(), args[1]);
        }

        if (exit)
        {
            Console.ReadKey();
        }
    }

    private static bool IsAdministrator() =>
        new WindowsPrincipal(WindowsIdentity.GetCurrent())
            .IsInRole(WindowsBuiltInRole.Administrator);
}
