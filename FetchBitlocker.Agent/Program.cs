using System.Security.Principal;
using CommandLine;
using FetchBitlocker.Shared;

namespace FetchBitlocker.Agent;

public static class TestSendAgent
{
    public static async Task Main(string[] args)
    {
#if !DEBUG
        var parsedArgs = Parser.Default.ParseArguments<ArgumentModel>(args)
            .WithNotParsed(o =>
            {
                Console.WriteLine("Required arguments not met.");
                Environment.Exit(1);
            });
#endif
#if DEBUG
        var parsedArgs = new ArgumentModel()
        {
            ApiKey = "0025CC72-9C65-47CD-BDFE-BAF9E3FC52C9",
            Host = "localhost"
        };
#endif
        var exit = false;

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

        var dataModel = HandleData.GetBitlockerState();
#if !DEBUG
        var endPointData = new EndPointData
        {
            Key = parsedArgs.Value.ApiKey,
            DataModel = dataModel
        };
        await HandleData.SendModelData(endPointData, parsedArgs.Value.Host);
#endif
#if DEBUG
        var endPointData = new EndPointData
        {
            Key = parsedArgs.ApiKey,
            DataModel = dataModel
        };
        await HandleData.SendModelData(endPointData, parsedArgs.Host);
#endif
    }

    private static bool IsAdministrator() =>
        new WindowsPrincipal(WindowsIdentity.GetCurrent())
            .IsInRole(WindowsBuiltInRole.Administrator);
}
