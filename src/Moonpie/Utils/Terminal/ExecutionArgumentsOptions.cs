using CommandLine;

namespace Moonpie.Utils.Terminal;

public class ExecutionArgumentsOptions
{
    [Option("host", Required = false, HelpText = "Host Address")]
    public string? Host { get; set; }
    
    [Option("port", Required = false, HelpText = "Host Port")]
    public ushort? Port { get; set; }
}