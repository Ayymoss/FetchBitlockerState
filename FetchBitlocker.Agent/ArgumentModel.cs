using CommandLine;

namespace FetchBitlocker.Agent;

public class ArgumentModel
{
    [Option('a', "api", Required = true, HelpText = "Shared server API Key")]
    public string ApiKey { get; set; }
    [Option('h', "host", Required = true, HelpText = "Server IP or Domain Name")]
    public string Host { get; set; }
}
