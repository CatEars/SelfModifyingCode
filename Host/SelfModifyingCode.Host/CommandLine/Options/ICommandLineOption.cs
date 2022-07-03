namespace SelfModifyingCode.Host.CommandLine.Options;

public interface ICommandLineOption
{

    string? ShortFlag => null;

    string? Description => null;

    string? SampleUsage => null;
    
    string LongFlag { get; }
    
    string Name { get; }
    
    bool IsMandatory { get; }

    CommandLineOptions Apply(CommandLineOptions current, string? argumentValue);
    
}